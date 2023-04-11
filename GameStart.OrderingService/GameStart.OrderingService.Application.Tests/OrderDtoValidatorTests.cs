using GameStart.OrderingService.Application.DtoModels;
using GameStart.OrderingService.Application.Tests.TestData;
using GameStart.OrderingService.Application.Validators;

namespace GameStart.OrderingService.Application.Tests
{
    public class OrderDtoValidatorTests
    {
        private readonly OrderDtoValidator sut = new();

        [Theory]
        [ClassData(typeof(ValidOrderDtos))]
        public void Validate_ShouldPass_WhenDtoIsValid(OrderDto orderDto)
        {
            // Act
            var result = sut.Validate(orderDto);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [ClassData(typeof(InvalidOrderDtos))]
        public void Validate_ShouldNotPass_WhenDtoIsInvalid(OrderDto orderDto)
        {
            // Act
            var result = sut.Validate(orderDto);

            // Assert
            result.IsValid.Should().BeFalse();
        }
    }
}
