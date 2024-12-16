using GameStore.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Data
{
    public class GameStoreContext : DbContext
    {
        public GameStoreContext(DbContextOptions<GameStoreContext> options) : base(options) { }

        public DbSet<Game> Games { get; set; }
        public DbSet<Genre> Genres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Genre>().HasData(
                new { Id = 1, Name = "Fighting" },
                new { Id = 2, Name = "Sports" },
                new { Id = 3, Name = "Racing" },
                new { Id = 4, Name = "MMORPG" },
                new { Id = 5, Name = "MOBO" }
            );
        }
    }
}