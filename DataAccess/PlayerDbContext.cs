using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public partial class PlayerDbContext : DbContext
    {
        private readonly string _connectionString;
        public PlayerDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        public virtual DbSet<PlayerValue> PlayerValue { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayerValue>()
                .HasKey(c => new { c.id, c.startYear });
        }
    }
}
