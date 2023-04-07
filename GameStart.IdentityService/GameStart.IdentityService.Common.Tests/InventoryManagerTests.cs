using GameStart.IdentityService.Data.Models;
using GameStart.IdentityService.Data.Repositories;
using System.Linq.Expressions;
using System.Security.Claims;

namespace GameStart.IdentityService.Common.Tests
{
    public class InventoryManagerTests
    {
        private readonly IRepository<InventoryItem> repositoryMock;
        private readonly InventoryManager sut;

        public InventoryManagerTests()
        {
            repositoryMock = Substitute.For<IRepository<InventoryItem>>();
            sut = new InventoryManager(repositoryMock);
        }

        [Fact]
        public async Task GetByUserClaimsAsync_ShouldReturnUsersInventoryItems()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var claims = Enumerable.Repeat(
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()), 1);
            var cancellationToken = CancellationToken.None;

            var items = new List<InventoryItem>()
            {
                new() { GameId = Guid.NewGuid(), User = new() { Id = Guid.NewGuid() } },
                new() { GameId = Guid.NewGuid(), User = new() { Id = userId } },
            };

            repositoryMock.FindByConditionAsync(Arg.Any<Expression<Func<InventoryItem, bool>>>(), cancellationToken)
                .Returns(items);

            // Act
            var result = await sut.GetByUserClaimsAsync(claims, cancellationToken);

            // Assert
            await repositoryMock.Received().FindByConditionAsync(
                Arg.Any<Expression<Func<InventoryItem, bool>>>(), cancellationToken);
            result.Should().BeEquivalentTo(items);
        }

        [Fact]
        public async Task GetByGameIdAsync_ShouldReturnInventoryItemWhenFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var claims = Enumerable.Repeat(
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()), 1);
            var cancellationToken = CancellationToken.None;

            var items = new List<InventoryItem>()
            {
                new() { GameId = Guid.NewGuid(), User = new() { Id = userId } },
            };

            repositoryMock.FindByConditionAsync(Arg.Any<Expression<Func<InventoryItem, bool>>>(), cancellationToken)
                .Returns(items);

            // Act
            var result = await sut.GetByGameIdAsync(items[0].GameId, claims, cancellationToken);

            // Assert
            await repositoryMock.Received().FindByConditionAsync(
                Arg.Any<Expression<Func<InventoryItem, bool>>>(), cancellationToken);
            result.Should().NotBeNull().And.BeEquivalentTo(items[0]);
        }

        [Fact]
        public async Task GetByGameIdAsync_ShouldReturnNullWhenNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var claims = Enumerable.Repeat(
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()), 1);
            var cancellationToken = CancellationToken.None;

            var items = Enumerable.Empty<InventoryItem>();

            repositoryMock.FindByConditionAsync(Arg.Any<Expression<Func<InventoryItem, bool>>>(), cancellationToken)
                .Returns(items);

            // Act
            var result = await sut.GetByGameIdAsync(Guid.NewGuid(), claims, cancellationToken);

            // Assert
            await repositoryMock.Received().FindByConditionAsync(
                Arg.Any<Expression<Func<InventoryItem, bool>>>(), cancellationToken);
            result.Should().BeNull();
        }

        [Fact]
        public async Task DeleteGameByUserClaimsAsync_WhenItemContainsInInventory_ShouldReturnTrue()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var claims = Enumerable.Repeat(
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()), 1);
            var cancellationToken = CancellationToken.None;

            var item = new InventoryItem()
            {
                GameId = Guid.NewGuid(),
                User = new() { Id = userId }
            };

            repositoryMock.FindByConditionAsync(Arg.Any<Expression<Func<InventoryItem, bool>>>(), cancellationToken)
                .Returns(Enumerable.Repeat(item, 1));

            // Act
            var result = await sut.DeleteGameByUserClaimsAsync(item.GameId, claims, cancellationToken);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteGameByUserClaimsAsync_WhenItemNotFound_ShouldReturnFalse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var claims = Enumerable.Repeat(
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()), 1);
            var cancellationToken = CancellationToken.None;

            repositoryMock.FindByConditionAsync(Arg.Any<Expression<Func<InventoryItem, bool>>>(), cancellationToken)
                .Returns(Enumerable.Empty<InventoryItem>());

            // Act
            var result = await sut.DeleteGameByUserClaimsAsync(Guid.NewGuid(), claims, cancellationToken);

            // Assert
            result.Should().BeFalse();
        }
    }
}
