using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using NhlDataCollection.GameCollection;
using Entities;
using NhlDataCleaning;
using NhlDataCollection.FutureGameCollection;
using NhlDataCollection;
using DataAccess.GameRepository;
using DataAccess.PlayerRepository;
using DataAccess;
using NhlDataCleaning.RequestMaker;

namespace DataCollectionTrigger
{
    public class DataCollection
    {
        [FunctionName("DataCollectionTrigger")]
        public async Task Run([TimerTrigger("0 0 5 * * *")]TimerInfo myTimer, ILogger logger)
        {
            string gamesConnectionString = System.Environment.GetEnvironmentVariable("GamesDatabase", EnvironmentVariableTarget.Process);
            string playersConnectionString = System.Environment.GetEnvironmentVariable("PlayersDatabase", EnvironmentVariableTarget.Process);
            await Main(logger, gamesConnectionString, playersConnectionString);
        }
        public async Task Main(ILogger logger, string gamesConnectionString, string playersConnectionString)
        {
            // Run Data Collection
            logger.LogInformation("Starting Data Collection");

            var gameDbContext = new GameDbContext(gamesConnectionString);
            var playerDbContext = new PlayerDbContext(playersConnectionString);
            var gameParser = new GameParser();
            var scheduleParser = new ScheduleParser();
            var gameRequestMaker = new GameRequestMaker();
            var scheduleRequestMaker = new ScheduleRequestMaker();
            var gameRepo = new GameRepository(gameDbContext);
            var playerRepo = new PlayerValueRepository(playerDbContext);
            var rosterRequestMaker = new RosterRequestMaker();
            var dateRange = new DateRange()
            {
                StartYear = 2012,
                EndYear = GetEndSeason(DateTime.UtcNow),
            };
            var dataGetter = new DataGetter(gameParser, scheduleParser, scheduleRequestMaker, gameRequestMaker, playerRepo, gameRepo, dateRange, logger);
            await dataGetter.GetData();
            logger.LogInformation("Completed Data Collection");

            // Run Data Cleaning
            logger.LogInformation("Starting Data Cleaning");
            var dataCleaner = new DataCleaner(logger, playerRepo, gameRepo, rosterRequestMaker, dateRange);
            await dataCleaner.CleanData();
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