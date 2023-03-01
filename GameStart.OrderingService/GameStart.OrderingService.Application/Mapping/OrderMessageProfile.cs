using AutoMapper;
using GameStart.OrderingService.Core.Entities;
using GameStart.Shared.MessageBus.Models.OrderModels;

namespace GameStart.OrderingService.Application.Mapping
{
    public class OrderMessageProfile : Profile
    {
        public OrderMessageProfile()
        {
            CreateMap<Order, OrderSubmitted>().ConvertUsing<OrderToMessageConverter>();
        }
    }
}
