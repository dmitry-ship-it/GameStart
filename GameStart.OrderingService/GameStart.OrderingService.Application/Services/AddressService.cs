using AutoMapper;
using FluentValidation;
using GameStart.OrderingService.Application.DtoModels;
using GameStart.OrderingService.Core.Abstractions;
using GameStart.OrderingService.Core.Entities;
using GameStart.Shared.Extensions;
using System.Security.Claims;

namespace GameStart.OrderingService.Application.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository repository;
        private readonly IMapper mapper;
        private readonly IValidator<AddressDto> validator;

        public AddressService(
            IAddressRepository repository,
            IMapper mapper,
            IValidator<AddressDto> validator)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.validator = validator;
        }

        public async Task CreateAsync(AddressDto address, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
        {
            validator.ValidateAndThrow(address);

            var entity = mapper.Map<Address>(address);
            entity.UserId = claims.GetUserId();

            await repository.CreateAsync(entity, cancellationToken);
        }

        public async Task<bool> DeleteAsync(Guid id, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
        {
            var addresses = await repository.GetByConditionAsync(
                entity => entity.Id == id, cancellationToken);

            if (!addresses.Any())
            {
                return false;
            }

            var address = addresses.First();

            if (address.UserId != claims.GetUserId())
            {
                // TODO: Move to constants
                throw new ArgumentException("User cannot delete other user's address");
            }

            await repository.DeleteAsync(address, cancellationToken);

            return true;
        }

        public async Task<IEnumerable<Address>> GetByUserIdAsync(IEnumerable<Claim> claims,
            CancellationToken cancellationToken = default)
        {
            var userId = claims.GetUserId();

            return await repository.GetByConditionAsync(
                entity => entity.UserId == userId, cancellationToken);
        }

        public async Task<bool> UpdateAsync(Guid id, IEnumerable<Claim> claims, AddressDto address,
            CancellationToken cancellationToken = default)
        {
            validator.ValidateAndThrow(address);

            var dbAddresses = await repository.GetByConditionAsync(
                entity => entity.Id == id, cancellationToken);

            if (!dbAddresses.Any())
            {
                return false;
            }

            var dbAddress = dbAddresses.First();

            if (dbAddress.UserId != claims.GetUserId())
            {
                // TODO: Move to constants
                throw new ArgumentException("User cannot update other user's address");
            }

            var updatedAddress = mapper.Map(address, dbAddress);
            await repository.UpdateAsync(updatedAddress, cancellationToken);

            return true;
        }
    }
}
