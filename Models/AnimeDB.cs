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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Anime>().HasKey(a => a.ID);
            modelBuilder.Entity<Anime>().Property(a => a.ID).ValueGeneratedOnAdd();
        }
    }
}
