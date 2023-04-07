using GameStart.IdentityService.Api.Controllers;
using GameStart.IdentityService.Common;
using GameStart.IdentityService.Data.Models;
using GameStart.IdentityService.Data.Repositories;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GameStart.IdentityService.Api.Tests
{
    public class InventoryControllerTests
    {
        private readonly InventoryManager inventoryManagerMock;
        private readonly InventoryController sut;

        public InventoryControllerTests()
        {
            inventoryManagerMock = Substitute.For<InventoryManager>(Substitute.For<IRepository<InventoryItem>>());

            var httpContextMock = Substitute.For<HttpContext>();

            var testClaim = new Claim("email", "example@mail.com");
            httpContextMock.User.Returns(new ClaimsPrincipal(
                Identity.Create("test", testClaim)));

            sut = new InventoryController(inventoryManagerMock)
            {
                ControllerContext = new() { HttpContext = httpContextMock }
            };
        }

        [Fact]
        public async Task GetItemAsync_WhenResultIsNull_ShouldReturnNotFound()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;
            inventoryManagerMock.GetByGameIdAsync(gameId, Arg.Any<IEnumerable<Claim>>(), cancellationToken)
                .Returns((InventoryItem)null!);

            // Act
            var result = await sut.GetItemAsync(gameId, cancellationToken);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetItemAsync_WhenResultIsNotNull_ShouldReturnOkObjectResult()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;
            var item = new InventoryItem() { Title = "Test title" };
            inventoryManagerMock.GetByGameIdAsync(gameId, Arg.Any<IEnumerable<Claim>>(), cancellationToken)
                .Returns(Task.FromResult(item));

            // Act
            var result = await sut.GetItemAsync(gameId, cancellationToken);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().BeEquivalentTo(item);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnOkResult()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var items = new List<InventoryItem>()
            {
                new() { Title = "Test item 1" },
                new() { Title = "Test item 2" }
            };

            inventoryManagerMock.GetByUserClaimsAsync(Arg.Any<IEnumerable<Claim>>(), cancellationToken)
                .Returns(Task.FromResult<IEnumerable<InventoryItem>>(items));

            // Act
            var result = await sut.GetAllAsync(cancellationToken);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().BeEquivalentTo(items);
        }

        [Fact]
        public async Task DeleteAsync_WhenDeletedSuccessfully_ShouldReturnNoContentResult()
        {
            //Arrange
            var gameId = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;

            inventoryManagerMock.DeleteGameByUserClaimsAsync(gameId, Arg.Any<IEnumerable<Claim>>(), cancellationToken)
                .Returns(true);

            //Act
            var result = await sut.DeleteAsync(gameId, cancellationToken);

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteAsync_WhenDeletedSuccessfully_ShouldReturnNotFoundResult()
        {
            //Arrange
            var gameId = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;

            inventoryManagerMock.DeleteGameByUserClaimsAsync(gameId, Arg.Any<IEnumerable<Claim>>(), cancellationToken)
                .Returns(false);

            //Act
            var result = await sut.DeleteAsync(gameId, cancellationToken);

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
