using AutoMapper;
using GameStart.OrderingService.Core.Entities;
using GameStart.Shared.MessageBus.Models.OrderModels;

namespace GameStart.OrderingService.Application.Mapping
{
    public class OrderToMessageConverter : ITypeConverter<Order, OrderSubmitted>
    {
        public OrderSubmitted Convert(Order source, OrderSubmitted destination, ResolutionContext context)
        {
            destination ??= new();

            destination.Id = source.Id;
            destination.UserId = source.UserId;

            var orderItems = new List<OrderItem>();
            foreach (var item in source.Items)
            {
                orderItems.Add(new OrderItem
                {
                    GameId = item.GameId,
                    Title = item.Title,
                    PurchaseDateTime = source.DateTime
                });
            }

            destination.OrderItems = orderItems;

            return destination;
        }
    }
}
