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
    public class OrderControllerTests
    {
        private readonly IOrderService orderServiceMock;
        private readonly OrderController sut;

        public OrderControllerTests()
        {
            orderServiceMock = Substitute.For<IOrderService>();

            var httpContextMock = Substitute.For<HttpContext>();
            httpContextMock.User.Returns(new ClaimsPrincipal(
                Identity.Create("test",
                    new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()))));

            sut = new OrderController(orderServiceMock)
            {
                ControllerContext = new() { HttpContext = httpContextMock }
            };
        }

        [Fact]
        public async Task GetAsync_ShouldReturnOkObjectResult()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;

            var orders = new List<Order>()
            {
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() }
            };

            orderServiceMock.GetByUserIdAsync(Arg.Any<IEnumerable<Claim>>(), cancellationToken)
                .Returns(orders);

            // Act
            var result = await sut.GetAsync(cancellationToken);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().BeEquivalentTo(orders);
        }

        [Fact]
        public async Task GetByIdAsync_WhenAddressFound_ShouldReturnOkObjectResult()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;

            var order = new Order { Id = Guid.NewGuid() };

            orderServiceMock.GetByIdAsync(order.Id, Arg.Any<IEnumerable<Claim>>(), cancellationToken)
                .Returns(order);

            // Act
            var result = await sut.GetByIdAsync(order.Id, cancellationToken);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().BeEquivalentTo(order);
        }

        [Fact]
        public async Task GetByIdAsync_WhenAddressNotFound_ShouldReturnNotFoundResult()
        {
            // Arrange
            var id = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;

            orderServiceMock.GetByIdAsync(id, Arg.Any<IEnumerable<Claim>>(), cancellationToken)
                .Returns((Order?)null);

            // Act
            var result = await sut.GetByIdAsync(id, cancellationToken);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnAcceptedResultWithId()
        {
            // Arrange
            var id = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;

            var orderDto = new OrderDto();

            orderServiceMock.CreateAsync(orderDto, Arg.Any<IEnumerable<Claim>>(), cancellationToken)
                .Returns(id);

            // Act
            var result = await sut.CreateAsync(orderDto, cancellationToken);

            // Assert
            result.Should().BeOfType<AcceptedResult>();
            result.As<AcceptedResult>().Value.Should().Be(id);
        }
    }
}
