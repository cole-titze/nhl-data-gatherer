using System;
using NhlDataCollection.DataAccess;
using NhlDataCollection.DataGetter;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DataCollectionTrigger
{
    public class DataCollectionTrigger
    {
        [FunctionName("DataCollectionTrigger")]
        public async Task Run([TimerTrigger("0 5 19 * * *")]TimerInfo myTimer, ILogger log)
        {
            string connectionString = System.Environment.GetEnvironmentVariable("GamesDatabase", EnvironmentVariableTarget.Process);

            // Run Application
            var gameParser = new GameParser();
            var requestMaker = new RequestMaker();
            var dataAccess = new GamesDA(connectionString);
            var endYear = DateTime.Now.Year;
            var dataGetter = new DataGetter(gameParser, requestMaker, dataAccess, endYear);
            await dataGetter.GetData();
            Console.WriteLine("Completed!");
        }
    }
}