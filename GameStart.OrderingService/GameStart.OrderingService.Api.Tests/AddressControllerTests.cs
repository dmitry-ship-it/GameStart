using GameStart.OrderingService.Api.Controllers;
using GameStart.OrderingService.Application.DtoModels;
using GameStart.OrderingService.Application.Services;
using GameStart.OrderingService.Core.Entities;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GameStart.OrderingService.Api.Tests
{
    public class AddressControllerTests
    {
        private readonly IAddressService addressServiceMock;
        private readonly AddressController sut;

        public AddressControllerTests()
        {
            addressServiceMock = Substitute.For<IAddressService>();

            var httpContextMock = Substitute.For<HttpContext>();
            httpContextMock.User.Returns(new ClaimsPrincipal(
                Identity.Create("test",
                    new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()))));

            sut = new AddressController(addressServiceMock)
            {
                ControllerContext = new() { HttpContext = httpContextMock }
            };
        }

        [Fact]
        public async Task GetAsync_ShouldReturnOkObjectResult()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;

            var addresses = new List<Address>()
            {
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() }
            };

            addressServiceMock.GetByUserIdAsync(Arg.Any<IEnumerable<Claim>>(), cancellationToken)
                .Returns(addresses);

            // Act
            var result = await sut.GetAsync(cancellationToken);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().BeEquivalentTo(addresses);
        }

        [Fact]
        public async Task CreateAsync_ShouldCallAddressServiceAndReturnOkResult()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;

            var address = new AddressDto
            {
                City = "Some City"
            };

            // Act
            var result = await sut.CreateAsync(address, cancellationToken);

            // Assert
            result.Should().BeOfType<OkResult>();
            await addressServiceMock.Received().CreateAsync(address, Arg.Is<IEnumerable<Claim>>(
                sequence => sequence.Any(
                    claim => claim.Type == ClaimTypes.NameIdentifier)), cancellationToken);
        }

        [Fact]
        public async Task UpdateAsync_WhenUpdateFailed_ShouldReturnNotFoundResult()
        {
            // Arrange
            var id = Guid.NewGuid();
            var address = new AddressDto
            {
                City = "Some City"
            };

            var cancellationToken = CancellationToken.None;

            addressServiceMock.UpdateAsync(id, Arg.Any<IEnumerable<Claim>>(), address, cancellationToken)
                .Returns(false);

            // Act
            var result = await sut.UpdateAsync(id, address, cancellationToken);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task UpdateAsync_WhenUpdateSucceed_ShouldReturnOkResult()
        {
            // Arrange
            var id = Guid.NewGuid();
            var address = new AddressDto
            {
                City = "Some City"
            };

            var cancellationToken = CancellationToken.None;

            addressServiceMock.UpdateAsync(id, Arg.Any<IEnumerable<Claim>>(), address, cancellationToken)
                .Returns(true);

            // Act
            var result = await sut.UpdateAsync(id, address, cancellationToken);

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task DeleteAsync_WhenDeletionFailed_ShouldReturnNotFoundResult()
        {
            // Arrange
            var id = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;

            addressServiceMock.DeleteAsync(id, Arg.Any<IEnumerable<Claim>>(), cancellationToken)
                .Returns(false);

            // Act
            var result = await sut.DeleteAsync(id, cancellationToken);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DeleteAsync_WhenDeletionSucceed_ShouldReturnNoContentResult()
        {
            // Arrange
            var id = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;

            addressServiceMock.DeleteAsync(id, Arg.Any<IEnumerable<Claim>>(), cancellationToken)
                .Returns(true);

            // Act
            var result = await sut.DeleteAsync(id, cancellationToken);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }
    }
}
