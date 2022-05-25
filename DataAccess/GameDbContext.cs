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
        public virtual DbSet<Game> Game { get; set; }
        public virtual DbSet<FutureGame> FutureGame { get; set; }
        public virtual DbSet<CleanedGame> CleanedGame { get; set; }
        public virtual DbSet<FutureCleanedGame> FutureCleanedGame { get; set; }
        public virtual DbSet<PredictedGame> PredictedGame { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}