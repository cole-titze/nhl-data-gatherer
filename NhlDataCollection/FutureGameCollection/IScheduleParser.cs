using Entities.Models;

namespace NhlDataCollection.FutureGameCollection
{
    public interface IScheduleParser
    {
        Task<List<Game>> BuildFutureGames(HttpResponseMessage response);
        Task<int> GetNumberOfGamesInSeason(HttpResponseMessage response);
    }
}