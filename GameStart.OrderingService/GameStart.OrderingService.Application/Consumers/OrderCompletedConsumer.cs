using GameStart.OrderingService.Core.Abstractions;
using GameStart.Shared;
using GameStart.Shared.MessageBus.Models.OrderModels;
using MassTransit;

namespace GameStart.OrderingService.Application.Consumers
{
    public class OrderCompletedConsumer : IConsumer<OrderCompleted>
    {
        private readonly IOrderRepository repository;

        public OrderCompletedConsumer(IOrderRepository repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<OrderCompleted> context)
        {
            var message = context.Message;

            var order = (await repository.GetByConditionAsync(order => order.Id == message.Id)).First();

            order.State = nameof(OrderStates.Completed);
            order.TotalPrice = message.TotalPrice;
            await repository.UpdateAsync(order, context.CancellationToken);
        }
    }
}
