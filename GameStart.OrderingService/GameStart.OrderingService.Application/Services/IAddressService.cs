using GameStart.OrderingService.Application.DtoModels;
using GameStart.OrderingService.Core.Entities;

namespace GameStart.OrderingService.Application.Services
{
    public interface IAddressService
    {
        Task<IEnumerable<Address>> GetByUserIdAsync(Guid userId,
            CancellationToken cancellationToken = default);

        Task CreateAsync(AddressDto address,
            CancellationToken cancellationToken = default);

        Task<bool> UpdateAsync(Guid id, AddressDto address,
            CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(Guid id,
            CancellationToken cancellationToken = default);
    }
}
