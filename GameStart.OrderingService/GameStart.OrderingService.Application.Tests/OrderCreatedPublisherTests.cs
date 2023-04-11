using AutoMapper;
using GameStart.OrderingService.Application.Services;
using GameStart.OrderingService.Core.Entities;
using GameStart.Shared.MessageBus.Models.OrderModels;
using MassTransit;

namespace GameStart.OrderingService.Application.Tests
{
    public class OrderCreatedPublisherTests
    {
        private readonly IPublishEndpoint publishEndpointMock;
        private readonly IMapper mapperMock;
        private readonly IGameKeyGeneratorService gameKeyGeneratorMock;

        private readonly OrderCreatedPublisher sut;

        public OrderCreatedPublisherTests()
        {
            publishEndpointMock = Substitute.For<IPublishEndpoint>();
            mapperMock = Substitute.For<IMapper>();
            gameKeyGeneratorMock = Substitute.For<IGameKeyGeneratorService>();

            sut = new OrderCreatedPublisher(
                publishEndpointMock,
                mapperMock,
                gameKeyGeneratorMock);
        }

        [Fact]
        public async Task PublishMessageAsync_ShouldPublishOrderSubmitted()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var order = new Order
            {
                Id = Guid.NewGuid(),
                Items = new Item[] { new() { Id = Guid.NewGuid() } }
            };

            var orderSubmitted = new OrderSubmitted
            {
                Id = order.Id,
                OrderItems = order.Items.Select(item => new OrderItem
                {
                    Id = item.Id
                }).ToArray()
            };

            mapperMock.Map<OrderSubmitted>(order).Returns(orderSubmitted);

            // Act
            await sut.PublishMessageAsync(order, cancellationToken);

            // Assert
            await publishEndpointMock.Received().Publish(Arg.Is<OrderSubmitted>(result =>
                result.Id == orderSubmitted.Id &&
                result.OrderItems == orderSubmitted.OrderItems), cancellationToken);
        }
    }
}
