using DataAccess;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.PlayerRepository
{
    public class PlayerValueRepository : IPlayerRepository
    {
        private List<PlayerValue> _cachedPlayerValues = new List<PlayerValue>();
        private readonly PlayerDbContext _dbContext;
        public PlayerValueRepository(PlayerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CachePlayerValues()
        {
            _cachedPlayerValues = await _dbContext.PlayerValue.ToListAsync();
        }
        public double GetPlayerValueByIdFromCache(int id)
        {
            var player = _cachedPlayerValues.FirstOrDefault(p => p.id == id);
            if (player == null)
                return 0;
            return player.value;
        }
        public string GetPositionByIdFromCache(int id)
        {
            var player = _cachedPlayerValues.FirstOrDefault(p => p.id == id);
            if (player == null)
                return "L";
            return player.position;
        }
        public async Task<double> GetPlayerValueById(int id)
        {
            var player = await _dbContext.PlayerValue.FirstOrDefaultAsync(p => p.id == id);
            if (player == null)
                return 0;
            return player.value;
        }
        public async Task<string> GetPositionById(int id)
        {
            var player = await _dbContext.PlayerValue.FirstOrDefaultAsync(p => p.id == id);
            if (player == null)
                return "L";
            return player.position;
        }
    }
}
