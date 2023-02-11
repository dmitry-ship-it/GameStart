using GameStart.OrderingService.Core.Abstractions;
using GameStart.Shared;
using GameStart.Shared.MessageBus.Models;
using MassTransit;

namespace GameStart.OrderingService.Application.Consumers
{
    public class OrderFaultedConsumer : IConsumer<OrderFaulted>
    {
        private readonly IOrderRepository repository;

        public OrderFaultedConsumer(IOrderRepository repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<OrderFaulted> context)
        {
            var message = context.Message;

            var order = (await repository.GetByConditionAsync(
                entity => entity.Id == message.Id, context.CancellationToken)).First();

            order.State = nameof(OrderStates.Faulted);
            await repository.UpdateAsync(order, context.CancellationToken);
        }
    }
}
