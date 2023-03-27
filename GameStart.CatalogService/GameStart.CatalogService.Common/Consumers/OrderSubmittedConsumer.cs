using GameStart.CatalogService.Data.Repositories;
using GameStart.Shared.MessageBus.Models.OrderModels;
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

            if (entities.Any() && message.OrderItems.Count(item => entities
                .Any(entity => entity.Id == item.GameId)) == message.OrderItems.Count)
            {
                var totalPrice = (from entity in entities
                                  join item in message.OrderItems
                                  on entity.Id equals item.GameId
                                  select entity.Price).Sum();

                await context.Publish(new OrderAccepted
                {
                    Id = message.Id,
                    UserId = message.UserId,
                    TotalPrice = totalPrice,
                    OrderItems = message.OrderItems.Select(item =>
                    {
                        item.Title = entities.First(entity => entity.Id == item.GameId).Title;

                        return item;
                    }).ToArray()
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
