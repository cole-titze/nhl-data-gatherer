using Entities.Models;

namespace NhlDataCleaning.Mappers
{
    public static class GameMapper
    {
        public static Game FutureGameToGame(FutureGame futureGame)
        {
            return new Game
            {
                id = futureGame.id,
                homeTeamName = futureGame.homeTeamName,
                awayTeamName = futureGame.awayTeamName,
                gameDate = futureGame.gameDate,
            };
        }
    }
}
