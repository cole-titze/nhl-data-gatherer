using Entities.Models;

namespace NhlDataCollection.DataGetter
{
    public interface IGameParser
    {
        Task<Game> BuildGame(HttpResponseMessage response);
    }
}