namespace GameStart.CatalogService.Common.Caching
{
    public interface IRedisCacheService
    {
        Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default);

        Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default);

        Task DeleteAsync(string key, CancellationToken cancellationToken = default);
    }
}
