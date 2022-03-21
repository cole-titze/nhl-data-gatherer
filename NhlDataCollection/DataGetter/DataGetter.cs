using Entities.Models;
using NhlDataCollection.DataAccess;

namespace NhlDataCollection.DataGetter
{
    public class DataGetter
	{
        private IGameParser GameParser;
        private IRequestMaker RequestMaker;
        private IGamesDA gamesDataAccess;
        private const int _maxGameId = 1400;
        private readonly int startYear = 2012;
        private readonly int endYear;
        private static Game _emptyGame = new Game();

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
                Game game = _emptyGame;
                var gameList = new List<Game>();
                for (int id = 0; id < _maxGameId; id++)
                {
                    var recordExists = CheckIfRecordExistsInDb(year, id);
                    if(recordExists)
                        continue;

                    var query = RequestMaker.CreateRequestQuery(year, id);
                    var response = await RequestMaker.MakeRequest(query);

                    if (response.IsSuccessStatusCode)
                    {
                        game = await GameParser.BuildGame(response);
                        if (game.IsValid())
                            gameList.Add(game);
                    }
                }
                // Add a years worth of games to db
                gamesDataAccess.AddGames(gameList);
            }
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