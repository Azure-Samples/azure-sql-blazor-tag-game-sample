using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Tag.Sample
{
    public static class GetUserInfo
    {
        // imperative input binding
        [FunctionName("GetUserInfo")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "userinfo")] HttpRequest req,
            IBinder binder,
            ILogger log)
        {
            // get UserId and Passkey from header
            Guid userId = Guid.Parse(req.Headers["x-tag-userid"]);
            Guid passkey = Guid.Parse(req.Headers["x-tag-passkey"]);

            try
            {
                var sqlAttribute = new SqlAttribute(connectionStringSetting: "SqlConnectionString",
                    commandText: "[Tag].[GetUserInfo]",
                    commandType: System.Data.CommandType.StoredProcedure,
                    parameters: $"@UserIdInput={userId},@PasskeyInput={passkey}"
                );

                // get user info from database
                var userInfoList = await binder.BindAsync<IEnumerable<UserInfo>>(sqlAttribute);
                UserInfo userInfo = userInfoList.FirstOrDefault();

                return new OkObjectResult(userInfo);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
