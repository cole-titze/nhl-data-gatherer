using nhl_data_builder.Entities;
using System.Data;

namespace nhl_data_gatherer.DataAccess
{
    public interface IGamesDA
    {
        void AddGames(List<Game> games);
        Game GetGameById(int id);
        List<Game> GetGames();
        Game MapDataRowToGame(DataRow row);
    }
}