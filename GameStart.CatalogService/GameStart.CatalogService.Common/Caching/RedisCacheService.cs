using GameStart.CatalogService.Data.EntityConfigurations.ValueConverters;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GameStart.CatalogService.Common.Caching
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(10))
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(120));

        private readonly JsonSerializerOptions jsonOptions;

        private readonly IDistributedCache cache;

        public RedisCacheService(IDistributedCache cache)
        {
            ArgumentNullException.ThrowIfNull(cache);
            this.cache = cache;

            jsonOptions = new();
            jsonOptions.Converters.Add(new DateOnlyJsonConverter());
            jsonOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
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
                : JsonSerializer.Deserialize<T>(cached, jsonOptions);
        }

        public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default)
        {
            var serialized = JsonSerializer.Serialize(value, jsonOptions);
            await cache.SetStringAsync(key, serialized, cacheOptions, cancellationToken);
        }
    }
}
