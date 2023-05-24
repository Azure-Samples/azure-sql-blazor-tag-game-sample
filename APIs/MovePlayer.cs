using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Tag.Sample
{
    public static class MovePlayer
    {
        [FunctionName("MovePlayer")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "playermove/{direction:alpha}")] HttpRequest req,
            string direction,
            IBinder binder,
            [Sql(commandText: "SELECT [UserId], [Red],[Orange],[Yellow],[Green],[Blue],[Purple],[Rainbow] FROM [Tag].[Scoreboard] WHERE [UserId] = @UserId",
                connectionStringSetting:"SqlConnectionString", parameters:"@UserId={Query.userid}")] IEnumerable<UserScores> originalScores,
            [Sql(commandText: "Tag.Scoreboard", connectionStringSetting: "SqlConnectionString")] IAsyncCollector<UserScores> newScoreTotals,
            ILogger log)
        {
            // get UserId and Passkey from header
            Guid userId = Guid.Parse(req.Headers["x-tag-userid"]);
            Guid passkey = Guid.Parse(req.Headers["x-tag-passkey"]);

            // validate user
            var sqlUserValidationAttribute = new SqlAttribute(connectionStringSetting: "SqlConnectionString",
                    commandText: "[Tag].[ValidateUser]",
                    commandType: System.Data.CommandType.StoredProcedure,
                    parameters: $"@UserIdInput={userId},@PasskeyInput={passkey}"
                );
            var userValidation = await binder.BindAsync<IEnumerable<StepValidation>>(sqlUserValidationAttribute);
            if (userValidation.FirstOrDefault().StepValidated != 1)
            {
                return new BadRequestObjectResult("Invalid user");
            }

            // check if direction is in up, down, left, right
            if (direction != "up" && direction != "down" && direction != "left" && direction != "right")
            {
                return new BadRequestObjectResult("Invalid direction");
            }

            // get userinfo from body
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            UserInfo userdata = JsonConvert.DeserializeObject<UserInfo>(requestBody);
            UserScores scores = new UserScores(userId);

            // move user
            switch (direction)
            {
                case "up":
                    userdata.YLocation += 1;
                    break;
                case "down":
                    userdata.YLocation -= 1;
                    break;
                case "left":
                    userdata.XLocation -= 1;
                    break;
                case "right":
                    userdata.XLocation += 1;
                    break;
            }

            // find out how many users they tagged
            var sqlMoveAttribute = new SqlAttribute(connectionStringSetting: "SqlConnectionString",
                    commandText: "[Tag].[MoveUser]",
                    commandType: System.Data.CommandType.StoredProcedure,
                    parameters: $"@UserIdInput={userId},@XLocation={userdata.XLocation},@YLocation={userdata.YLocation}"
                );
            var moved = await binder.BindAsync<IEnumerable<UserScores>>(sqlMoveAttribute);

            // you can only gain points when your token isn't white
            if (moved.Count() > 0 && userdata.TokenColor != "white")
            {
                scores = moved.FirstOrDefault();
            }

            // update their overall score
            UserScores newScoreTotal = originalScores.FirstOrDefault();
            newScoreTotal.AddScores(scores);
            await newScoreTotals.AddAsync(newScoreTotal);
            await newScoreTotals.FlushAsync();

            // prepare the response
            MoveResult moveResult = new MoveResult();
            moveResult.XLocation = userdata.XLocation;
            moveResult.YLocation = userdata.YLocation;
            moveResult.PointsEarned = scores;

            return new OkObjectResult(moveResult);
        }
    }
}
