using AutoMapper;
using GameStart.OrderingService.Application.DtoModels;
using GameStart.OrderingService.Core.Entities;

namespace GameStart.OrderingService.Application.Mapping
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<AddressDto, Address>();
        }
    }
}
