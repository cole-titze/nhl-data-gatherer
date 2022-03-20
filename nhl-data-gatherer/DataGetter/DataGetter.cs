using Microsoft.Extensions.Configuration;
using nhl_data_builder.Entities;
using nhl_data_gatherer.DataAccess;

namespace nhl_data_builder.DataGetter
{
    public class DataGetter
	{
        private IConfiguration Config;
        private IGameParser GameParser;
        private IRequestMaker RequestMaker;
        private GamesDA gamesDA;
        private const int _maxGameId = 9;
        private readonly List<int> _years = new List<int>(){ 2012, 2013, 2014, 2015, 2016, 2017, 2018, 2019, 2020, 2021 };
        private static Game _emptyGame = new Game();

        public DataGetter(IGameParser gameParser, IRequestMaker requestMaker, IConfiguration config)
		{
            Config = config;
            GameParser = gameParser;
            RequestMaker = requestMaker;
            gamesDA = new GamesDA(Config); // Should be abstracted to interface
		}
		public async Task GetData()
        {
            foreach (var year in _years)
            {
                Game game = _emptyGame;
                var gameList = new List<Game>();
                for (int id = 0; id < _maxGameId; id++)
                {
                    var recordExists = CheckIfRecordExists(year, id);
                    if(recordExists)
                        continue;

                    var query = RequestMaker.CreateRequestQuery(year, id);
                    var response = await RequestMaker.MakeRequest(query);

                    if (response.IsSuccessStatusCode)
                    {
                        game = await GameParser.BuildGame(response);
                        if(game.homeTeamName != string.Empty) // abstract
                            gameList.Add(game);
                    }
                }
                // Add a years worth of games to db
                gamesDA.AddGames(gameList);
            }
        }

        private bool CheckIfRecordExists(int year, int id)
        {
            Game game = gamesDA.GetGameById(RequestMaker.BuildId(year, id));

            if(game.id == -1)
                return false;
            return true;
        }
    }
}