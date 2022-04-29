using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using DataAccess.GamesRepository;
using DataAccess.CleanedGamesRepository;
using NhlDataCollection.DataGetter;
using Entities;
using NhlDataCleaning;

namespace DataCollectionTrigger
{
    public class DataCollection
    {
        [FunctionName("DataCollectionTrigger")]
        public async Task Run([TimerTrigger("0 0 5 * * *")]TimerInfo myTimer, ILogger logger)
        {
            string connectionString = System.Environment.GetEnvironmentVariable("GamesDatabase", EnvironmentVariableTarget.Process);
            await Main(logger, connectionString);
        }
        public async Task Main(ILogger logger, string connectionString)
        {
            // Run Data Collection
            logger.LogInformation("Starting Data Collection");

            var gameParser = new GameParser();
            var requestMaker = new RequestMaker();
            var dataAccess = new GamesDA(connectionString);
            var cleanDataAccess = new CleanedGamesDA(connectionString);
            var dateRange = new DateRange()
            {
                StartYear = 2012,
                EndYear = GetEndSeason(DateTime.UtcNow),
            };
            var dataGetter = new DataGetter(gameParser, requestMaker, dataAccess, dateRange, logger);
            await dataGetter.GetData();
            logger.LogInformation("Completed Data Collection");

            // Run Data Cleaning
            logger.LogInformation("Starting Data Cleaning");
            var dataCleaner = new DataCleaner(logger, dataAccess, cleanDataAccess, dateRange);
            dataCleaner.CleanData();
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