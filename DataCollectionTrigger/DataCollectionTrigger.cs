using System;
using NhlDataCollection.DataAccess;
using NhlDataCollection.DataGetter;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace DataCollectionTrigger
{
    public class DataCollectionTrigger
    {
        [FunctionName("DataCollectionTrigger")]
        public async Task Run([TimerTrigger("0 0 0 * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            // Setup Database Configuration
            var environment = Environment.GetEnvironmentVariable("ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true)
                .AddJsonFile($"appsettings.{environment}.json", true, true);
            IConfiguration config = builder.Build();

            // Run Application
            var gameParser = new GameParser();
            var requestMaker = new RequestMaker();
            var dataAccess = new GamesDA(config);
            var endYear = DateTime.Now.Year;
            var dataGetter = new DataGetter(gameParser, requestMaker, dataAccess, endYear);
            await dataGetter.GetData();
        }
    }
}

