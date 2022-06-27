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
                    awayTeamId = futureGame.awayTeamId,
                    homeTeamId = futureGame.homeTeamId,
                    gameDate = futureGame.gameDate,
                };
                predictedGames.Add(game);
            }
            return predictedGames;
        }
    }
}

