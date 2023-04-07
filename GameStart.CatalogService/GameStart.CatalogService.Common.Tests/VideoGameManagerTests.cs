using AutoMapper;
using GameStart.CatalogService.Common.Caching;
using GameStart.CatalogService.Common.Elasticsearch;
using GameStart.CatalogService.Common.Elasticsearch.Search;
using GameStart.CatalogService.Common.ViewModels;
using GameStart.CatalogService.Data.Models;
using GameStart.CatalogService.Data.Repositories;
using NSubstitute.ReceivedExtensions;
using System.Linq.Expressions;

namespace GameStart.CatalogService.Common.Tests
{
    public class VideoGameManagerTests
    {
        private readonly IRepositoryWrapper repositoryMock;
        private readonly ISelectorByPage<VideoGame> selectorByPageMock;
        private readonly IGameCounter gameCounterMock;
        private readonly IMapper mapperMock;
        private readonly IRedisCacheService cacheMock;
        private readonly IElasticsearchService<VideoGame, VideoGameSearchRequest> elasticsearchMock;
        private readonly IVideoGameManager sut;

        public VideoGameManagerTests()
        {
            repositoryMock = Substitute.For<IRepositoryWrapper>();
            selectorByPageMock = Substitute.For<ISelectorByPage<VideoGame>>();
            gameCounterMock = Substitute.For<IGameCounter>();
            mapperMock = Substitute.For<IMapper>();
            cacheMock = Substitute.For<IRedisCacheService>();
            elasticsearchMock = Substitute.For<IElasticsearchService<VideoGame, VideoGameSearchRequest>>();

            sut = new VideoGameManager(
                repositoryMock,
                selectorByPageMock,
                gameCounterMock,
                mapperMock,
                cacheMock,
                elasticsearchMock);
        }

        [Fact]
        public async Task GetByIdAsync_WhenCachedExists_ShouldReturnCachedVideoGame()
        {
            // Arrange
            var id = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;
            var cachedVideoGame = new VideoGame { Id = id };
            cacheMock.GetAsync<VideoGame>(id.ToString(), cancellationToken)
                .Returns(cachedVideoGame);

            // Act
            var result = await sut.GetByIdAsync(id, cancellationToken);

            // Assert
            result.Should().BeSameAs(cachedVideoGame);
            await cacheMock.DidNotReceive()
                .SetAsync(Arg.Any<string>(), Arg.Any<VideoGame>(), cancellationToken);
            await repositoryMock.DidNotReceive().VideoGames.FindByConditionAsync(
                Arg.Any<Expression<Func<VideoGame, bool>>>(),
                cancellationToken);
        }

        [Fact]
        public async Task GetByIdAsync_WhenNotFoundInCacheButFoundInRepository_ShouldReturnVideoGame()
        {
            // Arrange
            var id = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;
            var foundVideoGame = new VideoGame { Id = id };
            cacheMock.GetAsync<VideoGame>(id.ToString(), cancellationToken).Returns((VideoGame)null!);
            repositoryMock.VideoGames
                .FindByConditionAsync(Arg.Any<Expression<Func<VideoGame, bool>>>(), cancellationToken)
                .Returns(new[] { foundVideoGame });

            // Act
            var result = await sut.GetByIdAsync(id, cancellationToken);

            // Assert
            result.Should().BeEquivalentTo(foundVideoGame);
            await cacheMock.Received(1).SetAsync(
                id.ToString(),
                foundVideoGame,
                cancellationToken);
        }

        [Fact]
        public async Task GetByIdAsync_WhenNotFoundInRepository_ShouldReturnNull()
        {
            // Arrange
            var id = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;
            cacheMock.GetAsync<VideoGame>(id.ToString(), cancellationToken).Returns((VideoGame)null!);

            repositoryMock.VideoGames
                .FindByConditionAsync(Arg.Any<Expression<Func<VideoGame, bool>>>(), cancellationToken)
                .Returns(Enumerable.Empty<VideoGame>());

            // Act
            var result = await sut.GetByIdAsync(id, cancellationToken);

            // Assert
            result.Should().BeNull();
            await cacheMock.DidNotReceive().SetAsync(
                Arg.Any<string>(),
                Arg.Any<VideoGame>(),
                cancellationToken);
        }

