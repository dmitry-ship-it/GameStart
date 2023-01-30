using GameStart.CatalogService.Data.Models;

namespace GameStart.CatalogService.Data.Repositories
{
    public class CatalogRepositoryWrapper : IRepositoryWrapper
    {
        public CatalogRepositoryWrapper(CatalogDbContext catalogDbContext)
        {
            VideoGames = new VideoGameRepository(catalogDbContext);
            SystemRequirements = new SystemRequirementsRepository(catalogDbContext);
            Publishers = new PublisherRepository(catalogDbContext);
            Platforms = new PlatformRepository(catalogDbContext);
            Languages = new LanguageRepository(catalogDbContext);
            Ganres = new GanreRepository(catalogDbContext);
            Developers = new DeveloperRepository(catalogDbContext);
        }

        public IRepository<VideoGame> VideoGames { get; }

        public IRepository<SystemRequirements> SystemRequirements { get; }

        public IRepository<Publisher> Publishers { get; }

        public IRepository<Platform> Platforms { get; }

        public IRepository<Language> Languages { get; }

        public IRepository<Ganre> Ganres { get; }

        public IRepository<Developer> Developers { get; }
    }
}
