using AutoMapper;
using GameStart.CatalogService.Api.Extensions;
using GameStart.CatalogService.Common;
using GameStart.CatalogService.Common.Caching;
using GameStart.CatalogService.Common.Elasticsearch;
using GameStart.CatalogService.Common.Elasticsearch.Search;
using GameStart.CatalogService.Common.Services;
using GameStart.CatalogService.Common.ViewModels;
using GameStart.CatalogService.Data;
using GameStart.CatalogService.Data.EntityConfigurations.ValueConverters;
using GameStart.CatalogService.Data.Models;
using GameStart.CatalogService.Data.Repositories;
using GameStart.Shared.Filters;
using GameStart.Shared.Services;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nest;
using System.Reflection;
using System.Text.Json.Serialization;

namespace GameStart.CatalogService.Api.Tests
{
    public class ServiceCollectionExtensionTests
    {
        [Theory]
        [InlineData(typeof(CatalogDbContext), typeof(CatalogDbContext))]
        [InlineData(typeof(IRepositoryWrapper), typeof(CatalogRepositoryWrapper))]
        [InlineData(typeof(ISelectorByPage<VideoGame>), typeof(VideoGameRepository))]
        [InlineData(typeof(IGameCounter), typeof(VideoGameRepository))]
        public void AddDbContextWithRepositories_ShouldRegisterServices(Type expectedServiceType, Type expectedImplementationType)
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddDbContextWithRepositories();

            // Assert
            services.Should().Contain(sd => sd.Lifetime == ServiceLifetime.Scoped && sd.ServiceType == expectedServiceType && sd.ImplementationType == expectedImplementationType);
        }

        [Fact]
        public void AddDbContextWithRepositories_ShouldReturnObjectOfSameType()
        {
            var services = new ServiceCollection();

            // Act
            var result = services.AddDbContextWithRepositories();

            // Assert
            result.Should().BeSameAs(services);
        }

        [Theory]
        [InlineData(ServiceLifetime.Scoped, typeof(IVideoGameManager), typeof(VideoGameManager))]
        [InlineData(ServiceLifetime.Singleton, typeof(IDateTimeProvider), typeof(DateTimeProvider))]
        [InlineData(ServiceLifetime.Singleton, typeof(IJsonSafeOptionsProvider), typeof(JsonSafeOptionsProvider))]
        public void AddServices_ShouldRegisterServices(ServiceLifetime lifetime, Type serviceType, Type implementationType)
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddServices();

