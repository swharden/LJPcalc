using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Linq;

namespace LJPcalc.API
{
    public static class CalculateLJP
    {
        [FunctionName("CalculateLJP")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req,
            ILogger log)
        {
            string ip = (req.Headers["X-Forwarded-For"].FirstOrDefault() ?? "").Split(new char[] { ':' }).FirstOrDefault();
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var experiment = LJPmath.Experiment.FromJson(requestBody);
            var result = experiment.Execute();
            await LogCalculation(experiment.GetShortDescription(), result.V, result.benchmark_s, ip, log);
            string resultJson = result.ToJson(pretty: true);
            return new OkObjectResult(resultJson);
        }

        private async static Task LogCalculation(string description, double result, double executionTime, string ip, ILogger log)
        {
            string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage", EnvironmentVariableTarget.Process);
            if (connectionString is null)
                throw new InvalidOperationException("null connection string");
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            var client = cloudStorageAccount.CreateCloudTableClient();
            var table = client.GetTableReference("LJPcalcExecutions");
            await table.CreateIfNotExistsAsync();
            var entry = new CalcTableEntry(description, result, executionTime, ip);
            TableOperation operation = TableOperation.InsertOrMerge(entry);
            await table.ExecuteAsync(operation);
            log.LogInformation("Added log to table storage");
        }
    }
}
