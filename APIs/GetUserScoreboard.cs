using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Tag.Sample
{
    public static class GetUserScoreboard
    {
        [FunctionName("GetUserScoreboard")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "userscoreboard")] HttpRequest req,
            [Sql(commandText: "SELECT [Red],[Orange],[Yellow],[Green],[Blue],[Purple],[Rainbow] FROM [Tag].[Scoreboard] WHERE [UserId] = @UserId",
                connectionStringSetting:"SqlConnectionString", parameters:"@UserId={Query.userid}")] IEnumerable<UserScores> scores,
            ILogger log)
        {
            return new OkObjectResult(scores.FirstOrDefault());
        }
    }
}
