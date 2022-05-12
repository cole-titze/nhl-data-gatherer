using Entities.Models;

namespace DataAccess.FutureGames
{
    public interface IFutureGamesDA
    {
        void AddFutureGames(List<FutureGame> games);
        List<FutureGame> GetFutureGames();
    }
}
