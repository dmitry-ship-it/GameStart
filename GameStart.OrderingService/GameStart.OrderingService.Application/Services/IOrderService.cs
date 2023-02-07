using GameStart.OrderingService.Application.DtoModels;
using GameStart.OrderingService.Core.Entities;

namespace GameStart.OrderingService.Application.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetByUserIdAsync(Guid userId,
            CancellationToken cancellationToken = default);

        Task CreateAsync(OrderDto order,
            CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(Guid id,
            CancellationToken cancellationToken = default);
    }
}
