using AutoMapper;
using FluentValidation;
using GameStart.OrderingService.Application.DtoModels;
using GameStart.OrderingService.Application.Hubs;
using GameStart.OrderingService.Application.Services;
using GameStart.OrderingService.Core.Abstractions;
using GameStart.OrderingService.Core.Entities;
using GameStart.Shared;
using GameStart.Shared.MessageBus;
using GameStart.Shared.Services;
using MassTransit.Transports;
using Microsoft.AspNetCore.SignalR;
using System.Linq.Expressions;
using System.Security.Claims;

namespace GameStart.OrderingService.Application.Tests
{
    public class OrderServiceTests
    {
        private readonly IEnumerable<Claim> claims;

        private readonly IOrderRepository repositoryMock;
        private readonly IMessagePublisher<Order> orderMessagePublisherMock;
        private readonly IMapper mapperMock;
        private readonly IValidator<OrderDto> validatorMock;
        private readonly IHubContext<OrderStatusHub> hubContextMock;

        private readonly OrderService sut;

        public OrderServiceTests()
        {
            claims = Enumerable.Repeat(
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()), 1);

            repositoryMock = Substitute.For<IOrderRepository>();
            orderMessagePublisherMock = Substitute.For<IMessagePublisher<Order>>();
            mapperMock = Substitute.For<IMapper>();
            validatorMock = Substitute.For<IValidator<OrderDto>>();
            hubContextMock = Substitute.For<IHubContext<OrderStatusHub>>();

            sut = new OrderService(
                repositoryMock,
                orderMessagePublisherMock,
                mapperMock,
                validatorMock,
                hubContextMock,
                new DateTimeProvider(),
                new GuidProvider());
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateOrderEntityAndPublishIt()
        {
            // Arrange
            var orderDto = new OrderDto
            {
                Address = new() { Country = "Country A" },
                Items = new ItemDto[] { new() { GameId = Guid.NewGuid() } }
            };

            var cancellationToken = CancellationToken.None;

            var mappedOrder = new Order
            {
                Address = new Address { Country = orderDto.Address.Country },
                Items = orderDto.Items.Select(item => new Item { GameId = item.GameId }).ToArray()
            };

            mapperMock.Map<Order>(orderDto).Returns(mappedOrder);

            // Act
            await sut.CreateAsync(orderDto, claims, cancellationToken);

            // Assert
            await repositoryMock.Received().CreateAsync(Arg.Is<Order>(order =>
                order.Address.Country == orderDto.Address.Country &&
                order.Items.Single().GameId == orderDto.Items.Single().GameId), cancellationToken);
            await hubContextMock.Received().Clients
                .Group(mappedOrder.Id.ToString())
                .SendCoreAsync(
                    Constants.OrderingService.HubOptions.OrderStatusMethod,
                    Arg.Any<object?[]>(),
                    cancellationToken);
            await orderMessagePublisherMock.Received()
                .PublishMessageAsync(Arg.Any<Order>(), cancellationToken);
        }

        [Fact]
        public async Task GetByUserIdAsync_ShouldCallRepository()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;

            // Act
            await sut.GetByUserIdAsync(claims, cancellationToken);

            // Assert
            await repositoryMock.Received().GetByConditionAsync(Arg.Any<Expression<Func<Order, bool>>>(), cancellationToken);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldCallRepository()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;

            // Act
            await sut.GetByIdAsync(orderId, claims, cancellationToken);

            // Assert
            await repositoryMock.Received().GetByConditionAsync(Arg.Any<Expression<Func<Order, bool>>>(), cancellationToken);
        }
    }
}
