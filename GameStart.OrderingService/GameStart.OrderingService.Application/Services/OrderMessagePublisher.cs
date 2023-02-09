using AutoMapper;
using GameStart.OrderingService.Application.DtoModels;
using GameStart.Shared.MessageBusModels;
using MassTransit;
using System.Security.Cryptography;
using System.Text;

namespace GameStart.OrderingService.Application.Services
{
    public class OrderMessagePublisher : IOrderMessagePublisher
    {
        private readonly IPublishEndpoint publishEndpoint;
        private readonly IMapper mapper;

        public OrderMessagePublisher(IPublishEndpoint publishEndpoint, IMapper mapper)
        {
            this.publishEndpoint = publishEndpoint;
            this.mapper = mapper;
        }

        public async Task PublishMessageAsync(OrderDto orderDto, CancellationToken cancellationToken = default)
        {
            var message = mapper.Map<OrderCreatedMessageModel>(orderDto);

            GenerateGameKeys(message.OrderItems);

            await publishEndpoint.Publish(orderDto, cancellationToken);
        }

        private void SetGameTitles(ICollection<OrderItemMessageModel> orderItems)
        {
            throw new NotImplementedException();
        }

        private static void GenerateGameKeys(ICollection<OrderItemMessageModel> orderItems)
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
                result[i] = charPool[RandomNumberGenerator.GetInt32(charPool.Length + 1)];
            }

            return new string(result);
        }
    }
}
