using GameStart.CatalogService.Common.Services;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace GameStart.CatalogService.Common.Caching
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(5))
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

        private readonly IDistributedCache cache;
        private readonly IJsonSafeOptionsProvider jsonOptionsProvider;

        public RedisCacheService(IDistributedCache cache, IJsonSafeOptionsProvider jsonOptionsProvider)
        {
            this.cache = cache;
            this.jsonOptionsProvider = jsonOptionsProvider;
        }

        public async Task DeleteAsync(string key, CancellationToken cancellationToken = default)
        {
            await cache.RemoveAsync(key, cancellationToken);
        }

        public async Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            var cached = await cache.GetStringAsync(key, cancellationToken);

            return string.IsNullOrEmpty(cached)
                ? default
                : JsonSerializer.Deserialize<T>(cached, jsonOptionsProvider.JsonSerializerOptions);
        }

        public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default)
        {
            var serialized = JsonSerializer.Serialize(value, jsonOptionsProvider.JsonSerializerOptions);
            await cache.SetStringAsync(key, serialized, cacheOptions, cancellationToken);
        }
    }
}
