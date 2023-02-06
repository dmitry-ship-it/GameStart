using AutoMapper;
using GameStart.OrderingService.Application.RequestModels;
using GameStart.OrderingService.Core.Entities;

namespace GameStart.OrderingService.Application.Mapping
{
    public class ItemProfile : Profile
    {
        public ItemProfile()
        {
            CreateMap<ItemModel, Item>();
        }
    }
}
