using Entities.Models;
using Newtonsoft.Json;

namespace NhlDataCollection.FutureGameCollection
{
    public class ScheduleParser : IScheduleParser
    {
        public async Task<List<FutureGame>> BuildFutureGames(HttpResponseMessage response)
        {
            // Get data as Json string 
            string data = await response.Content.ReadAsStringAsync();
            // Add Json string conversion to hard object
            var message = JsonConvert.DeserializeObject<dynamic>(data);
            if (InvalidGame(message))
                return new List<FutureGame>();

            List<FutureGame> games = ParseMessageToGames(message);
            return games;
        }
        private List<FutureGame> ParseMessageToGames(dynamic message)
        {
            var gameList = new List<FutureGame>();
            foreach (var date in message.dates)
            {
                foreach (var game in date.games)
                {
                    var futureGame = new FutureGame()
                    {
                        id = (int)game.gamePk,
                        homeTeamName = (string)game.teams.home.team.name,
                        awayTeamName = (string)game.teams.away.team.name,
                        gameDate = DateTime.Parse((string)game.gameDate),
                    };
                    gameList.Add(futureGame);
                }
            }

            return gameList;
        }
        private bool InvalidGame(dynamic message)
        {
            if (message == null)
                return true;
            return (int)message.totalItems == 0;
        }
    }
}
