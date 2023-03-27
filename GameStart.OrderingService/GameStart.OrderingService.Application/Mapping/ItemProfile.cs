using AutoMapper;
using GameStart.OrderingService.Application.DtoModels;
using GameStart.OrderingService.Core.Entities;

namespace GameStart.OrderingService.Application.Mapping
{
    public class ItemProfile : Profile
    {
        public ItemProfile()
        {
            CreateMap<ItemDto, Item>().ReverseMap();
        }
    }
}
