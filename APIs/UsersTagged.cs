using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Azure.WebJobs.Extensions.Sql;
using Microsoft.Extensions.Logging;

namespace Tag.Sample
{
    public static class UsersTagged
    {
        [FunctionName("UsersTagged")]
        public static async Task Run(
            [SqlTrigger("[Tag].[Moves]", "SqlConnectionString")] IReadOnlyList<SqlChange<UserMove>> userUpdates,
            IBinder binder, // imperative bindings
            [SignalR(HubName = "scoreboard")]IAsyncCollector<SignalRMessage> signalRMessages, // signalR output binding
            ILogger log)
        {
            log.LogInformation("SQL trigger function processed a request.");

            foreach (SqlChange<UserMove> userChange in userUpdates)
            {
                // only for inserts
                if (userChange.Operation == SqlChangeOperation.Insert)
                {
                    UserMove userMove = userChange.Item;

                    // get other users on that square
                    var sqlUsersBinding = new SqlAttribute(
                        connectionStringSetting: "SqlConnectionString",
                        commandText: "SELECT [UserId], '', UserName, TokenColor, XLocation, YLocation FROM Tag.Users WHERE XLocation = @XLocation AND YLocation = @YLocation AND UserId != @UserId",
                        commandType: System.Data.CommandType.Text,
                        parameters: $"@XLocation={userMove.XLocation},@YLocation={userMove.YLocation},@UserId={userMove.UserId}"
                    );
                    IEnumerable<UserInfo> taggedUsers = await binder.BindAsync<IEnumerable<UserInfo>>(sqlUsersBinding);

                    if (taggedUsers.Count() > 0)
                    {
                        log.LogInformation($"Found {taggedUsers.Count()} users on square {userMove.XLocation},{userMove.YLocation}");

                        // select a user at random to add a rainbow to
                        int randomUser = new Random().Next(taggedUsers.Count());
                        UserInfo userToTag = taggedUsers.ElementAt(randomUser);

                        log.LogInformation($"Adding rainbow 🌈 to user {userToTag.UserId}");

                        // run stored procedure to add rainbow to user
                        var sqlTagBinding = new SqlAttribute(
                            connectionStringSetting: "SqlConnectionString",
                            commandText: "Tag.TagUser",
                            commandType: System.Data.CommandType.StoredProcedure,
                            parameters: $"@UserIdInput={userToTag.UserId}"
                        );
                        
                        IEnumerable<StepValidation> tagComplete = await binder.BindAsync<IEnumerable<StepValidation>>(sqlTagBinding);

                        await signalRMessages.AddAsync(
                            new SignalRMessage
                            {
                                // the message will only be sent to this user ID
                                UserId = userToTag.UserId.ToString(),
                                Target = "tagged",
                                Arguments = new [] { "user tagged" }
                            }
                        );
                    }
                    
                }
            }
        }
    }
}
