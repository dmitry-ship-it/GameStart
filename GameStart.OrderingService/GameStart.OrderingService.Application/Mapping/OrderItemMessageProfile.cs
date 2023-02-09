using AutoMapper;
using GameStart.OrderingService.Application.DtoModels;
using GameStart.Shared.MessageBusModels;

namespace GameStart.OrderingService.Application.Mapping
{
    public class OrderItemMessageProfile : Profile
    {
        public OrderItemMessageProfile()
        {
            CreateMap<OrderDto, OrderItemMessageModel>()
                .ForMember(message => message.PurchaseDateTime, options => options.MapFrom(dto => dto.DateTime));
            
            CreateMap<ItemDto, OrderItemMessageModel>()
                .ForMember(message => message.GameId, options => options.MapFrom(item => item.GameId));
        }
    }
}
