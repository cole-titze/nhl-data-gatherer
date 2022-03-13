using Newtonsoft.Json;
using nhl_data_builder.Entities;

namespace nhl_data_builder.DataGetter
{
    public class GameParser : IGameParser
	{
        public async Task<Game> BuildGame(HttpResponseMessage response)
        {
            // Get data as Json string 
            string data = await response.Content.ReadAsStringAsync();
            // Add Json string conversion to hard object
            var message = JsonConvert.DeserializeObject<dynamic>(data);
            if (InvalidGame(message))
                return new Game();
            Console.WriteLine(message);

            var game = ParseMessageToGame(message);
            return game;
        }

        private Game ParseMessageToGame(dynamic message)
        {
            return new Game();
        }

        private bool InvalidGame(dynamic message)
        {
            if(message == null)
                return true;

            float homeFaceoffs = (float)message.liveData.boxscore.teams.home.teamStats.teamSkaterStats.faceOffWinPercentage;
            float awayFaceoffs = (float)message.liveData.boxscore.teams.away.teamStats.teamSkaterStats.faceOffWinPercentage;
            return (homeFaceoffs == 0 && awayFaceoffs == 0);
        }
	}
}