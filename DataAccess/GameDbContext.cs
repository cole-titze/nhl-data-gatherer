using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public partial class GameDbContext : DbContext
    {
        private readonly string _connectionString;
        public GameDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<FutureGame> FutureGames { get; set; }
        public virtual DbSet<CleanedGame> CleanedGames { get; set; }
        public virtual DbSet<FutureCleanedGame> FutureCleanedGames { get; set; }
        public virtual DbSet<PredictedGame> PredictedGames { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}