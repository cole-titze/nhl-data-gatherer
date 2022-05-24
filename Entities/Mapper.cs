using Entities.Models;

namespace Entities
{
    public static class Mapper
    {
        public static List<PredictedGame> MapFutureToPredictedGames(List<FutureGame> futureGames)
        {
            var predictedGames = new List<PredictedGame>();
            foreach(var futureGame in futureGames)
            {
                var game = new PredictedGame()
                {
                    id = futureGame.id,
                    awayTeamName = futureGame.awayTeamName,
                    homeTeamName = futureGame.homeTeamName,
                    gameDate = futureGame.gameDate,
                };
                predictedGames.Add(game);
            }
            return predictedGames;
        }
    }
}

