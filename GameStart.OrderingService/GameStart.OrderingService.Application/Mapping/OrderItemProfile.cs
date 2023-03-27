using AutoMapper;
using GameStart.OrderingService.Core.Entities;
using GameStart.Shared.MessageBus.Models.OrderModels;

namespace GameStart.OrderingService.Application.Mapping
{
    public class OrderItemProfile : Profile
    {
        public OrderItemProfile()
        {
            CreateMap<Item, OrderItem>().ReverseMap();
        }
    }
}
