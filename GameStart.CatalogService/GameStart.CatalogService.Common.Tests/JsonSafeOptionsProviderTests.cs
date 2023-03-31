using GameStart.CatalogService.Common.Services;
using GameStart.CatalogService.Data.EntityConfigurations.ValueConverters;
using System.Text.Json.Serialization;

namespace GameStart.CatalogService.Common.Tests
{
    public class JsonSafeOptionsProviderTests
    {
        [Fact]
        public void JsonSerializerOptions_ShouldContainDateOnlyJsonConverter()
        {
            // Arrange
            var provider = new JsonSafeOptionsProvider();

            // Act
            var options = provider.JsonSerializerOptions;

            // Assert
            options.Converters.Should().Contain(converter => converter is DateOnlyJsonConverter);
        }

        [Fact]
        public void JsonSerializerOptions_ShouldIgnoreCycles()
        {
            // Arrange
            var provider = new JsonSafeOptionsProvider();

            // Act
            var options = provider.JsonSerializerOptions;

            // Assert
            options.ReferenceHandler.Should().BeSameAs(ReferenceHandler.IgnoreCycles);
        }
    }
}
