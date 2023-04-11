using GameStart.OrderingService.Core.Entities;
using GameStart.OrderingService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;

namespace GameStart.OrderingService.Infrastructure.Tests
{
    public class RepositoryTests
    {
        private readonly OrdersDbContext dbContextMock;
        private readonly Repository<Item> sut;

        public RepositoryTests()
        {
            dbContextMock = Substitute.For<OrdersDbContext>(new DbContextOptions<OrdersDbContext>());
            sut = new ItemRepository(dbContextMock);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllEntities()
        {
            // Arrange
            var entities = new Item[]
            {
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() },
            };

            var cancellationToken = CancellationToken.None;

            var dbSet = entities.AsQueryable().BuildMockDbSet();
            dbContextMock.Set<Item>().Returns(dbSet);

            // Act
            var result = await sut.GetAllAsync(cancellationToken);

            // Assert
            result.Should().BeEquivalentTo(entities);
        }

        [Fact]
        public async Task GetByConditionAsync_ShouldReturnEntitiesThatSatisfyProvidedExpression()
        {
            // Arrange
            var entities = new Item[]
            {
                new() { Id = Guid.NewGuid(), Title = "Some other title" },
                new() { Id = Guid.NewGuid(), Title = "Item A" },
                new() { Id = Guid.NewGuid(), Title = "Item B" },
            };

            var cancellationToken = CancellationToken.None;

            var dbSet = entities.AsQueryable().BuildMockDbSet();
            dbContextMock.Set<Item>().Returns(dbSet);

            // Act
            var result = await sut.GetByConditionAsync(
                item => item.Title.StartsWith("Item"), cancellationToken);

            // Assert
            result.Should().BeEquivalentTo(entities.Skip(1));
        }
    }
}
