using AutoMapper;
using GameStart.OrderingService.Application.DtoModels;
using GameStart.Shared.MessageBusModels;

namespace GameStart.OrderingService.Application.Mapping
{
    public class OrderMessageProfile : Profile
    {
        public OrderMessageProfile()
        {
            CreateMap<OrderDto, OrderCreatedMessageModel>()
                .ForMember(message => message.UserId, options => options.MapFrom(dto => dto.UserId)); ;
        }
    }
}
