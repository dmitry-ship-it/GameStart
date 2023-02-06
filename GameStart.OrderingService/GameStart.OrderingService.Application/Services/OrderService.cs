using AutoMapper;
using FluentValidation;
using GameStart.OrderingService.Application.DtoModels;
using GameStart.OrderingService.Core.Abstractions;
using GameStart.OrderingService.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace GameStart.OrderingService.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository repository;
        private readonly IMapper mapper;
        private readonly IValidator<OrderDto> validator;

        public OrderService(IOrderRepository repository, IMapper mapper, IValidator<OrderDto> validator)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.validator = validator;
        }

        public async Task CreateAsync(OrderDto order)
        {
            validator.ValidateAndThrow(order);
            await repository.CreateAsync(mapper.Map<Order>(order));
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var orders = await repository.GetByConditionAsync(entity => entity.Id == id);

            if (orders.Any())
            {
                await repository.DeleteAsync(orders.First());
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<Order>> GetByUserIdAsync(Guid userId)
        {
            return await repository.GetByConditionAsync(entity => entity.UserId == userId);
        }
    }
}
