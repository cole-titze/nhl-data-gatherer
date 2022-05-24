using Entities.Models;

namespace DataAccess.GameRepository
{
	public interface IGameRepository
	{
		Task AddGames(List<Game> games);
        Task<int> GetGameCountBySeason(int year);
        Task AddFutureGames(List<FutureGame> futureGames);
        Task AddPredictedGames(List<PredictedGame> futureGames);
        Task CacheSeasonOfGames(int season);
        Game GetCachedGameById(int v);
        List<Game> GetCachedGames();
        Task AddCleanedGames(List<CleanedGame> games);
        Task<List<FutureGame>> GetFutureGames();
        Task AddFutureCleanedGames(List<FutureCleanedGame> futureCleanedGames);
        Task<bool> GetIfGameExistsById(int id);
        Task<bool> GetIfFutureGameExistsById(int id);
    }
}

