using Entities;
using Entities.Models;
using DataAccess.GamesRepository;
using Microsoft.Extensions.Logging;
using DataAccess.FutureGames;
using DataAccess.PredictedGames;

namespace NhlDataCollection.DataGetter
{
    public class DataGetter
	{
        private IGameParser _gameParser;
        private IScheduleParser _scheduleParser;
        private IGameRequestMaker _gameRequestMaker;
        private IScheduleRequestMaker _scheduleRequestMaker;
        private IPredictedGamesDA _predictedGamesDA;
        private IGamesDA _gamesDA;
        private IFutureGamesDA _futureGamesDA;
        private const int cutOffCount = 300;
        private const int _maxGameId = 1400;
        private readonly DateRange _yearRange;
        private readonly ILogger _logger;
        private readonly int _daysToAdd = 0;

        public DataGetter(IGameParser gameParser, IScheduleParser scheduleParser, IScheduleRequestMaker scheduleRequestMaker, IGameRequestMaker gameRequestMaker, IGamesDA gamesDA, IFutureGamesDA futureGamesDA, IPredictedGamesDA predictedGamesDA, DateRange yearRange, ILogger logger)
		{
            _gameParser = gameParser;
            _scheduleParser = scheduleParser;
            _gameRequestMaker = gameRequestMaker;
            _scheduleRequestMaker = scheduleRequestMaker;
            _gamesDA = gamesDA;
            _yearRange = yearRange;
            _logger = logger;
            _futureGamesDA = futureGamesDA;
            _predictedGamesDA = predictedGamesDA;
		}
		public async Task GetData()
        {
            for (int year = _yearRange.StartYear; year <= _yearRange.EndYear; year++)
            {
                var gameCount = _gamesDA.GetGameCountBySeason(year);
                // Skip if data is already in db and not the current year
                // If current year, data could be incomplete so run anyways
                if (gameCount > cutOffCount && year < _yearRange.EndYear)
                    continue;

                var gameList = await GetGamesForSeason(year);
                // Add a years worth of games to db
                _gamesDA.AddGames(gameList);
            }
            var futureGames = await GetFutureGames();
            _futureGamesDA.AddFutureGames(futureGames);
            _predictedGamesDA.AddPredictedGames(futureGames);
        }
        private async Task<List<FutureGame>> GetFutureGames()
        {
            List<FutureGame> gameList = new List<FutureGame>();
            var tomorrow = DateTime.Now.AddDays(_daysToAdd);
            var query = _scheduleRequestMaker.CreateRequestQuery(tomorrow);
            var response = await _scheduleRequestMaker.MakeRequest(query);

            if (response.IsSuccessStatusCode)
                gameList = await _scheduleParser.BuildFutureGames(response);

            return gameList;
        }

        private async Task<List<Game>> GetGamesForSeason(int season)
        {
            _gamesDA.CacheSeasonOfGames(season);
            var gameList = new List<Game>();

            for (int id = 0; id < _maxGameId; id++)
            {
                var recordExists = CheckIfRecordExistsInDb(season, id);
                if (recordExists)
                    continue;

                var query = _gameRequestMaker.CreateRequestQuery(season, id);
                var response = await _gameRequestMaker.MakeRequest(query);

                if (response.IsSuccessStatusCode)
                {
                    var game = await _gameParser.BuildGame(response);
                    if (game.IsValid())
                        gameList.Add(game);
                }
            }
            return gameList;
        }

        private bool CheckIfRecordExistsInDb(int year, int id)
        {
            Game game = _gamesDA.GetCachedGameById(_gameRequestMaker.BuildId(year, id));

            if(game.id == -1)
                return false;
            return true;
        }
    }
}