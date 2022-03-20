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
        private const int _maxGameId = 1300;
        private readonly List<int> _years = new List<int>(){ 2012, 2013, 2014, 2015, 2016, 2017, 2018, 2019, 2020, 2021 };
        private static Game _emptyGame = new Game();

        public DataGetter(IGameParser gameParser, IRequestMaker requestMaker, IConfiguration config)
		{
            Config = config;
            GameParser = gameParser;
            RequestMaker = requestMaker;
		}
		public async Task GetData()
        {
            var gamesDA = new GamesDA(Config);            

            foreach (var year in _years)
            {
                Game game = _emptyGame;
                var gameList = new List<Game>();
                for (int id = 0; id < _maxGameId; id++)
                {
                    //var recordExists = CheckIfRecordExists();

                    var query = RequestMaker.CreateRequestQuery(year, id);
                    var response = await RequestMaker.MakeRequest(query);

                    if (response.IsSuccessStatusCode)
                    {
                        game = await GameParser.BuildGame(response);
                        if(game.homeTeamName != string.Empty)
                            gameList.Add(game);
                    }
                }
                // Add a years worth of games to db
                gamesDA.AddGames(gameList);
            }
        }
	}
}