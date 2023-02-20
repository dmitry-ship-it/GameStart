using GameStart.OrderingService.Application.DtoModels;
using GameStart.OrderingService.Core.Entities;
using System.Security.Claims;

namespace GameStart.OrderingService.Application.Services
{
    public interface IAddressService
    {
        Task<IEnumerable<Address>> GetByUserIdAsync(IEnumerable<Claim> claims,
            CancellationToken cancellationToken = default);

        Task CreateAsync(AddressDto address,
            CancellationToken cancellationToken = default);

        Task<bool> UpdateAsync(Guid id, AddressDto address,
            CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(Guid id,
            CancellationToken cancellationToken = default);
    }
}
