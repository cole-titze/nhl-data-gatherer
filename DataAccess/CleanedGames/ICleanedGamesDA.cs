using Entities.Models;
namespace DataAccess.CleanedGamesRepository
{
    public interface ICleanedGamesDA
    {
        void AddGames(List<CleanedGame> games);
        void CacheGameIds();
        bool GetIfGameExistsByIdFromCache(int id);
    }
}
