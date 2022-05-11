using Entities.Models;

namespace NhlDataCollection.DataGetter
{
    public interface IScheduleParser
    {
        Task<List<FutureGame>> BuildFutureGames(HttpResponseMessage response);
    }
}