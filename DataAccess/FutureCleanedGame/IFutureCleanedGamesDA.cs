using Entities.Models;

namespace DataAccess.FutureCleanedGame
{
    public interface IFutureCleanedGamesDA
    {
        void AddFutureGames(List<CleanedGame> games);
        bool GetIfFutureGameExistsByIdFromCache(int id);
        public void CacheFutureGameIds();
    }
}
