using Entities.Models;
using System.Data;

namespace NhlDataCollection.DataAccess
{
    public interface IGamesDA
    {
        void AddGames(List<Game> games);
        Game GetGameById(int id);
        List<Game> GetGames();
        Game MapDataRowToGame(DataRow row);
    }
}