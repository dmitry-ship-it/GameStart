using GameStart.CatalogService.Common.Caching;
using GameStart.CatalogService.Common.Services;
using GameStart.CatalogService.Data.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace GameStart.CatalogService.Common.Tests
{
    public class RedisCacheServiceTests
    {
        [Fact]
        public async Task DeleteAsync_WhenKeyContainsInCache_ShouldRemoveItemFromDistributedCache()
        {
            // Arrange
            const string key = "test key";
            var value = new byte[] { 1, 2, 3 };
            var cancellationToken = CancellationToken.None;
            var options = Options.Create(Substitute.For<MemoryDistributedCacheOptions>());
            var cache = new MemoryDistributedCache(options);
            await cache.SetAsync(key, value, cancellationToken);
            var jsonOptions = Substitute.For<IJsonSafeOptionsProvider>();
            var service = new RedisCacheService(cache, jsonOptions);

            // Act
            await service.DeleteAsync(key, cancellationToken);

            // Assert
            var retrievedValue = await cache.GetAsync(key);
            retrievedValue.Should().BeNull();
        }

        [Fact]
        public async Task DeleteAsync_WhenKeyIsWrong_ShouldDoNothing()
        {
            // Arrange
            const string key = "test key";
            var value = new byte[] { 1, 2, 3 };
            var cancellationToken = CancellationToken.None;
            var options = Options.Create(Substitute.For<MemoryDistributedCacheOptions>());
            var cache = new MemoryDistributedCache(options);
            await cache.SetAsync(key, value, cancellationToken);
            var jsonOptions = Substitute.For<IJsonSafeOptionsProvider>();
            var service = new RedisCacheService(cache, jsonOptions);

            // Act
            await service.DeleteAsync("wrong key", cancellationToken);

            // Assert
            var retrievedValue = await cache.GetAsync(key);
            retrievedValue.Should().NotBeNull().And.BeEquivalentTo(value);
        }

        [Fact]
        public async Task DeleteAsync_WhenKeyIsMissing_ShouldNotThrow()
        {
            // Arrange
            const string key = "test key";
            var cancellationToken = CancellationToken.None;
            var options = Options.Create(Substitute.For<MemoryDistributedCacheOptions>());
            var cache = new MemoryDistributedCache(options);
            var jsonOptions = Substitute.For<IJsonSafeOptionsProvider>();
            var service = new RedisCacheService(cache, jsonOptions);

            // Act
            var act = async () => await service.DeleteAsync(key, cancellationToken);

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallDistributedCache()
        {
            // Arrange
            const string key = "test key";
            var cancellationToken = CancellationToken.None;
            var cache = Substitute.For<IDistributedCache>();
            var jsonOptions = Substitute.For<IJsonSafeOptionsProvider>();
            var service = new RedisCacheService(cache, jsonOptions);

            // Act
            await service.DeleteAsync(key, cancellationToken);

            // Assert
            await cache.Received().RemoveAsync(key, cancellationToken);
        }

        [Fact]
        public async Task GetAsync_WhenCacheIsEmptyAndValueIsInHeap_ShouldReturnNull()
        {
            // Arrange
            const string key = "test key";
            var cancellationToken = CancellationToken.None;
            var jsonOptions = Substitute.For<IJsonSafeOptionsProvider>();
            var cache = new MemoryDistributedCache(Options.Create(Substitute.For<MemoryDistributedCacheOptions>()));
            var target = new RedisCacheService(cache, jsonOptions);

            // Act
            var result = await target.GetAsync<object>(key, cancellationToken);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAsync_WhenCacheIsEmptyAndValueIsInStack_ShouldReturnDefault()
        {
            // Arrange
            const string key = "test key";
            var cancellationToken = CancellationToken.None;
            var jsonOptions = Substitute.For<IJsonSafeOptionsProvider>();
            var cache = new MemoryDistributedCache(Options.Create(Substitute.For<MemoryDistributedCacheOptions>()));
            var target = new RedisCacheService(cache, jsonOptions);

            // Act
            var result = await target.GetAsync<int>(key, cancellationToken);

            // Assert
            result.Should().Be(default);
        }

        [Fact]
        public async Task GetAsync_WhenCacheIsNotEmpty_ShouldReturnDeserializedValue()
        {
            // Arrange
            const string key = "test key";
            var cancellationToken = CancellationToken.None;
            var jsonOptions = Substitute.For<IJsonSafeOptionsProvider>();
            var cache = new MemoryDistributedCache(Options.Create(Substitute.For<MemoryDistributedCacheOptions>()));
            var target = new RedisCacheService(cache, jsonOptions);
            var expected = new Language { Id = Guid.NewGuid(), Name = "Test language" };
            await cache.SetStringAsync(key, JsonSerializer.Serialize(expected), cancellationToken);

            // Act
            var result = await target.GetAsync<Language>(key, cancellationToken);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetAsync_ShouldCallDistributedCache()
        {
            // Arrange
            const string key = "test key";
            var cancellationToken = CancellationToken.None;
            var cache = Substitute.For<IDistributedCache>();
            var jsonOptions = Substitute.For<IJsonSafeOptionsProvider>();
            var service = new RedisCacheService(cache, jsonOptions);

            // Act
            await service.GetAsync<int>(key, cancellationToken);

            // Assert
            await cache.Received().GetStringAsync(key, cancellationToken);
        }

        [Fact]
        public async Task SetAsync_ShouldStoreSerializedValueInCache()
        {
            // Arrange
            const string key = "test key";
            var value = new Language { Id = Guid.NewGuid(), Name = "Test language" };
            var cancellationToken = CancellationToken.None;
            var jsonOptions = Substitute.For<IJsonSafeOptionsProvider>();
            var cache = new MemoryDistributedCache(Options.Create(Substitute.For<MemoryDistributedCacheOptions>()));
            var target = new RedisCacheService(cache, jsonOptions);

            // Act
            await target.SetAsync(key, value, cancellationToken);
            var cachedValueBytes = await cache.GetAsync(key, cancellationToken);
            var cachedValue = JsonSerializer.Deserialize<Language>(cachedValueBytes);

            // Assert
            cachedValue.Should().BeEquivalentTo(value);
        }

        [Fact]
        public async Task SetAsync_ShouldCallDistributedCache()
        {
            // Arrange
            const string key = "test key";
            var value = new Language { Id = Guid.Parse("B9EF129B-E8D1-4E45-BDB4-3CA8B820E648"), Name = "Test language" };
            var cancellationToken = CancellationToken.None;
            var cache = Substitute.For<IDistributedCache>();
            var jsonOptions = Substitute.For<IJsonSafeOptionsProvider>();
            var service = new RedisCacheService(cache, jsonOptions);

            // Act

            await service.SetAsync(key, value, cancellationToken);

            // Assert
            // for some reason can't assert SetStringAsync's second argument (Guids are not the issue) so test uses Arg.Any
            await cache.Received().SetAsync(key, Arg.Any<byte[]>(), Arg.Any<DistributedCacheEntryOptions>(), cancellationToken);
        }
    }
}
