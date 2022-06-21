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
            foreach(var futureGame in futureGames)
            {
                var game = _dbContext.FutureGame.FirstOrDefault(i => i.id == futureGame.id);
                if (game == null)
                    await _dbContext.FutureGame.AddAsync(futureGame);
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddGames(List<Game> games)
        {
            await _dbContext.Game.AddRangeAsync(games);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddPredictedGames(List<PredictedGame> predictedGames)
        {
            foreach(var predictedGame in predictedGames)
            {
                var game = _dbContext.PredictedGame.FirstOrDefault(i => i.id == predictedGame.id);
                if(game == null)
                    await _dbContext.PredictedGame.AddAsync(predictedGame);
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task CacheSeasonOfGames(int season)
        {
            _cachedGames = await _dbContext.Game.Where(s => s.seasonStartYear == season).ToListAsync();
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
            return await _dbContext.Game.Where(s => s.seasonStartYear == year)
                                .CountAsync();
        }

        public async Task<int> GetCleanedGameCountBySeason(int year)
        {
            return await _dbContext.CleanedGame.Where(s => s.seasonStartYear == year)
                                .CountAsync();
        }

        public List<Game> GetCachedGames()
        {
            return _cachedGames;
        }

        public async Task AddCleanedGames(List<CleanedGame> games)
        {
            await _dbContext.CleanedGame.AddRangeAsync(games);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<FutureGame>> GetFutureGames()
        {
            return await _dbContext.FutureGame.ToListAsync();
        }

        public async Task AddFutureCleanedGames(List<FutureCleanedGame> futureCleanedGames)
        {
            foreach (var futureCleanedGame in futureCleanedGames)
            {
                var game = _dbContext.FutureCleanedGame.FirstOrDefault(i => i.id == futureCleanedGame.id);
                if (game == null)
                    await _dbContext.FutureCleanedGame.AddAsync(futureCleanedGame);
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> GetIfCleanedGameExistsById(int id)
        {
            var game = await _dbContext.CleanedGame.FirstOrDefaultAsync(i => i.id == id);
            if (game == null)
                return false;
            return true;
        }

        public async Task<bool> GetIfFutureCleanedGameExistsById(int id)
        {
            var game = await _dbContext.FutureCleanedGame.FirstOrDefaultAsync(i => i.id == id);
            if (game == null)
                return false;
            return true;
        }
    }
}

