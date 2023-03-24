using GameStart.OrderingService.Application.Services;
using GameStart.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace GameStart.OrderingService.Application.Hubs
{
    public class OrderStatusHub : Hub
    {
        private readonly IOrderService orderService;

        public OrderStatusHub(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        public async Task SubscribeToOrder(string orderId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, orderId);
        }

        public async Task UnsubscribeFromOrder(string orderId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, orderId);
        }
    }
}
