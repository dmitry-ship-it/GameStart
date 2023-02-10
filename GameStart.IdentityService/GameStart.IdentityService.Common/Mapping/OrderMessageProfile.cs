using AutoMapper;
using GameStart.IdentityService.Data.Models;
using GameStart.Shared.MessageBus.Models;

namespace GameStart.IdentityService.Common.Mapping
{
    public class InventoryItemProfile : Profile
    {
        public InventoryItemProfile()
        {
            CreateMap<OrderCreated, User>();
            CreateMap<OrderItem, InventoryItem>();
        }
    }
}
