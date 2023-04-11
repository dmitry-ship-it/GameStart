using GameStart.OrderingService.Application.Services;

namespace GameStart.OrderingService.Application.Tests
{
    public class GameKeyGeneratorServiceTests
    {
        [Fact]
        public void Generate_ShouldGenerateKeyByPattern()
        {
            // Arrange
            var sut = new GameKeyGeneratorService();

            // Act
            var result = sut.Generate();

            // Assert
            result.Should().MatchRegex("^[a-zA-Z0-9]{5}-[a-zA-Z0-9]{5}-[a-zA-Z0-9]{5}$");
        }
    }
}
