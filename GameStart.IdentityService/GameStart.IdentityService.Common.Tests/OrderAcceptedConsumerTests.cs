using AutoMapper;
using GameStart.IdentityService.Common.Consumers;
using GameStart.IdentityService.Data.Models;
using GameStart.IdentityService.Data.Repositories;
using GameStart.Shared.MessageBus.Models.OrderModels;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;

namespace GameStart.IdentityService.Common.Tests
{
    public class OrderAcceptedConsumerTests
    {
        private readonly UserManager<User> userManager;
        private readonly IRepository<InventoryItem> inventoryRepository;
        private readonly IMapper mapper;
        private readonly ConsumeContext<OrderAccepted> context;
        private readonly OrderAcceptedConsumer sut;

        public OrderAcceptedConsumerTests()
        {
            userManager = Substitute.For<UserManager<User>>(
                Substitute.For<IUserStore<User>>(),
                null, null, null, null, null, null, null, null);

            inventoryRepository = Substitute.For<IRepository<InventoryItem>>();
            mapper = Substitute.For<IMapper>();

            context = Substitute.For<ConsumeContext<OrderAccepted>>();
            context.CancellationToken.Returns(CancellationToken.None);

            sut = new OrderAcceptedConsumer(userManager, inventoryRepository, mapper);
        }

        [Fact]
        public async Task Consume_WhenUserDoesNotExist_ShouldPublishOrderFaulted()
        {
            // Arrange
            var messageId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            context.Message.Returns(new OrderAccepted
            {
                Id = messageId,
                UserId = userId,
                OrderItems = Array.Empty<OrderItem>()
            });

            userManager.FindByIdAsync(userId.ToString()).Returns((User)null!);

            // Act
            await sut.Consume(context);

            // Assert
            await context.Received().Publish<OrderFaulted>(new
            {
                Id = messageId,
                UserId = userId
            }, context.CancellationToken);
        }

        [Fact]
        public async Task Consume_WhenUserAlreadyOwnsAnyOfDigitalCopies_ShouldPublishOrderFaulted()
        {
            // Arrange
            var messageId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var gameId = Guid.NewGuid();

            var user = new User { Id = userId };
            var orderItem = new OrderItem { GameId = gameId };
            var inventoryItem = new InventoryItem { GameId = gameId, User = user };

            context.Message.Returns(new OrderAccepted
            {
                Id = messageId,
                UserId = userId,
                OrderItems = new[] { orderItem }
            });

            userManager.FindByIdAsync(userId.ToString()).Returns(user);
            inventoryRepository.FindByConditionAsync(Arg.Any<Expression<Func<InventoryItem, bool>>>(), context.CancellationToken)
                .Returns(new[] { inventoryItem });

            // Act
            await sut.Consume(context);

            // Assert
            await context.Received().Publish<OrderFaulted>(new
            {
                Id = messageId,
                UserId = userId
            }, context.CancellationToken);
        }

        [Fact]
        public async Task Consume_WhenItsContainsInOrderAndOrderIsValid_ShouldNotAddOnlyDigitalCopiesToInventoryButPublishAllOfThem()
        {
            // Arrange
            var messageId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var gameId = Guid.NewGuid();
            const decimal totalPrice = 59.99M;

            var user = new User { Id = userId };
            var orderItems = new OrderItem[]
            {
                new() { GameId = gameId, IsPhysicalCopy = false },
                new() { GameId = Guid.NewGuid(), IsPhysicalCopy = true }
            };
            var inventoryItem = new InventoryItem { GameId = gameId, User = user };

            context.Message.Returns(new OrderAccepted
            {
                Id = messageId,
                UserId = userId,
                OrderItems = orderItems,
                TotalPrice = totalPrice
            });

            userManager.FindByIdAsync(userId.ToString()).Returns(user);
            inventoryRepository.FindByConditionAsync(Arg.Any<Expression<Func<InventoryItem, bool>>>(), context.CancellationToken).Returns(Array.Empty<InventoryItem>());
            mapper.Map<IEnumerable<InventoryItem>>(Arg.Any<IEnumerable<OrderItem>>()).Returns(new[] { inventoryItem });

            // Act
            await sut.Consume(context);

            // Assert
            await inventoryRepository.Received().CreateRangeAsync(Arg.Is<IEnumerable<InventoryItem>>(items =>
                items.Count() == 1 &&
                items.Single().GameId == gameId &&
                items.Single().User == user), context.CancellationToken);
            await context.Received().Publish<OrderCompleted>(new
            {
                Id = messageId,
                UserId = userId,
                TotalPrice = totalPrice,
                OrderItems = orderItems,
            }, context.CancellationToken);
        }
    }
}
