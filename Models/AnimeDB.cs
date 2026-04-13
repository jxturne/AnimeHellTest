using Microsoft.EntityFrameworkCore;
namespace AnimeHellTest.Models

{
    public class AnimeDB: DbContext
    {
        public DbSet<Anime> Animes { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=anime.db");
        }
    }
}
