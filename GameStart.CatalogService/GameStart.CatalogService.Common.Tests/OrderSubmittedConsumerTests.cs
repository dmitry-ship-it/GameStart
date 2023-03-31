using GameStart.CatalogService.Common.Consumers;
using GameStart.CatalogService.Data.Models;
using GameStart.CatalogService.Data.Repositories;
using GameStart.Shared.MessageBus.Models.OrderModels;
using MassTransit;

namespace GameStart.CatalogService.Common.Tests
{
    public class OrderSubmittedConsumerTests
    {
        private readonly IRepositoryWrapper repositoryMock;
        private readonly ICollection<VideoGame> repositoryReturnValues;
        private readonly ConsumeContext<OrderSubmitted> contextMock;
        private readonly OrderSubmitted testMessage;
        private readonly OrderSubmittedConsumer consumer;

        public OrderSubmittedConsumerTests()
        {
            repositoryMock = Substitute.For<IRepositoryWrapper>();
            repositoryReturnValues = new[]
            {
                new VideoGame() { Id = Guid.NewGuid(), Title = "VideoGame 1", Price = 17.99M },
                new VideoGame() { Id = Guid.NewGuid(), Title = "VideoGame 2", Price = 59.99M }
            };
            contextMock = Substitute.For<ConsumeContext<OrderSubmitted>>();
            testMessage = new()
            {
                Id = Guid.NewGuid(),
                OrderItems = repositoryReturnValues.Select(videoGame => new OrderItem
                {
                    GameId = videoGame.Id,
                    Title = videoGame.Title
                }).ToArray()
            };
            consumer = new(repositoryMock);
        }

        [Fact]
        public async Task Consume_ShouldAcceptOrder_WhenAllGamesExist()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            repositoryMock.VideoGames.FindAllAsync(false, cancellationToken).Returns(repositoryReturnValues);
            contextMock.Message.Returns(testMessage);
            contextMock.CancellationToken.Returns(cancellationToken);
            var expected = new OrderAccepted
            {
                Id = testMessage.Id,
                UserId = testMessage.UserId,
                TotalPrice = repositoryReturnValues.Sum(value => value.Price),
                OrderItems = testMessage.OrderItems.Select(item =>
                {
                    item.Title = repositoryReturnValues.First(entity => entity.Id == item.GameId).Title;

                    return item;
                }).ToArray()
            };

            // Act
            await consumer.Consume(contextMock);

            // Assert
            await contextMock.Received().Publish(expected, contextMock.CancellationToken);
        }

        [Fact]
        public async Task Consume_ShouldAcceptOrder_WhenMessageContainsLessItemsThenInRepository()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            repositoryMock.VideoGames.FindAllAsync(false, cancellationToken).Returns(repositoryReturnValues);
            contextMock.Message.Returns(testMessage);
            contextMock.CancellationToken.Returns(cancellationToken);
            var expected = new OrderAccepted
            {
                Id = testMessage.Id,
                UserId = testMessage.UserId,
                TotalPrice = repositoryReturnValues.Sum(value => value.Price),
                OrderItems = testMessage.OrderItems.Take(1).Select(item =>
                {
                    item.Title = repositoryReturnValues.First(entity => entity.Id == item.GameId).Title;

                    return item;
                }).ToArray()
            };

            // Act
            await consumer.Consume(contextMock);

            // Assert
            await contextMock.Received().Publish(expected, contextMock.CancellationToken);
        }

        [Fact]
        public async Task Consume_ShouldFaultOrder_WhenGameIsMissingInRepository()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            repositoryMock.VideoGames.FindAllAsync(false, cancellationToken).Returns(repositoryReturnValues.Take(1).ToArray());
            contextMock.Message.Returns(testMessage);
            contextMock.CancellationToken.Returns(cancellationToken);
            var expected = new OrderFaulted
            {
                Id = testMessage.Id,
                UserId = testMessage.UserId
            };

            // Act
            await consumer.Consume(contextMock);

            // Assert
            await contextMock.Received().Publish(expected, contextMock.CancellationToken);
        }
    }
}
