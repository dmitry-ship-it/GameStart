using GameStart.CatalogService.Data.Repositories;
using GameStart.Shared.MessageBus.Models;
using MassTransit;

namespace GameStart.CatalogService.Common.Consumers
{
    public class OrderSubmittedConsumer : IConsumer<OrderSubmitted>
    {
        private readonly IRepositoryWrapper repository;

        public OrderSubmittedConsumer(IRepositoryWrapper repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<OrderSubmitted> context)
        {
            var message = context.Message;

            var entities = await repository.VideoGames.FindAllAsync(
                includeGraph: false, context.CancellationToken);

            if (entities.Any() && entities.All(entity => message.OrderItems.All(item => item.GameId == entity.Id)))
            {
                var totalPrice = (from entity in entities
                                  join item in message.OrderItems
                                  on entity.Id equals item.GameId
                                  select entity.Price).Sum();

                await context.Publish<OrderAccepted>(new
                {
                    message.Id,
                    message.UserId,
                    TotalPrice = totalPrice,
                    message.OrderItems
                }, context.CancellationToken);
            }
            else
            {
                await context.Publish<OrderFaulted>(new
                {
                    message.Id,
                    message.UserId
                }, context.CancellationToken);
            }
        }
    }
}
