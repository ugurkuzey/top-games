using Microsoft.EntityFrameworkCore;
using TopGames.Core.Models;

namespace TopGames.EntityFramework
{
    public class TopGamesDbContext : DbContext
    {
        public TopGamesDbContext(DbContextOptions<TopGamesDbContext> options) : base(options)
        {
        }

        public DbSet<Game> Games { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>().HasKey(x => x.Id);
            modelBuilder.Entity<Game>().Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}
