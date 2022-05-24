using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.GameRepository
{
	public class GameRepository : IGameRepository
	{
        private List<Game> _cachedGames = new List<Game>();
        private readonly GameDbContext _dbContext;
        public GameRepository(GameDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddFutureGames(List<FutureGame> futureGames)
        {
            await _dbContext.FutureGames.AddRangeAsync(futureGames);
        }

        public async Task AddGames(List<Game> games)
        {
            await _dbContext.Games.AddRangeAsync(games);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddPredictedGames(List<PredictedGame> predictedGames)
        {
            await _dbContext.PredictedGames.AddRangeAsync(predictedGames);
            await _dbContext.SaveChangesAsync();
        }

        public async Task CacheSeasonOfGames(int season)
        {
            _cachedGames = await _dbContext.Games.Where(s => s.seasonStartYear == season).ToListAsync();
        }

        public Game GetCachedGameById(int id)
        {
            var game = _cachedGames.FirstOrDefault(i => i.id == id);
            if (game == null)
                return new Game();
            return game;
        }

        public async Task<int> GetGameCountBySeason(int year)
        {
            return await _dbContext.Games.Where(s => s.seasonStartYear == year)
                                .CountAsync();
        }

        public List<Game> GetCachedGames()
        {
            return _cachedGames;
        }

        public async Task AddCleanedGames(List<CleanedGame> games)
        {
            await _dbContext.CleanedGames.AddRangeAsync(games);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<FutureGame>> GetFutureGames()
        {
            return await _dbContext.FutureGames.ToListAsync();
        }

        public async Task AddFutureCleanedGames(List<FutureCleanedGame> futureCleanedGames)
        {
            await _dbContext.FutureCleanedGames.AddRangeAsync(futureCleanedGames);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> GetIfGameExistsById(int id)
        {
            var game = await _dbContext.Games.FirstOrDefaultAsync(i => i.id == id);
            if (game == null)
                return false;
            return true;
        }

        public async Task<bool> GetIfFutureGameExistsById(int id)
        {
            var game = await _dbContext.FutureGames.FirstOrDefaultAsync(i => i.id == id);
            if (game == null)
                return false;
            return true;
        }
    }
}

