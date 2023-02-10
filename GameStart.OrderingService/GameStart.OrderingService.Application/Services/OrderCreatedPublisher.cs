using AutoMapper;
using GameStart.OrderingService.Core.Entities;
using GameStart.Shared.MessageBus;
using GameStart.Shared.MessageBus.Models;
using MassTransit;
using System.Security.Cryptography;
using System.Text;

namespace GameStart.OrderingService.Application.Services
{
    public class OrderCreatedPublisher : IMessagePublisher<Order>
    {
        private readonly IBus bus;
        private readonly IMapper mapper;

        public OrderCreatedPublisher(IBus bus, IMapper mapper)
        {
            this.bus = bus;
            this.mapper = mapper;
        }

        public async Task PublishMessageAsync(Order order, CancellationToken cancellationToken = default)
        {
            var message = mapper.Map<OrderCreated>(order);

            GenerateGameKeys(message.OrderItems);

            // TODO: MOVE TO CONSTANTS
            var endpoint = await bus.GetSendEndpoint(new Uri($"rabbitmq://messagebus/{nameof(OrderCreated)}"));
            
            await endpoint.Send(message , cancellationToken);
        }

        private static void GenerateGameKeys(ICollection<OrderItem> orderItems)
        {
            foreach (var item in orderItems)
            {
                item.GameKey = GetRandomKey();
            }
        }

        private static string GetRandomKey()
        {
            const int blocks = 3;
            const int blockSize = 5;

            const char separator = '-';
            const int separators = blocks - 1;

            var keyBase = GetRandomString(blocks * blockSize).AsSpan();

            var builder = new StringBuilder();

            var pointer = 0;
            for (var i = 0; i < separators; i++)
            {
                builder.Append(keyBase.Slice(pointer, blockSize));
                builder.Append(separator);
                pointer += blockSize;
            }

            builder.Append(keyBase[pointer..]);

            return builder.ToString();
        }

        private static string GetRandomString(int size)
        {
            const string charPool =
                "abcdefghijklmnopqrstuvwxyz" +
                "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                "0123456789";

            var result = new char[size];

            for (var i = 0; i < size; i++)
            {
                result[i] = charPool[RandomNumberGenerator.GetInt32(charPool.Length)];
            }

            return new string(result);
        }
    }
}
