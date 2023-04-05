using GameStart.CatalogService.Data.Models;
using GameStart.CatalogService.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;

namespace GameStart.CatalogService.Data.Tests
{
    public class RepositoryBaseTests
    {
        private readonly CatalogDbContext dbContextMock;
        private readonly RepositoryBase<Platform, CatalogDbContext> sut;

        public RepositoryBaseTests()
        {
            dbContextMock = Substitute.For<CatalogDbContext>(new DbContextOptions<CatalogDbContext>());
            sut = new PlatformRepository(dbContextMock);
        }

        [Fact]
        public async Task FindAllAsync_ShouldReturnEntities()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var expectedEntities = new List<Platform>
            {
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() }
            };

            var dbSet = expectedEntities.AsQueryable().BuildMockDbSet();
            dbContextMock.Set<Platform>().Returns(dbSet);

            // Act
            var result = await sut.FindAllAsync(true, cancellationToken);

            // Assert
            result.Should().BeEquivalentTo(expectedEntities);
        }

        [Fact]
        public async Task FindByConditionAsync_ShouldReturnEntities()
        {
            // Arrange
            const string id = "3390C6CE-C594-4ABD-BE5F-E6ACB51E378F";
            var cancellationToken = CancellationToken.None;
            var entities = new List<Platform>
            {
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.Parse(id) }
            };

            var dbSet = entities.AsQueryable().BuildMockDbSet();
            dbContextMock.Set<Platform>().Returns(dbSet);

            // Act
            var result = await sut.FindByConditionAsync(
                entity => entity.Id == Guid.Parse(id), cancellationToken);

            // Assert
            result.Should().BeEquivalentTo(
                new[] { entities.Find(entity => entity.Id == Guid.Parse(id)) });
        }

        [Fact]
        public async Task CreateAsync_ShouldSaveEntity()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var entity = new Platform()
            {
                Id = Guid.NewGuid(),
                Name = "Test platform"
            };

            var dbSet = new List<Platform>().AsQueryable().BuildMockDbSet();
            dbContextMock.Set<Platform>().Returns(dbSet);

            // Act
            await sut.CreateAsync(entity, cancellationToken);

            // Assert
            await dbSet.Received().AddAsync(entity, cancellationToken);
            await dbContextMock.Received().SaveChangesAsync(cancellationToken);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateEntity()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var oldEntity = new Platform()
            {
                Id = Guid.NewGuid(),
                Name = "Old platform"
            };

            var newEntity = new Platform()
            {
                Id = oldEntity.Id,
                Name = "New Platform"
            };

            var dbSet = new List<Platform>() { oldEntity }.AsQueryable().BuildMockDbSet();
            dbContextMock.Set<Platform>().Returns(dbSet);

            // Act
            await sut.UpdateAsync(newEntity, cancellationToken);

            // Assert
            dbSet.Received().Update(newEntity);
            await dbContextMock.Received().SaveChangesAsync(cancellationToken);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteEntity()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var entity = new Platform()
            {
                Id = Guid.NewGuid(),
                Name = "Test platform"
            };

            var dbSet = new List<Platform>() { entity }.AsQueryable().BuildMockDbSet();
            dbContextMock.Set<Platform>().Returns(dbSet);

            // Act
            await sut.DeleteAsync(entity, cancellationToken);

            // Assert
            dbSet.Received().Remove(entity);
            await dbContextMock.Received().SaveChangesAsync(cancellationToken);
        }
    }
}
