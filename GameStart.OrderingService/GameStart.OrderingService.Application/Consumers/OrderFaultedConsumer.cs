using GameStart.OrderingService.Application.Hubs;
using GameStart.OrderingService.Core.Abstractions;
using GameStart.Shared;
using GameStart.Shared.MessageBus.Models.OrderModels;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace GameStart.OrderingService.Application.Consumers
{
    public class OrderFaultedConsumer : IConsumer<OrderFaulted>
    {
        private readonly IOrderRepository repository;
        private readonly IHubContext<OrderStatusHub> hubContext;

        public OrderFaultedConsumer(IOrderRepository repository, IHubContext<OrderStatusHub> hubContext)
        {
            this.repository = repository;
            this.hubContext = hubContext;
        }

        public async Task Consume(ConsumeContext<OrderFaulted> context)
        {
            var message = context.Message;

            var order = (await repository.GetByConditionAsync(
                entity => entity.Id == message.Id, context.CancellationToken)).First();

            order.State = nameof(OrderStates.Faulted);
            await repository.UpdateAsync(order, context.CancellationToken);

            await hubContext.Clients.Group(order.Id.ToString())
                .SendAsync(Constants.OrderingService.HubOptions.OrderStatusMethod, order, context.CancellationToken);
        }
    }
}
