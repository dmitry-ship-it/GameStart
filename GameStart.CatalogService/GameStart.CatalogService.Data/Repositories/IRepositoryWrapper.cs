using GameStart.CatalogService.Data.Models;

namespace GameStart.CatalogService.Data.Repositories
{
    public interface IRepositoryWrapper
    {
        IRepository<VideoGame> VideoGames { get; }

        IRepository<SystemRequirements> SystemRequirements { get; }

        IRepository<Publisher> Publishers { get; }

        IRepository<Platform> Platforms { get; }

        IRepository<Language> Languages { get; }

        IRepository<LanguageAvailability> LanguageAvailabilies { get; }

        IRepository<Ganre> Ganres { get; }

        IRepository<Developer> Developers { get; }
    }
}
