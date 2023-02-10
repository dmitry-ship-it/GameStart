using AutoMapper;
using GameStart.OrderingService.Core.Entities;
using GameStart.Shared.MessageBus.Models;

namespace GameStart.OrderingService.Application.Mapping
{
    public class OrderToMessageConverter : ITypeConverter<Order, OrderCreated>
    {
        public OrderCreated Convert(Order source, OrderCreated destination, ResolutionContext context)
        {
            destination ??= new();

            destination.UserId = source.UserId;

            var orderItems = new List<OrderItem>();
            foreach (var item in source.Items)
            {
                orderItems.Add(new OrderItem
                {
                    GameId = item.GameId,
                    PurchaseDateTime = source.DateTime
                });
            }

            destination.OrderItems = orderItems;

            return destination;
        }
    }
}