using Entities.Models;

namespace DataAccess.GameRepository
{
	public interface IGameRepository
	{
		Task AddGames(List<Game> games);
        Task AddUpdateGames(List<Game> games);
        Task<int> GetGameCountBySeason(int year);
        Task<int> GetCleanedGameCountBySeason(int year);
        Task AddPredictedGames(List<PredictedGame> futureGames);
        Task CacheSeasonOfGames(int season);
        Task CacheLastSeasonOfGames(int season);
        Game GetCachedGameById(int v);
        List<Game> GetCachedSeasonsGames();
        List<Game> GetCachedLastSeasonsGames();
        Task AddCleanedGames(List<CleanedGame> games);
        Task<bool> GetIfCleanedGameExistsById(int id);
    }
}

