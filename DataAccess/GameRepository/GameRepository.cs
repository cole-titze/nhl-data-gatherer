using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.GameRepository
{
	public class GameRepository : IGameRepository
	{
        private List<Game> _cachedSeasonsGames = new List<Game>();
        private List<Game> _cachedLastSeasonsGames = new List<Game>();
        private readonly GameDbContext _dbContext;
        public GameRepository(GameDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddGames(List<Game> games)
        {
            await _dbContext.Game.AddRangeAsync(games);
            await _dbContext.SaveChangesAsync();
        }
        public async Task AddUpdateGames(List<Game> games)
        {
            var addList = new List<Game>();
            var updateList = new List<Game>();
            foreach(var game in games)
            {
                var dbGame = _dbContext.Game.FirstOrDefault(i => i.id == game.id);
                if (dbGame == null)
                    addList.Add(game);
                else
                {
                    dbGame.Clone(game);
                    updateList.Add(dbGame);
                }
            }
            await _dbContext.Game.AddRangeAsync(addList);
            _dbContext.Game.UpdateRange(updateList);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddPredictedGames(List<PredictedGame> predictedGames)
        {
            foreach(var predictedGame in predictedGames)
            {
                var game = _dbContext.PredictedGame.FirstOrDefault(i => i.id == predictedGame.id);
                if (game == null)
                    await _dbContext.PredictedGame.AddAsync(predictedGame);
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task CacheSeasonOfGames(int season)
        {
            _cachedSeasonsGames = await _dbContext.Game.Where(s => s.seasonStartYear == season).ToListAsync();
        }
        public async Task CacheLastSeasonOfGames(int season)
        {
            _cachedLastSeasonsGames = await _dbContext.Game.Where(s => s.seasonStartYear == season).ToListAsync();
        }

        public Game GetCachedGameById(int id)
        {
            var game = _cachedSeasonsGames.FirstOrDefault(i => i.id == id);
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

        public List<Game> GetCachedSeasonsGames()
        {
            return _cachedSeasonsGames;
        }
        public List<Game> GetCachedLastSeasonsGames()
        {
            return _cachedLastSeasonsGames;
        }

        public async Task AddCleanedGames(List<CleanedGame> games)
        {
            await _dbContext.CleanedGame.AddRangeAsync(games);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> GetIfCleanedGameExistsById(int id)
        {
            var game = await _dbContext.CleanedGame.FirstOrDefaultAsync(i => i.id == id);
            if (game == null)
                return false;
            return true;
        }
    }
}