        [Fact]
        public async Task GetByPageAsync_WhenCachedExists_ShouldReturnCachedVideoGames()
        {
            // Arrange
            const int page = 1;
            const int pageSize = 10;
            var cacheKey = $"{typeof(VideoGame).Name};{page};{pageSize}";
            var cancellationToken = CancellationToken.None;
            var cachedVideoGames = new List<VideoGame> { new VideoGame { Id = Guid.NewGuid() } };
            cacheMock.GetAsync<IEnumerable<VideoGame>>(cacheKey, cancellationToken)
                .Returns(cachedVideoGames);

            var selectorByPage = Substitute.For<ISelectorByPage<VideoGame>>();

            // Act
            var result = await sut.GetByPageAsync(page, pageSize, cancellationToken);

            // Assert
            result.Should().BeEquivalentTo(cachedVideoGames);
            await cacheMock.DidNotReceive().SetAsync(
                Arg.Any<string>(),
                Arg.Any<IEnumerable<VideoGame>>(),
                cancellationToken);
            await selectorByPage.DidNotReceive().GetByPageAsync(page, pageSize, cancellationToken);
        }

        [Fact]
        public async Task GetByPageAsync_WhenNotCached_ShouldReturnVideoGames()
        {
            // Arrange
            const int page = 1;
            const int pageSize = 10;
            var cacheKey = $"{typeof(VideoGame).Name};{page};{pageSize}";
            var cancellationToken = CancellationToken.None;
            var videoGames = new List<VideoGame> { new VideoGame { Id = Guid.NewGuid() } };
            cacheMock.GetAsync<IEnumerable<VideoGame>>(cacheKey, cancellationToken)
                .Returns((IEnumerable<VideoGame>)null!);
            selectorByPageMock.GetByPageAsync(page, pageSize, cancellationToken)
                .Returns(videoGames);

            // Act
            var result = await sut.GetByPageAsync(page, pageSize, cancellationToken);

            // Assert
            result.Should().BeEquivalentTo(videoGames);
            await cacheMock.Received(1)
                .SetAsync(cacheKey, videoGames, cancellationToken);
        }

        [Fact]
        public async Task GetByPageAsync_WhenPageIsLessThanOne_ShouldThrowArgumentOutOfRangeException()
        {
            // Arrange
            const int page = 0;
            const int pageSize = 10;
            var cancellationToken = CancellationToken.None;

            // Act
            var act = async () => await sut.GetByPageAsync(page, pageSize, cancellationToken);

            // Assert
            await act.Should().ThrowAsync<ArgumentOutOfRangeException>();
        }

        [Fact]
        public async Task GetByPageAsync_WhenPageSizeIsLessThanOne_ShouldThrowArgumentOutOfRangeException()
        {
            // Arrange
            const int page = 1;
            const int pageSize = 0;
            var cancellationToken = CancellationToken.None;

            // Act
            var act = async () => await sut.GetByPageAsync(page, pageSize, cancellationToken);

            // Assert
            await act.Should().ThrowAsync<ArgumentOutOfRangeException>();
        }

        [Fact]
        public async Task AddAsync_WhenViewModelIsValid_ShouldAddVideoGame()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var viewModel = new VideoGameViewModel
            {
                Title = "New Video Game",
                Price = 59.99M,
            };
            var videoGame = new VideoGame
            {
                Id = Guid.NewGuid(),
                Title = viewModel.Title,
                Price = viewModel.Price.Value,
            };
            mapperMock.Map<VideoGame>(viewModel).Returns(videoGame);

            // Act
            await sut.AddAsync(viewModel, cancellationToken);

            // Assert
            await repositoryMock.VideoGames.Received(1)
                .CreateAsync(videoGame, cancellationToken);
            await elasticsearchMock.Received(1)
                .InsertAsync(videoGame, cancellationToken);
            await cacheMock.Received(1)
                .SetAsync(videoGame.Id.ToString(), videoGame, cancellationToken);
        }

        [Fact]
        public async Task UpdateAsync_WhenIdExistsAndModelIsValid_ShouldUpdateVideoGame()
        {
            // Arrange
            var id = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;
            var model = new VideoGameViewModel { Title = "Updated Video Game", Price = 59.99M };
            var originalVideoGame = new VideoGame { Id = id, Title = "Original Video Game", Price = 69.99M };
            var updatedByTrackerVideoGame = new VideoGame { Id = id, Title = model.Title, Price = model.Price.Value };
            mapperMock.Map(model, originalVideoGame).Returns(originalVideoGame);
            repositoryMock.VideoGames
                .FindByConditionAsync(Arg.Any<Expression<Func<VideoGame, bool>>>(), cancellationToken)
                .Returns(new[] { originalVideoGame });

            // Act
            var result = await sut.UpdateAsync(id, model, cancellationToken);

            // Assert
            result.Should().BeTrue();
            await repositoryMock.VideoGames.Received(1).UpdateAsync(originalVideoGame, cancellationToken);
            updatedByTrackerVideoGame.Title.Should().Be(model.Title);
            updatedByTrackerVideoGame.Price.Should().Be(model.Price);
        }

