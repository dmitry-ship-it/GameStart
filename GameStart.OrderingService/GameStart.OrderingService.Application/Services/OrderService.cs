using AutoMapper;
using FluentValidation;
using GameStart.OrderingService.Application.DtoModels;
using GameStart.OrderingService.Application.Hubs;
using GameStart.OrderingService.Core.Abstractions;
using GameStart.OrderingService.Core.Entities;
using GameStart.Shared;
using GameStart.Shared.Extensions;
using GameStart.Shared.MessageBus;
using GameStart.Shared.Services;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace GameStart.OrderingService.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository repository;
        private readonly IMessagePublisher<Order> orderMessagePublisher;
        private readonly IMapper mapper;
        private readonly IValidator<OrderDto> validator;
        private readonly IHubContext<OrderStatusHub> hubContext;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IGuidProvider guidProvider;

        public OrderService(
            IOrderRepository repository,
            IMessagePublisher<Order> orderMessagePublisher,
            IMapper mapper,
            IValidator<OrderDto> validator,
            IHubContext<OrderStatusHub> hubContext,
            IDateTimeProvider dateTimeProvider,
            IGuidProvider guidProvider)
        {
            this.repository = repository;
            this.orderMessagePublisher = orderMessagePublisher;
            this.mapper = mapper;
            this.validator = validator;
            this.hubContext = hubContext;
            this.dateTimeProvider = dateTimeProvider;
            this.guidProvider = guidProvider;
        }

        public async Task<Guid> CreateAsync(OrderDto order, IEnumerable<Claim> claims,
            CancellationToken cancellationToken = default)
        {
            validator.ValidateAndThrow(order);

            var dbOrder = mapper.Map<Order>(order);
            SeedMissingData(dbOrder, claims);

            dbOrder.State = nameof(OrderStates.Submitted);
            await repository.CreateAsync(dbOrder, cancellationToken);

            await hubContext.Clients.Group(dbOrder.Id.ToString())
                .SendAsync(Constants.OrderingService.HubOptions.OrderStatusMethod, dbOrder, cancellationToken);

            await orderMessagePublisher.PublishMessageAsync(dbOrder, cancellationToken);

            return dbOrder.Id;
        }

        public async Task<IEnumerable<Order>> GetByUserIdAsync(
            IEnumerable<Claim> claims,
            CancellationToken cancellationToken = default)
        {
            var userId = claims.GetUserId();

            return await repository.GetByConditionAsync(
                entity => entity.UserId == userId, cancellationToken);
        }

        public async Task<Order> GetByIdAsync(Guid orderId, IEnumerable<Claim> claims,
            CancellationToken cancellationToken = default)
        {
            var userId = claims.GetUserId();

            var orders = await repository.GetByConditionAsync(
                entity => entity.UserId == userId && entity.Id == orderId, cancellationToken);

            return orders.FirstOrDefault();
        }

        private void SeedMissingData(Order order, IEnumerable<Claim> claims)
        {
            order.Id = guidProvider.NewGuid;
            order.DateTime = dateTimeProvider.Now;

            var userId = claims.GetUserId();
            order.UserId = userId;

            if (order.Address is not null)
            {
                order.Address.UserId = userId;
            }
        }
    }
}
