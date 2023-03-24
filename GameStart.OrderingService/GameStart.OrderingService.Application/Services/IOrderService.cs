using GameStart.OrderingService.Application.DtoModels;
using GameStart.OrderingService.Core.Entities;
using System.Security.Claims;

namespace GameStart.OrderingService.Application.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetByUserIdAsync(IEnumerable<Claim> claims,
            CancellationToken cancellationToken = default);

        Task<Order> GetByIdAsync(Guid orderId, IEnumerable<Claim> claims,
            CancellationToken cancellationToken = default);

        Task<Guid> CreateAsync(OrderDto order, IEnumerable<Claim> claims,
            CancellationToken cancellationToken = default);
    }
}
