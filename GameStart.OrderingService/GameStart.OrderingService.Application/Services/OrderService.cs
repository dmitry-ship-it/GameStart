using AutoMapper;
using GameStart.OrderingService.Application.DtoModels;
using GameStart.OrderingService.Core.Abstractions;
using GameStart.OrderingService.Core.Entities;

namespace GameStart.OrderingService.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository repository;
        private readonly IMapper mapper;

        public OrderService(IOrderRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task CreateAsync(OrderDto order)
        {
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
