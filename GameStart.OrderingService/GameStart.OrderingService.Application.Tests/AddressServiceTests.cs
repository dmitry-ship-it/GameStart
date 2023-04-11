using AutoMapper;
using FluentValidation;
using GameStart.OrderingService.Application.DtoModels;
using GameStart.OrderingService.Application.Services;
using GameStart.OrderingService.Core.Abstractions;
using GameStart.OrderingService.Core.Entities;
using GameStart.Shared;
using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;

namespace GameStart.OrderingService.Application.Tests
{
    public class AddressServiceTests
    {
        private readonly IAddressRepository repositoryMock;
        private readonly IMapper mapperMock;
        private readonly IValidator<AddressDto> validatorMock;

        private readonly AddressService sut;

        public AddressServiceTests()
        {
            repositoryMock = Substitute.For<IAddressRepository>();
            mapperMock = Substitute.For<IMapper>();
            validatorMock = Substitute.For<IValidator<AddressDto>>();

            sut = new AddressService(repositoryMock, mapperMock, validatorMock);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddAddressToRepository()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;

            var addressDto = new AddressDto
            {
                City = "City A",
                Country = "Some Country",
                Flat = "33",
                House = "5b",
                PostCode = "GT100",
                Street = "John Doe's"
            };

            var claims = Enumerable.Repeat(new Claim(
                ClaimTypes.NameIdentifier, userId.ToString()), 1);

            var expectedAddress = new Address
            {
                City = addressDto.City,
                Country = addressDto.Country,
                Flat = addressDto.Flat,
                House = addressDto.House,
                PostCode = addressDto.PostCode,
                Street = addressDto.Street,
                UserId = userId
            };

            mapperMock.Map<Address>(addressDto).Returns(expectedAddress);

            // Act
            await sut.CreateAsync(addressDto, claims, cancellationToken);

            // Assert
            await repositoryMock.Received().CreateAsync(expectedAddress, cancellationToken);
        }

        [Fact]
        public async Task DeleteAsync_WhenAddressNotFound_ShouldReturnFalse()
        {
            // Arrange
            var addressId = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;

            repositoryMock.GetByConditionAsync(Arg.Any<Expression<Func<Address, bool>>>(), cancellationToken)
                .Returns(Enumerable.Empty<Address>());

            // Act
            var result = await sut.DeleteAsync(addressId, Enumerable.Empty<Claim>(), cancellationToken);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteAsync_WhenAddressUserIdAndClaimsUserIdAreNotEquals_ShouldThrowArgumentException()
        {
            // Arrange
            var addressId = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;

            var address = new Address
            {
                UserId = Guid.NewGuid()
            };

            var claims = Enumerable.Repeat(new Claim(
                ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()), 1);

            repositoryMock.GetByConditionAsync(Arg.Any<Expression<Func<Address, bool>>>(), cancellationToken)
                .Returns(Enumerable.Repeat(address, 1));

            // Act
            var act = async () => await sut.DeleteAsync(addressId, claims, cancellationToken);

            // Assert
            await act.Should().ThrowExactlyAsync<ArgumentException>()
                .WithMessage(Constants.OrderingService.ExceptionMessages.CantDeleteOtherUsersAddress);
        }

        [Fact]
        public async Task DeleteAsync_WhenAddressFoundAndUserIdAreEqual_ShouldReturnTrue()
        {
            // Arrange
            var addressId = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;

            var address = new Address
            {
                UserId = Guid.NewGuid()
            };

            var claims = Enumerable.Repeat(new Claim(
                ClaimTypes.NameIdentifier, address.UserId.ToString()), 1);

            repositoryMock.GetByConditionAsync(Arg.Any<Expression<Func<Address, bool>>>(), cancellationToken)
                .Returns(Enumerable.Repeat(address, 1));

            // Act
            var result = await sut.DeleteAsync(addressId, claims, cancellationToken);

            // Assert
            result.Should().BeTrue();
            await repositoryMock.Received().DeleteAsync(address, cancellationToken);
        }

        [Fact]
        public async Task GetByUserIdAsync_ShouldCallRepository()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;

            var claims = Enumerable.Repeat(new Claim(
                ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()), 1);

            // Act
            await sut.GetByUserIdAsync(claims, cancellationToken);

            // Assert
            await repositoryMock.Received().GetByConditionAsync(
                Arg.Any<Expression<Func<Address, bool>>>(), cancellationToken);
        }

        [Fact]
        public async Task UpdateAsync_WhenAddressNotFound_ShouldReturnFalse()
        {
            // Arrange
            var addressId = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;

            var addressDto = new AddressDto { City = "New City" };

            repositoryMock.GetByConditionAsync(Arg.Any<Expression<Func<Address, bool>>>(), cancellationToken)
                .Returns(Enumerable.Empty<Address>());

            // Act
            var result = await sut.UpdateAsync(
                addressId, Enumerable.Empty<Claim>(), addressDto, cancellationToken);

            // Assert
            result.Should().BeFalse();
        }


        [Fact]
        public async Task UpdateAsync_WhenAddressUserIdAndClaimsUserIdAreNotEquals_ShouldThrowArgumentException()
        {
            // Arrange
            var addressId = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;

            var addressDto = new AddressDto { City = "New City" };

            var address = new Address
            {
                UserId = Guid.NewGuid()
            };

            var claims = Enumerable.Repeat(new Claim(
                ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()), 1);

            repositoryMock.GetByConditionAsync(Arg.Any<Expression<Func<Address, bool>>>(), cancellationToken)
                .Returns(Enumerable.Repeat(address, 1));

            // Act
            var act = async () => await sut.UpdateAsync(addressId, claims, addressDto, cancellationToken);

            // Assert
            await act.Should().ThrowExactlyAsync<ArgumentException>()
                .WithMessage(Constants.OrderingService.ExceptionMessages.CantUpdateOtherUsersAddress);
        }

        [Fact]
        public async Task UpdateAsync_WhenAddressFoundAndUserIdAreEqual_ShouldReturnTrue()
        {
            // Arrange
            var addressId = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;

            var addressDto = new AddressDto { City = "New City" };

            var address = new Address
            {
                UserId = Guid.NewGuid()
            };

            var claims = Enumerable.Repeat(new Claim(
                ClaimTypes.NameIdentifier, address.UserId.ToString()), 1);

            repositoryMock.GetByConditionAsync(Arg.Any<Expression<Func<Address, bool>>>(), cancellationToken)
                .Returns(Enumerable.Repeat(address, 1));

            var updatedAddress = new Address
            {
                UserId = address.UserId,
                City = addressDto.City
            };

            mapperMock.Map(addressDto, address).Returns(updatedAddress);

            // Act
            var result = await sut.UpdateAsync(addressId, claims, addressDto, cancellationToken);

            // Assert
            result.Should().BeTrue();
            await repositoryMock.Received().UpdateAsync(updatedAddress, cancellationToken);
        }
    }
}
