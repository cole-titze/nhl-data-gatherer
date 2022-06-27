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
                homeTeamId = futureGame.homeTeamId,
                awayTeamId = futureGame.awayTeamId,
                gameDate = futureGame.gameDate,
            };
        }
    }
}
