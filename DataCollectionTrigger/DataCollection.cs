using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using DataAccess.GamesRepository;
using NhlDataCollection.DataGetter;

namespace DataCollectionTrigger
{
    public class DataCollection
    {
        [FunctionName("DataCollectionTrigger")]
        public async Task Run([TimerTrigger("0 0 0 * * *")]TimerInfo myTimer, ILogger logger)
        {
            // Run Data Collection
            logger.LogInformation("Starting Data Collection");
            string connectionString = System.Environment.GetEnvironmentVariable("GamesDatabase", EnvironmentVariableTarget.Process);

            var gameParser = new GameParser();
            var requestMaker = new RequestMaker();
            var dataAccess = new GamesDA(connectionString);
            var endYear = GetEndSeason(DateTime.UtcNow);
            var dataGetter = new DataGetter(gameParser, requestMaker, dataAccess, endYear, logger);
            await dataGetter.GetData();
            logger.LogInformation("Completed Data Collection");

            // Run Data Cleaning
            logger.LogInformation("Starting Data Cleaning");

            logger.LogInformation("Completed Data Cleaning");
        }

        // Season spans 2 years (2021-2022) but we only want the start year of the season
        // (ex. February 2022 we want 2021 to be the end season)
        public int GetEndSeason(DateTime currentDate)
        {
            var endSeasonDate = new DateTime(currentDate.Year, 09, 15);
            int currentSeasonYear;

            if (currentDate > endSeasonDate)
                currentSeasonYear = currentDate.Year;
            else
                currentSeasonYear = currentDate.Year - 1;

            return currentSeasonYear;
        }
    }
}