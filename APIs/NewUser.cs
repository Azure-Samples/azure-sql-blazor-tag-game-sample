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
    public static class NewUser
    {
        // input binding to storedproc creating a newuser
        // output binding to update user with passkey
        [FunctionName("NewUser")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "newuser")] HttpRequest req,
            [Sql(commandText: "Tag.NewUser", commandType: System.Data.CommandType.StoredProcedure, connectionStringSetting: "SqlConnectionString")] IEnumerable<UserInfo> newUserResult,
            [Sql(commandText: "Tag.Users", connectionStringSetting: "SqlConnectionString")] IAsyncCollector<UserInfo> users,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string passkey = "";
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            log.LogInformation(requestBody);

            dynamic data = JsonConvert.DeserializeObject(requestBody);
            passkey = data?.Passkey;

            if (string.IsNullOrEmpty(passkey))
            {
                return new BadRequestObjectResult("Please pass a Passkey in the request body");
            }
            
            UserInfo newUser = newUserResult.First();
            newUser.Passkey = Guid.Parse(passkey);

            await users.AddAsync(newUser);

            return new OkObjectResult(newUser);
        }
    }
}
