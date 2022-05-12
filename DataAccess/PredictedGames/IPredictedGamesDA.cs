using Entities.Models;

namespace DataAccess.PredictedGames
{
    public interface IPredictedGamesDA
    {
        public void AddPredictedGames(List<FutureGame> games);
    }
}
