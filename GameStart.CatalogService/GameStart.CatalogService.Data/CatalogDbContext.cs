using GameStart.CatalogService.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStart.CatalogService.Data
{
    public class CatalogDbContext : DbContext
    {
        public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
        {
        }

        public DbSet<VideoGame> VideoGames { get; set; }

        public DbSet<Ganre> Ganres { get; set; }

        public DbSet<Developer> Developers { get; set; }

        public DbSet<Publisher> Publishers { get; set; }

        public DbSet<Platform> Platforms { get; set; }

        public DbSet<SystemRequirements> SystemRequirements { get; set; }

        public DbSet<LanguageAvailability> LanguageAvailabilities { get; set; }

        public DbSet<Language> Languages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);
        }
    }
}
