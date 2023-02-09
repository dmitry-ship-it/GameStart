using GameStart.OrderingService.Application.DtoModels;

namespace GameStart.OrderingService.Application.Services
{
    public interface IOrderMessagePublisher
    {
        Task PublishMessageAsync(OrderDto orderDto, CancellationToken cancellationToken = default);
    }
}
