using GameStart.IdentityService.Data;
using GameStart.IdentityService.Data.Models;
using GameStart.IdentityService.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;

namespace GameStart.IdentityService.Common.Tests
{
    public class RepositoryBaseTests
    {
        private readonly AccountsDbContext dbContextMock;
        private readonly RepositoryBase<InventoryItem> sut;

        public RepositoryBaseTests()
        {
            dbContextMock = Substitute.For<AccountsDbContext>(
                new DbContextOptions<AccountsDbContext>());
            sut = new InventoryItemRepository(dbContextMock);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddEntity()
        {
            // Arrange
            var item = new InventoryItem() { Id = Guid.NewGuid() };
            var cancellationToken = CancellationToken.None;

            var dbSet = new List<InventoryItem>()
            {
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() }
            }.AsQueryable().BuildMockDbSet();
            dbContextMock.Set<InventoryItem>().Returns(dbSet);

            // Act
            await sut.CreateAsync(item, cancellationToken);

            // Assert
            await dbContextMock.Received().Set<InventoryItem>().AddAsync(item, cancellationToken);
            await dbContextMock.Received().SaveChangesAsync();
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveEntity()
        {
            // Arrange
            var itemId = Guid.NewGuid();
            var item = new InventoryItem() { Id = itemId };
            var cancellationToken = CancellationToken.None;

            var dbSet = new List<InventoryItem>()
            {
                new() { Id = Guid.NewGuid() },
                item
            }.AsQueryable().BuildMockDbSet();
            dbContextMock.Set<InventoryItem>().Returns(dbSet);

            // Act
            await sut.DeleteAsync(item, cancellationToken);

            // Assert
            dbContextMock.Received().Set<InventoryItem>().Remove(item);
            await dbContextMock.Received().SaveChangesAsync();
        }

        [Fact]
        public async Task FindAllAsync_ShouldReturnAllItems()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var items = new List<InventoryItem>()
            {
                new() { Id = Guid.NewGuid(), Title = "Test title 1" },
                new() { Id = Guid.NewGuid(), Title = "Test title 2" }
            };

            var dbSet = items.AsQueryable().BuildMockDbSet();
            dbContextMock.Set<InventoryItem>().Returns(dbSet);

            // Act
            var result = await sut.FindAllAsync(cancellationToken);

            // Assert
            result.Should().BeEquivalentTo(items);
            dbSet.Received().AsNoTracking();
        }

        [Fact]
        public async Task FindByConditionAsync_ShouldReturnFoundItems()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var items = new List<InventoryItem>()
            {
                new() { Id = Guid.NewGuid(), Title = "Test title 1" },
                new() { Id = Guid.NewGuid(), Title = "Test title 2" },
                new() { Id = Guid.NewGuid(), Title = "Other item" }
            };

            var dbSet = items.AsQueryable().BuildMockDbSet();
            dbContextMock.Set<InventoryItem>().Returns(dbSet);

            // Act
            var result = await sut.FindByConditionAsync(
                item => item.Title.Contains("title"), cancellationToken);

            // Assert
            result.Should().BeEquivalentTo(items.Where(item =>
                item.Title.Contains("title")));
            dbSet.Received().AsNoTracking();
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateEntity()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;

            var oldEntity = new InventoryItem() { Id = Guid.NewGuid(), Title = "Old title" };
            var newEntity = new InventoryItem() { Id = oldEntity.Id, Title = "New title" };

            var dbSet = new List<InventoryItem>()
            {
                oldEntity,
                new() { Id = Guid.NewGuid() }
            }.AsQueryable().BuildMockDbSet();
            dbContextMock.Set<InventoryItem>().Returns(dbSet);

            // Act
            await sut.UpdateAsync(newEntity, cancellationToken);

            // Assert
            dbContextMock.Received().Set<InventoryItem>().Update(oldEntity);
            await dbContextMock.Received().SaveChangesAsync();
        }

        [Fact]
        public async Task CreateRangeAsync_ShouldCreateSequenceOfItems()
        {
            // Arrange
            var items = new List<InventoryItem>()
            {
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() }
            };

            var cancellationToken = CancellationToken.None;

            var dbSet = new List<InventoryItem>().AsQueryable().BuildMockDbSet();
            dbContextMock.Set<InventoryItem>().Returns(dbSet);

            // Act
            await sut.CreateRangeAsync(items, cancellationToken);

            // Assert
            await dbContextMock.Received().Set<InventoryItem>().AddRangeAsync(items, cancellationToken);
            await dbContextMock.Received().SaveChangesAsync();
        }
    }
}
