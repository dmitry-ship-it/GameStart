using AutoMapper;
using GameStart.OrderingService.Core.Entities;
using GameStart.Shared.MessageBus;
using GameStart.Shared.MessageBus.Models;
using MassTransit;

namespace GameStart.OrderingService.Application.Services
{
    public class OrderCreatedPublisher : IMessagePublisher<Order>
    {
        private readonly IPublishEndpoint publishEndpoint;
        private readonly IMapper mapper;
        private readonly IGameKeyGeneratorService gameKeyGenerator;

        public OrderCreatedPublisher(IPublishEndpoint publishEndpoint, IMapper mapper, IGameKeyGeneratorService gameKeyGenerator)
        {
            this.publishEndpoint = publishEndpoint;
            this.mapper = mapper;
            this.gameKeyGenerator = gameKeyGenerator;
        }

        public async Task PublishMessageAsync(Order order, CancellationToken cancellationToken = default)
        {
            var message = mapper.Map<OrderSubmitted>(order);

            foreach (var item in message.OrderItems)
            {
                item.GameKey = gameKeyGenerator.Generate();
            }

            await publishEndpoint.Publish(message, cancellationToken);
        }
    }
}
