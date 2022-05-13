using Entities.Models;

namespace NhlDataCollection.GameCollection
{
    public interface IGameParser
    {
        Task<Game> BuildGame(HttpResponseMessage response);
    }
}