            // Assert
            services.Should().Contain(sd => sd.Lifetime == lifetime && sd.ServiceType == serviceType && sd.ImplementationType == implementationType);
        }

        [Fact]
        public void AddServices_ShouldReturnObjectOfSameType()
        {
            var services = new ServiceCollection();

            // Act
            var result = services.AddServices();

            // Assert
            result.Should().BeSameAs(services);
        }

        [Fact]
        public void AddModelsMapper_ShouldAddScopedService()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddModelsMapper();

            // Assert
            services.Should().Contain(sd => sd.Lifetime == ServiceLifetime.Scoped && sd.ServiceType == typeof(IMapper) && sd.ImplementationFactory != null);
        }

        [Fact]
        public void AddModelsMapper_ShouldAddMapperWithProfile()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddModelsMapper();
            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var mapper = scope.ServiceProvider.GetService<IMapper>();

            // Assert
            mapper?.ConfigurationProvider.Should().NotBeNull();
            mapper?.ConfigurationProvider.FindTypeMapFor<VideoGameViewModel, VideoGame>().Should().NotBeNull();
        }

        [Fact]
        public void AddPreconfiguredJwtAuthentication_ShouldAddAuthenticationWithScheme()
        {
            // Arrange
            var services = Substitute.For<IServiceCollection>();

            // Act
            var result = services.AddPreconfiguredJwtAuthentication();

            // Assert
            result.Should().BeSameAs(services);
            services.Received().AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
        }

        [Fact]
        public void AddPreconfiguredJwtAuthentication_ShouldAddJwtBearer()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var result = services.AddPreconfiguredJwtAuthentication();

            // Assert
            result.Should().Contain(sd => sd.ServiceType == typeof(IPostConfigureOptions<JwtBearerOptions>));
        }

        [Fact]
        public void AddPreconfiguredJwtAuthentication_ShouldAddAuthorization()
        {
            // Arrange
            var services = Substitute.For<IServiceCollection>();

            // Act
            var result = services.AddPreconfiguredJwtAuthentication();

            // Assert
            result.Should().BeSameAs(services);
            services.Received().AddAuthorization();
        }

        [Fact]
        public void AddControllersWithJsonConfigurationAndFilters_ShouldAddEmailVerifiedActionFilter()
        {
            // Arrange
            var services = Substitute.For<IServiceCollection>();

            // Act
            var result = services.AddControllersWithJsonConfigurationAndFilters();

            // Assert
            result.Should().BeSameAs(services);
            services.Received().AddControllers(config => config.Filters.Add<EmailVerifiedActionFilter>());
        }

        [Fact]
        public void AddControllersWithJsonConfigurationAndFilters_ShouldAddDateOnlyJsonConverter()
        {
            // Arrange
            var services = Substitute.For<IServiceCollection>();

            // Act
            var result = services.AddControllersWithJsonConfigurationAndFilters();

            // Assert
            result.Should().BeSameAs(services);
            services.Received().AddControllers(config =>
                config.ReturnHttpNotAcceptable = true).AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Should()
                        .Contain(converter => converter is DateOnlyJsonConverter);
                });
        }

        [Fact]
        public void AddControllersWithJsonConfigurationAndFilters_ShouldIgnoreReferenceCycles()
        {
            // Arrange
            var services = Substitute.For<IServiceCollection>();

            // Act
            var result = services.AddControllersWithJsonConfigurationAndFilters();

            // Assert
            result.Should().BeSameAs(services);
            services.Received().AddControllers(config =>
                config.ReturnHttpNotAcceptable = true).AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler
                        .Should().Be(ReferenceHandler.IgnoreCycles);
                });
        }

        [Fact]
        public void AddMassTransitEventConsuming_ShouldAddBus()
        {
            // arrange
            var services = new ServiceCollection();

            // act
            var result = services.AddMassTransitEventConsuming();

            // assert
            result.Should().Contain(sd => sd.ServiceType == typeof(IBus));
        }

        [Fact]
        public void AddMassTransitEventConsuming_ShouldReturnObjectOfSameType()
        {
            // arrange
            var services = new ServiceCollection();

            // act
            var result = services.AddMassTransitEventConsuming();

            // assert
            result.Should().BeSameAs(services);
        }

        [Fact]
        public void AddRedisCache_ShouldAddDistributedCache()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddRedisCache();

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var cache = serviceProvider.GetService<IDistributedCache>();
            cache.Should().NotBeNull();
        }

        [Fact]
        public void AddRedisCache_ShouldConfigureRedisCacheOptions()
        {
            // Arrange
            var services = new ServiceCollection();
            const string environmentVariable = "REDIS_CACHE_URL";
            Environment.SetEnvironmentVariable(environmentVariable, "localhost:6379");

            // Act
            services.AddRedisCache();

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<RedisCacheOptions>>()?.Value;
            options.Should().NotBeNull();
            options?.Configuration.Should().BeEquivalentTo(Environment.GetEnvironmentVariable(environmentVariable));
            options?.InstanceName.Should().Be(Assembly.GetAssembly(typeof(ServiceCollectionExtension))!.GetName().Name);
        }

        [Fact]
        public void AddRedisCache_ShouldAddScopedRedisCacheService()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddSingleton<IJsonSafeOptionsProvider>(Substitute.For<JsonSafeOptionsProvider>());

            // Act
            services.AddRedisCache();

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var cacheService = scope.ServiceProvider.GetService<IRedisCacheService>();
            cacheService.Should().NotBeNull();
            cacheService.Should().BeOfType<RedisCacheService>();
        }

        [Fact]
        public void AddRedisCache_ShouldReturnObjectOfSameType()
        {
            // arrange
            var services = new ServiceCollection();

            // act
            var result = services.AddRedisCache();

            // assert
            result.Should().BeSameAs(services);
        }

        [Fact]
        public void AddElasticsearch_ShouldAddSingletonElasticClient()
        {
            // Arrange
            var services = new ServiceCollection();
            Environment.SetEnvironmentVariable("ELASTICSEARCH_URI", "localhost:9200");

            // Act
            services.AddElasticsearch();

            // Assert
            services.Should().Contain(x => x.ServiceType == typeof(IElasticClient)
                && x.ImplementationInstance is ElasticClient
                && x.Lifetime == ServiceLifetime.Singleton);
        }

        [Fact]
        public void AddElasticsearch_ShouldAddScopedElasticsearchService()
        {
            // Arrange
            var services = new ServiceCollection();
            Environment.SetEnvironmentVariable("ELASTICSEARCH_URI", "localhost:9200");

            // Act
            services.AddElasticsearch();

            // Assert
            services.Should().Contain(x => x.ServiceType == typeof(IElasticsearchService<VideoGame, VideoGameSearchRequest>)
                && x.ImplementationType == typeof(VideoGameSearchService)
                && x.Lifetime == ServiceLifetime.Scoped);
        }

        [Fact]
        public void AddElasticsearch_ShouldReturnObjectOfSameType()
        {
            // Arrange
            var services = new ServiceCollection();
            Environment.SetEnvironmentVariable("ELASTICSEARCH_URI", "localhost:9200");

            // Act
            var result = services.AddElasticsearch();

            // Assert
            result.Should().BeSameAs(services);
        }
    }
}
