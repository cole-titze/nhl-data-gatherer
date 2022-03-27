using Entities.Models;
using NhlDataCollection.DataAccess;

namespace NhlDataCollection.DataGetter
{
    public class DataGetter
	{
        private IGameParser GameParser;
        private IRequestMaker RequestMaker;
        private IGamesDA gamesDataAccess;
        private const int cutOffCount = 300;
        private const int _maxGameId = 1400;
        private readonly int startYear = 2012;
        private readonly int endYear;

        public DataGetter(IGameParser gameParser, IRequestMaker requestMaker, IGamesDA gamesDA, int endingYear)
		{
            GameParser = gameParser;
            RequestMaker = requestMaker;
            gamesDataAccess = gamesDA;
            endYear = endingYear;
		}
		public async Task GetData()
        {
            for (int year = startYear; year <= endYear; year++)
            {
                var gameCount = GetStoredGameCountForSeason(year);
                // Skip if data is already in db and not the current year
                // If current year data could be incomplete so run anyways
                if (gameCount > cutOffCount && year < endYear)
                    continue;

                var gameList = await GetGamesForSeason(year);
                // Add a years worth of games to db
                gamesDataAccess.AddGames(gameList);
            }
        }

        private int GetStoredGameCountForSeason(int year)
        {
            return gamesDataAccess.GetGameCountBySeason(year);
        }

        private async Task<List<Game>> GetGamesForSeason(int season)
        {
            var gameList = new List<Game>();

            for (int id = 0; id < _maxGameId; id++)
            {
                var recordExists = CheckIfRecordExistsInDb(season, id);
                if (recordExists)
                    continue;

                var query = RequestMaker.CreateRequestQuery(season, id);
                var response = await RequestMaker.MakeRequest(query);

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
            Game game = gamesDataAccess.GetGameById(RequestMaker.BuildId(year, id));

            if(game.id == -1)
                return false;
            return true;
        }
    }
}