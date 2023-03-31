using Microsoft.AspNetCore.SignalR;

namespace GameStart.OrderingService.Application.Hubs
{
    public class OrderStatusHub : Hub
    {
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
