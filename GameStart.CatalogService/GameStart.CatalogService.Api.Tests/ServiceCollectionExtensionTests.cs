using GameStart.CatalogService.Api.Extensions;
using GameStart.CatalogService.Data.Models;
using GameStart.CatalogService.Data.Repositories;
using GameStart.CatalogService.Data;
using Microsoft.Extensions.DependencyInjection;
using GameStart.CatalogService.Common.Services;
using GameStart.CatalogService.Common;
using GameStart.Shared.Services;
using AutoMapper;
using GameStart.CatalogService.Common.ViewModels;

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
    }
}
