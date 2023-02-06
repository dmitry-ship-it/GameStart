using GameStart.OrderingService.Application.DtoModels;
using GameStart.OrderingService.Core.Entities;

namespace GameStart.OrderingService.Application.Services
{
    public interface IAddressService
    {
        Task<IEnumerable<Address>> GetByUserIdAsync(Guid userId);

        Task CreateAsync(AddressDto address);

        Task<bool> UpdateAsync(Guid id, AddressDto address);

        Task<bool> DeleteAsync(Guid id);
    }
}
