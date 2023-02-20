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

        public async Task CreateAsync(AddressDto address, CancellationToken cancellationToken = default)
        {
            validator.ValidateAndThrow(address);
            await repository.CreateAsync(mapper.Map<Address>(address), cancellationToken);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var addresses = await repository.GetByConditionAsync(
                entity => entity.Id == id, cancellationToken);

            if (addresses.Any())
            {
                await repository.DeleteAsync(addresses.First(), cancellationToken);
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<Address>> GetByUserIdAsync(IEnumerable<Claim> claims,
            CancellationToken cancellationToken = default)
        {
            var userId = claims.GetUserId();

            return await repository.GetByConditionAsync(
                entity => entity.UserId == userId, cancellationToken);
        }

        public async Task<bool> UpdateAsync(Guid id, AddressDto address,
            CancellationToken cancellationToken = default)
        {
            validator.ValidateAndThrow(address);

            var dbAddresses = await repository.GetByConditionAsync(
                entity => entity.Id == id, cancellationToken);

            if (dbAddresses?.Any() != true)
            {
                return false;
            }

            var updatedAddress = mapper.Map(address, dbAddresses.First());
            await repository.UpdateAsync(updatedAddress, cancellationToken);

            return true;
        }
    }
}
