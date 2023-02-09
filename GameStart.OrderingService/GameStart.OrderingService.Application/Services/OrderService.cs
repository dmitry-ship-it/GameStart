using AutoMapper;
using FluentValidation;
using GameStart.OrderingService.Application.DtoModels;
using GameStart.OrderingService.Core.Abstractions;
using GameStart.OrderingService.Core.Entities;

namespace GameStart.OrderingService.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository repository;
        private readonly IOrderMessagePublisher messagePublisher;
        private readonly IMapper mapper;
        private readonly IValidator<OrderDto> validator;

        public OrderService(
            IOrderRepository repository,
            IOrderMessagePublisher messagePublisher,
            IMapper mapper,
            IValidator<OrderDto> validator)
        {
            this.repository = repository;
            this.messagePublisher = messagePublisher;
            this.mapper = mapper;
            this.validator = validator;
        }

        public async Task CreateAsync(OrderDto order, CancellationToken cancellationToken = default)
        {
            validator.ValidateAndThrow(order);
            await repository.CreateAsync(mapper.Map<Order>(order), cancellationToken);
            await messagePublisher.PublishMessageAsync(order, cancellationToken);
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

        public async Task<IEnumerable<Order>> GetByUserIdAsync(Guid userId,
            CancellationToken cancellationToken = default)
        {
            return await repository.GetByConditionAsync(
                entity => entity.UserId == userId, cancellationToken);
        }
    }
}
