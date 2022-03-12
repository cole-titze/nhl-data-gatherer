using nhl_data_builder.Entities;
namespace nhl_data_builder.DataGetter
{
    public interface IGameParser
    {
        Task<Game> BuildGame(HttpResponseMessage response);
    }
}