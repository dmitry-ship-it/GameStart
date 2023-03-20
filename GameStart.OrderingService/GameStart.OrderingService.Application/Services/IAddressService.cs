using GameStart.OrderingService.Application.DtoModels;
using GameStart.OrderingService.Core.Entities;
using System.Security.Claims;

namespace GameStart.OrderingService.Application.Services
{
    public interface IAddressService
    {
        Task<IEnumerable<Address>> GetByUserIdAsync(IEnumerable<Claim> claims,
            CancellationToken cancellationToken = default);

        Task CreateAsync(AddressDto address, IEnumerable<Claim> claims,
            CancellationToken cancellationToken = default);

        Task<bool> UpdateAsync(Guid id, IEnumerable<Claim> claims, AddressDto address,
            CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(Guid id, IEnumerable<Claim> claims,
            CancellationToken cancellationToken = default);
    }
}
