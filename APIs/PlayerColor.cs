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
    public static class PlayerColor
    {
        [FunctionName("PlayerColor")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "patch", Route = "playercolor")] HttpRequest req,
            IBinder binder,
            [Sql(commandText: "Tag.Users", connectionStringSetting: "SqlConnectionString")] IAsyncCollector<UserInfo> outputToTable,
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

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            UserInfo userdata = JsonConvert.DeserializeObject<UserInfo>(requestBody);
            
            await outputToTable.AddAsync(userdata);
            await outputToTable.FlushAsync();

            return new OkObjectResult(userdata);
        }
    }
}
