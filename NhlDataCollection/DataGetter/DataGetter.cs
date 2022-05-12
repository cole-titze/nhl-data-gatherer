using Entities;
using Entities.Models;
using DataAccess.GamesRepository;
using Microsoft.Extensions.Logging;

namespace NhlDataCollection.DataGetter
{
    public class DataGetter
	{
        private IGameParser GameParser;
        private IScheduleParser ScheduleParser;
        private IGameRequestMaker GameRequestMaker;
        private IScheduleRequestMaker ScheduleRequestMaker;
        private IGamesDA gamesDataAccess;
        private const int cutOffCount = 300;
        private const int _maxGameId = 1400;
        private readonly DateRange _yearRange;
        private readonly ILogger _logger;
        private readonly int _daysToAdd = 0;

        public DataGetter(IGameParser gameParser, IScheduleParser scheduleParser, IScheduleRequestMaker scheduleRequestMaker, IGameRequestMaker gameRequestMaker, IGamesDA gamesDA, DateRange yearRange, ILogger logger)
		{
            GameParser = gameParser;
            ScheduleParser = scheduleParser;
            GameRequestMaker = gameRequestMaker;
            ScheduleRequestMaker = scheduleRequestMaker;
            gamesDataAccess = gamesDA;
            _yearRange = yearRange;
            _logger = logger;
		}
		public async Task GetData()
        {
            for (int year = _yearRange.StartYear; year <= _yearRange.EndYear; year++)
            {
                _logger.LogInformation("Getting Year: " + year.ToString());
                var gameCount = gamesDataAccess.GetGameCountBySeason(year);
                // Skip if data is already in db and not the current year
                // If current year, data could be incomplete so run anyways
                if (gameCount > cutOffCount && year < _yearRange.EndYear)
                    continue;

                gamesDataAccess.CacheSeasonOfGames(year);
                var gameList = await GetGamesForSeason(year);
                // Add a years worth of games to db
                gamesDataAccess.AddGames(gameList);
            }
            var futureGames = await GetFutureGames();
            gamesDataAccess.AddFutureGames(futureGames);
        }
        private async Task<List<FutureGame>> GetFutureGames()
        {
            List<FutureGame> gameList = new List<FutureGame>();
            var tomorrow = DateTime.Now.AddDays(_daysToAdd);
            var query = ScheduleRequestMaker.CreateRequestQuery(tomorrow);
            var response = await ScheduleRequestMaker.MakeRequest(query);

            if (response.IsSuccessStatusCode)
                gameList = await ScheduleParser.BuildFutureGames(response);

            return gameList;
        }

        private async Task<List<Game>> GetGamesForSeason(int season)
        {
            var gameList = new List<Game>();

            for (int id = 0; id < _maxGameId; id++)
            {
                var recordExists = CheckIfRecordExistsInDb(season, id);
                if (recordExists)
                    continue;

                var query = GameRequestMaker.CreateRequestQuery(season, id);
                var response = await GameRequestMaker.MakeRequest(query);

                if (response.IsSuccessStatusCode)
                {
                    var game = await GameParser.BuildGame(response);
                    if (game.IsValid())
                        gameList.Add(game);
                }
            }
            return gameList;
        }

        private bool CheckIfRecordExistsInDb(int year, int id)
        {
            Game game = gamesDataAccess.GetCachedGameById(GameRequestMaker.BuildId(year, id));

            if(game.id == -1)
                return false;
            return true;
        }
    }
}