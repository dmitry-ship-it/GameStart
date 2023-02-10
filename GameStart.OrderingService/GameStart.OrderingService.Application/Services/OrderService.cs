﻿using AutoMapper;
using FluentValidation;
using GameStart.OrderingService.Application.DtoModels;
using GameStart.OrderingService.Core.Abstractions;
using GameStart.OrderingService.Core.Entities;
using GameStart.Shared.Extensions;
using GameStart.Shared.MessageBus;
using System.Security.Claims;

namespace GameStart.OrderingService.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository repository;
        private readonly IMessagePublisher<Order> orderMessagePublisher;
        private readonly IMapper mapper;
        private readonly IValidator<OrderDto> validator;

        public OrderService(
            IOrderRepository repository,
            IMessagePublisher<Order> orderMessagePublisher,
            IMapper mapper,
            IValidator<OrderDto> validator)
        {
            this.repository = repository;
            this.orderMessagePublisher = orderMessagePublisher;
            this.mapper = mapper;
            this.validator = validator;
        }

        public async Task CreateAsync(OrderDto order, IEnumerable<Claim> claims,
            CancellationToken cancellationToken = default)
        {
            validator.ValidateAndThrow(order);

            var dbOrder = mapper.Map<Order>(order);
            SeedMessingData(dbOrder, claims);

            await orderMessagePublisher.PublishMessageAsync(dbOrder, cancellationToken);

            await repository.CreateAsync(dbOrder, cancellationToken);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var orders = await repository.GetByConditionAsync(
                entity => entity.Id == id, cancellationToken);

            if (orders.Any())
            {
                await repository.DeleteAsync(orders.First(), cancellationToken);
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<Order>> GetByUserIdAsync(
            IEnumerable<Claim> claims,
            CancellationToken cancellationToken = default)
        {
            var userId = claims.GetUserId();

            return await repository.GetByConditionAsync(
                entity => entity.UserId == userId, cancellationToken);
        }

        private static void SeedMessingData(Order order, IEnumerable<Claim> claims)
        {
            order.DateTime = DateTime.Now;

            var userId = claims.GetUserId();
            order.UserId = userId;

            if (order.Address is not null)
            {
                order.Address.UserId = userId;
            }
        }
    }
}
