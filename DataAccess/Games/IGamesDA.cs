using Entities.Models;
using System.Data;

namespace DataAccess.GamesRepository
{
    public interface IGamesDA
    {
        void AddGames(List<Game> games);
        Game GetGameById(int id);
        List<Game> GetGames();
        Game MapDataRowToGame(DataRow row);
        int GetGameCountBySeason(int year);
        void CacheSeasonOfGames(int year);
        Game GetCachedGameById(int id);
        List<Game> GetCachedGames();
        List<FutureGame> GetFutureGames();
    }
}