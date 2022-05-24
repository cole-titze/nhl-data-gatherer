using DataAccess;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.PlayerRepository
{
    public class PlayerValueRepository : IPlayerRepository
    {
        private readonly PlayerDbContext _dbContext;
        public PlayerValueRepository(PlayerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<double> GetPlayerValueById(int id)
        {
            var player = await _dbContext.PlayerValue.FirstOrDefaultAsync(p => p.id == id);
            if (player == null)
                return 0;
            return player.value;
        }
    }
}
