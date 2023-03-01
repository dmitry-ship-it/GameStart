using AutoMapper;
using GameStart.IdentityService.Data.Models;
using GameStart.Shared.MessageBus.Models.OrderModels;

namespace GameStart.IdentityService.Common.Mapping
{
    public class InventoryItemProfile : Profile
    {
        public InventoryItemProfile()
        {
            CreateMap<OrderItem, InventoryItem>();
        }
    }
}
