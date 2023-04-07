using GameStart.CatalogService.Api.Controllers;
using GameStart.CatalogService.Common;
using GameStart.CatalogService.Common.Elasticsearch.Search;
using GameStart.CatalogService.Common.ViewModels;
using GameStart.CatalogService.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameStart.CatalogService.Api.Tests
{
    public class CatalogControllerTests
    {
        [Fact]
        public async Task GetByPageAsync_WhenManagerGetsByPage_ShouldReturnOkWithData()
        {
            // Arrange
            const int page = 1;
            const int pageSize = 10;
            var cancellationToken = CancellationToken.None;
            var result = Enumerable.Repeat(new VideoGame(), 10);
            var manager = Substitute.For<IVideoGameManager>();
            manager.GetByPageAsync(page, pageSize, cancellationToken).Returns(result);
            var controller = new CatalogController(manager);

            // Act
            var actionResult = await controller.GetByPageAsync(page, pageSize, cancellationToken);
            var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;

            // Assert
            okResult.Should().NotBeNull();
            okResult.Value.Should().Be(result);
            await manager.Received().GetByPageAsync(page, pageSize, cancellationToken);
        }

        [Fact]
        public async Task GetByIdAsync_WhenManagerGetsById_ShouldReturnOkWithData()
        {
            // Arrange
            var id = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;
            var result = new VideoGame();
            var manager = Substitute.For<IVideoGameManager>();
            manager.GetByIdAsync(id, cancellationToken).Returns(result);
            var controller = new CatalogController(manager);

            // Act
            var actionResult = await controller.GetByIdAsync(id, cancellationToken);
            var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;

            // Assert
            okResult.Should().NotBeNull();
            okResult.Value.Should().Be(result);
            await manager.Received().GetByIdAsync(id, cancellationToken);
        }

        [Fact]
        public async Task GetByIdAsync_WhenManagerReturnsNull_ShouldReturnNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;
            var manager = Substitute.For<IVideoGameManager>();
            manager.GetByIdAsync(id, cancellationToken).Returns((VideoGame?)null);
            var controller = new CatalogController(manager);

            // Act
            var actionResult = await controller.GetByIdAsync(id, cancellationToken);

            // Assert
            actionResult.Should().BeOfType<NotFoundResult>();
            await manager.Received().GetByIdAsync(id, cancellationToken);
        }

        [Fact]
        public async Task CreateAsync_WhenManagerAddsVideoGame_ShouldReturnOk()
        {
            // Arrange
            var model = new VideoGameViewModel();
            var cancellationToken = CancellationToken.None;
            var manager = Substitute.For<IVideoGameManager>();
            var controller = new CatalogController(manager);

            // Act
            var result = await controller.CreateAsync(model, cancellationToken);

            // Assert
            result.Should().BeOfType<OkResult>();
            await manager.Received().AddAsync(model, cancellationToken);
        }

        [Fact]
        public async Task UpdateAsync_WhenManagerReturnsTrue_ShouldReturnOk()
        {
            // Arrange
            var id = Guid.NewGuid();
            var viewModel = new VideoGameViewModel();
            var cancellationToken = CancellationToken.None;
            var manager = Substitute.For<IVideoGameManager>();
            manager.UpdateAsync(id, viewModel, cancellationToken).Returns(true);
            var controller = new CatalogController(manager);

            // Act
            var result = await controller.UpdateAsync(id, viewModel, cancellationToken);

            // Assert
            result.Should().BeOfType<OkResult>();
            await manager.Received().UpdateAsync(id, viewModel, cancellationToken);
        }

        [Fact]
        public async Task UpdateAsync_WhenManagerReturnsFalse_ShouldReturnNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var viewModel = new VideoGameViewModel();
            var cancellationToken = CancellationToken.None;
            var manager = Substitute.For<IVideoGameManager>();
            manager.UpdateAsync(id, viewModel, cancellationToken).Returns(false);
            var controller = new CatalogController(manager);

            // Act
            var result = await controller.UpdateAsync(id, viewModel, cancellationToken);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
            await manager.Received().UpdateAsync(id, viewModel, cancellationToken);
        }

        [Fact]
        public async Task DeleteAsync_WhenManagerDeletesVideoGame_ShouldReturnNoContent()
        {
            // Arrange
            var id = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;
            var manager = Substitute.For<IVideoGameManager>();
            manager.DeleteAsync(id, cancellationToken).Returns(true);
            var controller = new CatalogController(manager);

            // Act
            var result = await controller.DeleteAsync(id, cancellationToken);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            await manager.Received().DeleteAsync(id, cancellationToken);
        }

        [Fact]
        public async Task DeleteAsync_WhenManagerFailsToDelete_ShouldReturnNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;
            var manager = Substitute.For<IVideoGameManager>();
            manager.DeleteAsync(id, cancellationToken).Returns(false);
            var controller = new CatalogController(manager);

            // Act
            var result = await controller.DeleteAsync(id, cancellationToken);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
            await manager.Received().DeleteAsync(id, cancellationToken);
        }

        [Fact]
        public async Task SearchAsync_ShouldReturnOkObjectResult()
        {
            // Arrange
            var request = new VideoGameSearchRequest();
            var cancellationToken = CancellationToken.None;
            var searchResult = Enumerable.Repeat(new VideoGame(), 10);
            var manager = Substitute.For<IVideoGameManager>();
            manager.SearchAsync(request, cancellationToken).Returns(searchResult);
            var controller = new CatalogController(manager);

            // Act
            var result = await controller.SearchAsync(request, cancellationToken);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().Be(searchResult);
            await manager.Received().SearchAsync(request, cancellationToken);
        }

        [Fact]
        public async Task GetDevelopersAsync_ShouldReturnOkObjectResult()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var manager = Substitute.For<IVideoGameManager>();
            var models = Enumerable.Range(1, 5).Select(i => new Developer { Name = $"Developer {i}" });
            manager.GetDevelopersAsync(cancellationToken).Returns(models);
            var controller = new CatalogController(manager);

            // Act
            var result = await controller.GetDevelopersAsync(cancellationToken);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okObjectResult = (OkObjectResult)result;
            okObjectResult.Value.Should().BeEquivalentTo(models.Select(m => m));
            await manager.Received().GetDevelopersAsync(cancellationToken);
        }

        [Fact]
        public async Task GetGenresAsync_ShouldReturnOkObjectResult()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var manager = Substitute.For<IVideoGameManager>();
            var models = Enumerable.Range(1, 5).Select(i => new Genre { Name = $"Genre {i}" });
            manager.GetGenresAsync(cancellationToken).Returns(models);
            var controller = new CatalogController(manager);

            // Act
            var result = await controller.GetGenresAsync(cancellationToken);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okObjectResult = (OkObjectResult)result;
            okObjectResult.Value.Should().BeEquivalentTo(models.Select(m => m));
            await manager.Received().GetGenresAsync(cancellationToken);
        }

        [Fact]
        public async Task GetLanguagesAsync_ShouldReturnOkObjectResult()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var manager = Substitute.For<IVideoGameManager>();
            var models = Enumerable.Range(1, 5).Select(i => new Data.Models.Language { Name = $"Language {i}" });
            manager.GetLanguagesAsync(cancellationToken).Returns(models);
            var controller = new CatalogController(manager);

            // Act
            var result = await controller.GetLanguagesAsync(cancellationToken);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okObjectResult = (OkObjectResult)result;
            okObjectResult.Value.Should().BeEquivalentTo(models.Select(m => m));
            await manager.Received().GetLanguagesAsync(cancellationToken);
        }

        [Fact]
        public async Task GetPlatformsAsync_ShouldReturnOkObjectResult()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var manager = Substitute.For<IVideoGameManager>();
            var models = Enumerable.Range(1, 5).Select(i => new Platform { Name = $"Language {i}" });
            manager.GetPlatformsAsync(cancellationToken).Returns(models);
            var controller = new CatalogController(manager);

            // Act
            var result = await controller.GetPlatformsAsync(cancellationToken);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okObjectResult = (OkObjectResult)result;
            okObjectResult.Value.Should().BeEquivalentTo(models.Select(m => m));
            await manager.Received().GetPlatformsAsync(cancellationToken);
        }

        [Fact]
        public async Task GetGamesCountAsync_ShouldReturnOkObjectResult()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var manager = Substitute.For<IVideoGameManager>();
            manager.GetGamesCountAsync(cancellationToken).Returns(10);
            var controller = new CatalogController(manager);

            // Act
            var result = await controller.GetGamesCountAsync(cancellationToken);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okObjectResult = (OkObjectResult)result;
            okObjectResult.Value.Should().Be(10);
            await manager.Received().GetGamesCountAsync(cancellationToken);
        }
    }
}
