using GameStart.CatalogService.Common.Elasticsearch.Search;
using GameStart.CatalogService.Common.ViewModels;
using GameStart.CatalogService.Data.Models;

namespace GameStart.CatalogService.Common
{
    public interface IVideoGameManager
    {
        Task<VideoGame> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<IEnumerable<VideoGame>> GetByPageAsync(int page, int pageSize, CancellationToken cancellationToken = default);

        Task AddAsync(VideoGameViewModel viewModel, CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

        Task<bool> UpdateAsync(Guid id, VideoGameViewModel model, CancellationToken cancellationToken = default);

        Task<IEnumerable<VideoGame>> SearchAsync(VideoGameSearchRequest request, CancellationToken cancellationToken = default);

        Task<IEnumerable<Developer>> GetDevelopersAsync(CancellationToken cancellationToken = default);

        Task<IEnumerable<Genre>> GetGenresAsync(CancellationToken cancellationToken = default);

        Task<IEnumerable<Language>> GetLanguagesAsync(CancellationToken cancellationToken = default);

        Task<IEnumerable<Platform>> GetPlatformsAsync(CancellationToken cancellationToken = default);

        Task<int> GetGamesCountAsync(CancellationToken cancellationToken = default);
    }
}
