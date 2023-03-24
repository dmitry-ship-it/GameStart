using GameStart.OrderingService.Application.Hubs;
using GameStart.Shared;

namespace GameStart.OrderingService.Api.Extensions
{
    public static class EndpointRouteBuilderExtension
    {
        public static IEndpointRouteBuilder MapOrderStatusHub(this IEndpointRouteBuilder builder)
        {
            builder.MapHub<OrderStatusHub>(Constants.OrderingService.HubOptions.Route);

            return builder;
        }
    }
}
