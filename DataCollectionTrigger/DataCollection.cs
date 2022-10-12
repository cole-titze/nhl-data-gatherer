using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
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
        public async Task Main(string gamesConnectionString, string playersConnectionString)
        {
            // Run Data Collection
            Console.WriteLine("Starting Data Collection");

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
                StartYear = 2010,
                EndYear = GetEndSeason(DateTime.UtcNow),
            };
            var dataGetter = new DataGetter(gameParser, scheduleParser, scheduleRequestMaker, gameRequestMaker, playerRepo, gameRepo, dateRange);
            await dataGetter.GetData();
            Console.WriteLine("Completed Data Collection");

            // Run Data Cleaning
            Console.WriteLine("Starting Data Cleaning");
            var dataCleaner = new DataCleaner(playerRepo, gameRepo, rosterRequestMaker, dateRange);
            await dataCleaner.CleanData();
            Console.WriteLine("Completed Data Cleaning");
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