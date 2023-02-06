using GameStart.OrderingService.Application.DtoModels;
using GameStart.OrderingService.Core.Entities;

namespace GameStart.OrderingService.Application.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetByUserIdAsync(Guid userId);

        Task CreateAsync(OrderDto order);

        Task<bool> DeleteAsync(Guid id);
    }
}
