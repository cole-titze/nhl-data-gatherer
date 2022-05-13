using Entities.Models;

namespace NhlDataCollection.FutureGameCollection
{
    public interface IScheduleParser
    {
        Task<List<FutureGame>> BuildFutureGames(HttpResponseMessage response);
    }
}