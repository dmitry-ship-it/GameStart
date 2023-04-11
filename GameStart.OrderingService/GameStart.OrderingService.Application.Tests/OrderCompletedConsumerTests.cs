using AutoMapper;
using GameStart.OrderingService.Application.Consumers;
using GameStart.OrderingService.Application.Hubs;
using GameStart.OrderingService.Core.Abstractions;
using GameStart.OrderingService.Core.Entities;
using GameStart.Shared;
using GameStart.Shared.MessageBus.Models.OrderModels;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using System.Linq.Expressions;

namespace GameStart.OrderingService.Application.Tests
{
    public class OrderCompletedConsumerTests
    {
        private readonly ConsumeContext<OrderCompleted> contextMock;

        private readonly IOrderRepository orderRepositoryMock;
        private readonly IHubContext<OrderStatusHub> hubContextMock;

        private readonly OrderCompletedConsumer sut;

        public OrderCompletedConsumerTests()
        {
            contextMock = Substitute.For<ConsumeContext<OrderCompleted>>();

            orderRepositoryMock = Substitute.For<IOrderRepository>();
            hubContextMock = Substitute.For<IHubContext<OrderStatusHub>>();

            sut = new OrderCompletedConsumer(orderRepositoryMock, hubContextMock, Substitute.For<IMapper>());
        }

        [Fact]
        public async Task Consume_WhenOrderCompleted_ShouldUpdateOrderEntity()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var completedOrder = new OrderCompleted
            {
                Id = Guid.NewGuid(),
                TotalPrice = 10.00M,
                OrderItems = new List<OrderItem>
                {
                    new() { Id = Guid.NewGuid(), Title = "Product A" },
                    new() { Id = Guid.NewGuid(), Title = "Product B" }
                }
            };

            var order = new Order { Id = completedOrder.Id };

            contextMock.CancellationToken.Returns(cancellationToken);
            contextMock.Message.Returns(completedOrder);

            orderRepositoryMock.GetByConditionAsync(Arg.Any<Expression<Func<Order, bool>>>(), cancellationToken)
                .Returns(new List<Order> { order });

            // Act
            await sut.Consume(contextMock);

            // Assert
            await orderRepositoryMock.Received().UpdateAsync(order, cancellationToken);
        }

        [Fact]
        public async Task Consume_WhenOrderCompleted_ShouldSendOrderToHubGroup()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var completedOrder = new OrderCompleted
            {
                Id = Guid.NewGuid(),
                TotalPrice = 10.00M,
                OrderItems = new List<OrderItem>
                {
                    new() { Id = Guid.NewGuid(), Title = "Product A" },
                    new() { Id = Guid.NewGuid(), Title = "Product B" }
                }
            };

            var order = new Order { Id = completedOrder.Id };

            contextMock.CancellationToken.Returns(cancellationToken);
            contextMock.Message.Returns(completedOrder);

            orderRepositoryMock.GetByConditionAsync(Arg.Any<Expression<Func<Order, bool>>>(), cancellationToken)
                .Returns(new List<Order> { order });

            // Act
            await sut.Consume(contextMock);

            // Assert
            await hubContextMock.Received().Clients
                .Group(Arg.Any<string>()).SendAsync(
                    Constants.OrderingService.HubOptions.OrderStatusMethod,
                    order,
                    cancellationToken);
        }
    }
}
