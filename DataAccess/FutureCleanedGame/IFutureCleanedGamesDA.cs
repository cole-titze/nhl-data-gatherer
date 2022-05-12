using Entities.Models;

namespace DataAccess.FutureCleanedGame
{
    public interface IFutureCleanedGamesDA
    {
        void AddFutureGames(List<CleanedGame> games);
    }
}