        [Fact]
        public async Task UpdateAsync_WhenIdDoesNotExist_ShouldReturnFalse()
        {
            // Arrange
            var id = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;
            var model = new VideoGameViewModel { Title = "Updated Video Game", Price = 59.99M };

            repositoryMock.VideoGames
                .FindByConditionAsync(Arg.Any<Expression<Func<VideoGame, bool>>>(), cancellationToken)
                .Returns(Array.Empty<VideoGame>());

            // Act
            var result = await sut.UpdateAsync(id, model, cancellationToken);

            // Assert
            result.Should().BeFalse();
            await repositoryMock.VideoGames.DidNotReceive().UpdateAsync(Arg.Any<VideoGame>(), cancellationToken);
            await elasticsearchMock.DidNotReceive().UpdateAsync(Arg.Any<VideoGame>(), cancellationToken);
            await cacheMock.DidNotReceive().SetAsync(Arg.Any<string>(), Arg.Any<VideoGame>(), cancellationToken);
        }

        [Fact]
        public async Task SearchAsync_ShouldReturnFoundVideoGames()
        {
            //Arrange
            var cancellationToken = CancellationToken.None;
            var request = new VideoGameSearchRequest()
            {
                Title = "Test title"
            };

            var expected = new VideoGame[]
            {
                new() { Title = request.Title }
            };

            elasticsearchMock.SearchAsync(request, cancellationToken).Returns(expected);

            //Act
            var actual = await sut.SearchAsync(request, cancellationToken);

            //Assert
            actual.Should().BeEquivalentTo(expected);
            await elasticsearchMock.Received().SearchAsync(request, cancellationToken);
        }

        [Fact]
        public async Task GetDevelopersAsync_ShouldReturnDevelopers()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var developers = new List<Developer>
            {
                new() { Name = "Dev 1" },
                new() { Name = "Dev 2" }
            };

            repositoryMock.Developers.FindAllAsync(false, cancellationToken).Returns(developers);

            // Act
            var result = await sut.GetDevelopersAsync(cancellationToken);

            // Assert
            result.Should().BeEquivalentTo(developers);
            await repositoryMock.Received().Developers.FindAllAsync(false, cancellationToken);
        }

        [Fact]
        public async Task GetGenresAsync_ShouldReturnGenres()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var genres = new List<Genre>
            {
                new() { Name = "Genre 1" },
                new() { Name = "Genre 2" }
            };

            repositoryMock.Genres.FindAllAsync(false, cancellationToken).Returns(genres);

            // Act
            var result = await sut.GetGenresAsync(cancellationToken);

            // Assert
            result.Should().BeEquivalentTo(genres);
            await repositoryMock.Received().Genres.FindAllAsync(false, cancellationToken);
        }

        [Fact]
        public async Task GetLanguagesAsync_ShouldReturnLanguages()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var languages = new List<Language>
            {
                new() { Name = "Language 1" },
                new() { Name = "Language 2" }
            };

            repositoryMock.Languages.FindAllAsync(false, cancellationToken).Returns(languages);

            // Act
            var result = await sut.GetLanguagesAsync(cancellationToken);

            // Assert
            result.Should().BeEquivalentTo(languages);
            await repositoryMock.Received().Languages.FindAllAsync(false, cancellationToken);
        }

        [Fact]
        public async Task GetPlatformsAsync_ShouldReturnPlatforms()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var platforms = new List<Platform>
            {
                new() { Name = "Platform 1" },
                new() { Name = "Platform 2" }
            };

            repositoryMock.Platforms.FindAllAsync(false, cancellationToken).Returns(platforms);

            // Act
            var result = await sut.GetPlatformsAsync(cancellationToken);

            // Assert
            result.Should().BeEquivalentTo(platforms);
            await repositoryMock.Received().Platforms.FindAllAsync(false, cancellationToken);
        }

        [Fact]
        public async Task GetGamesCountAsync_ShouldReturnGamesCount()
        {
            // Arrange
            const int gamesCount = 10;
            var cancellationToken = CancellationToken.None;
            gameCounterMock.CountAsync(cancellationToken).Returns(gamesCount);

            // Act
            var result = await sut.GetGamesCountAsync(cancellationToken);

            // Assert
            result.Should().Be(gamesCount);
            await gameCounterMock.Received().CountAsync(cancellationToken);
        }
    }
}
