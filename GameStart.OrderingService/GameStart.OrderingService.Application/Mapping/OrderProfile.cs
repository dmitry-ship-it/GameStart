using AutoMapper;
using GameStart.OrderingService.Application.RequestModels;
using GameStart.OrderingService.Core.Abstractions;
using GameStart.OrderingService.Core.Entities;

namespace GameStart.OrderingService.Application.Mapping
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<CreateOrderRequest, Order>();
        }
    }
}
