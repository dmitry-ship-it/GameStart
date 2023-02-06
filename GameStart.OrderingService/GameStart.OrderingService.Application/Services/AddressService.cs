using AutoMapper;
using FluentValidation;
using GameStart.OrderingService.Application.DtoModels;
using GameStart.OrderingService.Core.Abstractions;
using GameStart.OrderingService.Core.Entities;

namespace GameStart.OrderingService.Application.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository repository;
        private readonly IMapper mapper;
        private readonly IValidator<AddressDto> validator;

        public AddressService(IAddressRepository repository, IMapper mapper, IValidator<AddressDto> validator)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.validator = validator;
        }

        public async Task CreateAsync(AddressDto address)
        {
            validator.ValidateAndThrow(address);
            await repository.CreateAsync(mapper.Map<Address>(address));
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var addresses = await repository.GetByConditionAsync(entity => entity.Id == id);

            if (addresses.Any())
            {
                await repository.DeleteAsync(addresses.First());
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<Address>> GetByUserIdAsync(Guid userId)
        {
            return await repository.GetByConditionAsync(entity => entity.UserId == userId);
        }

        public async Task<bool> UpdateAsync(Guid id, AddressDto address)
        {
            validator.ValidateAndThrow(address);

            var dbAddresses = await repository.GetByConditionAsync(entity => entity.Id == id);

            if (dbAddresses?.Any() != true)
            {
                return false;
            }

            var updatedAddress = mapper.Map(address, dbAddresses.First());
            await repository.UpdateAsync(updatedAddress);

            return true;
        }
    }
}
