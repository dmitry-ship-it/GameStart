using GameStart.OrderingService.Application.DtoModels;
using GameStart.OrderingService.Application.Tests.TestData;
using GameStart.OrderingService.Application.Validators;

namespace GameStart.OrderingService.Application.Tests
{
    public class AddressDtoValidatorTests
    {
        private readonly AddressDtoValidator sut = new();

        [Theory]
        [ClassData(typeof(ValidAddressDtos))]
        public void Validate_ShouldPass_WhenDtoIsValid(AddressDto addressDto)
        {
            // Act
            var result = sut.Validate(addressDto);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [ClassData(typeof(InvalidAddressDtos))]
        public void Validate_ShouldNotPass_WhenDtoIsInvalid(AddressDto addressDto)
        {
            // Act
            var result = sut.Validate(addressDto);

            // Assert
            result.IsValid.Should().BeFalse();
        }
    }
}
