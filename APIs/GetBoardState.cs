using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Tag.Sample
{
    public static class GetBoardState
    {
        [FunctionName("GetBoardState")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "boardstate")] HttpRequest req,
            [Sql(commandText:"[Tag].[GetBoardState]", commandType: System.Data.CommandType.StoredProcedure, connectionStringSetting: "SqlConnectionString", parameters: "@XLocation={Query.X},@YLocation={Query.Y}")] IEnumerable<BoardState> boardState,
            ILogger log)
        {
            return new OkObjectResult(boardState);
        }
    }
}
