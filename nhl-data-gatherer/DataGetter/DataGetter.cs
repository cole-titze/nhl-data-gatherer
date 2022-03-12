using nhl_data_builder.Entities;

namespace nhl_data_builder.DataGetter
{
    public class DataGetter
	{
        private IGameParser GameParser;
        private IRequestMaker RequestMaker;
        private const int _maxGameId = 9;
        private readonly List<int> _years = new List<int>(){ 2019 };
        private static Game _emptyGame = new Game();

        public DataGetter(IGameParser gameParser, IRequestMaker requestMaker)
		{
            GameParser = gameParser;
            RequestMaker = requestMaker;
		}
		public async Task GetData()
        {
            foreach (var year in _years)
            {
                Game game = _emptyGame;
                var gameList = new List<Game>();
                for (int i = 0; i < _maxGameId; i++)
                {
                    var query = RequestMaker.CreateRequestQuery(year, i);
                    var response = await RequestMaker.MakeRequest(query);

                    if (response.IsSuccessStatusCode)
                        game = await GameParser.BuildGame(response);
                    if(game.homeTeam != string.Empty)
                        gameList.Add(game);
                }
                // Store next year into database
            }
        }
	}
}