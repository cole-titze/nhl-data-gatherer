using Entities;
using Entities.Models;
using DataAccess.GameRepository;
using DataAccess.PlayerRepository;
using Microsoft.Extensions.Logging;
using NhlDataCollection.FutureGameCollection;
using NhlDataCollection.GameCollection;

namespace NhlDataCollection
{
    public class DataGetter
	{
        private IGameParser _gameParser;
        private IScheduleParser _scheduleParser;
        private IGameRequestMaker _gameRequestMaker;
        private IScheduleRequestMaker _scheduleRequestMaker;
        private IGameRepository _gameRepo;
        private IPlayerRepository _playerRepo;
        private const int cutOffCount = 300;
        private const int _maxGameId = 1400;
        private readonly DateRange _yearRange;
        private readonly ILogger _logger;
        private readonly int _daysToAdd = 0;

        public DataGetter(IGameParser gameParser, IScheduleParser scheduleParser, IScheduleRequestMaker scheduleRequestMaker, IGameRequestMaker gameRequestMaker, IPlayerRepository playerRepo, IGameRepository gameRepo, DateRange yearRange, ILogger logger)
		{
            _gameParser = gameParser;
            _scheduleParser = scheduleParser;
            _gameRequestMaker = gameRequestMaker;
            _scheduleRequestMaker = scheduleRequestMaker;
            _yearRange = yearRange;
            _logger = logger;
            _playerRepo = playerRepo;
            _gameRepo = gameRepo;
		}
		public async Task GetData()
        {
            for (int year = _yearRange.StartYear; year <= _yearRange.EndYear; year++)
            {
                var gameCount = await _gameRepo.GetGameCountBySeason(year);
                // Skip if data is already in db and not the current year
                // If current year, data could be incomplete so run anyways
                if (gameCount > cutOffCount && year < _yearRange.EndYear)
                    continue;

                var gameList = await GetGamesForSeason(year);
                // Add a years worth of games to db
                await _gameRepo.AddGames(gameList);
            }
            var futureGames = await GetFutureGames();
            await _gameRepo.AddFutureGames(futureGames);
            var predictedGames = Mapper.MapFutureToPredictedGames(futureGames);
            await _gameRepo.AddPredictedGames(predictedGames);
            //var vegasOdds = await 
        }
        private async Task<List<FutureGame>> GetFutureGames()
        {
            List<FutureGame> gameList = new();
            var tomorrow = DateTime.Now.AddDays(_daysToAdd);
            var query = _scheduleRequestMaker.CreateRequestQuery(tomorrow);
            var response = await _scheduleRequestMaker.MakeRequest(query);

            if (response.IsSuccessStatusCode)
                gameList = await _scheduleParser.BuildFutureGames(response);

            return gameList;
        }

        private async Task<List<Game>> GetGamesForSeason(int season)
        {
            await _gameRepo.CacheSeasonOfGames(season);
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
            Game game = _gameRepo.GetCachedGameById(_gameRequestMaker.BuildId(year, id));

            if(game.id == -1)
                return false;
            return true;
        }
    }
}