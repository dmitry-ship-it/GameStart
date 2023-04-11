using GameStart.OrderingService.Application.DtoModels;
using GameStart.OrderingService.Application.Validators;

namespace GameStart.OrderingService.Application.Tests
{
    public class ItemDtoValidatorTests
    {
        private readonly ItemDtoValidator sut = new();

        [Fact]
        public void Validate_ShouldPass_WhenIdIsNotEmpty()
        {
            // Arrange
            var item = new ItemDto
            {
                GameId = Guid.NewGuid()
            };

            // Act
            var result = sut.Validate(item);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_ShouldNotPass_WhenIdIsEmpty()
        {
            // Arrange
            var item = new ItemDto
            {
                GameId = Guid.Empty
            };

            // Act
            var result = sut.Validate(item);

            // Assert
            result.IsValid.Should().BeFalse();
        }
    }
}
