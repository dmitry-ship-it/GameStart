using GameStart.CatalogService.Common.Elasticsearch.Extensions;
using GameStart.CatalogService.Common.Tests.TestData;
using GameStart.CatalogService.Data.Models;
using Nest;

namespace GameStart.CatalogService.Common.Tests
{
    public class TypeMapperDescriptorExtensionTests
    {
        [Fact]
        public void MapVideoGameGraph_ShouldReturnSameObjectType()
        {
            // Arrange
            var descriptor = Substitute.For<TypeMappingDescriptor<VideoGame>>();

            // Act
            var result = descriptor.MapVideoGameGraph();

            // Assert
            result.Should().BeSameAs(descriptor);
        }

        [Theory]
        [ClassData(typeof(TypeMappingDescriptorVideoGameTestData))]
        public void MapVideoGameGraph_ShouldMapVideoGameObjectProperly(Func<PropertiesDescriptor<VideoGame>, IPromise<IProperties>> selector)
        {
            // Arrange
            var descriptor = Substitute.For<TypeMappingDescriptor<VideoGame>>();

            // Act
            descriptor.MapVideoGameGraph();

            // Assert
            descriptor.Properties(Arg.Any<Func<PropertiesDescriptor<VideoGame>, IPromise<IProperties>>>())
                .Received()
                .Properties(selector);
        }

        [Theory]
        [ClassData(typeof(TypeMappingDescriptorPublisherTestData))]
        public void MapVideoGameGraph_ShouldMapPublisherObjectProperly(Func<PropertiesDescriptor<Publisher>, IPromise<IProperties>> selector)
        {
            // Arrange
            var descriptor = Substitute.For<TypeMappingDescriptor<VideoGame>>();

            // Act
            descriptor.MapVideoGameGraph();

            // Assert
            descriptor.Properties(Arg.Any<Func<PropertiesDescriptor<Publisher>, IPromise<IProperties>>>())
                .Received()
                .Properties(selector);
        }

        [Theory]
        [ClassData(typeof(TypeMappingDescriptorDeveloperTestData))]
        public void MapVideoGameGraph_ShouldMapDeveloperObjectProperly(Func<PropertiesDescriptor<Developer>, IPromise<IProperties>> selector)
        {
            // Arrange
            var descriptor = Substitute.For<TypeMappingDescriptor<VideoGame>>();

            // Act
            descriptor.MapVideoGameGraph();

            // Assert
            descriptor.Properties(Arg.Any<Func<PropertiesDescriptor<Developer>, IPromise<IProperties>>>())
                .Received()
                .Properties(selector);
        }

        [Theory]
        [ClassData(typeof(TypeMappingDescriptorGenreTestData))]
        public void MapVideoGameGraph_ShouldMapGenreObjectProperly(Func<PropertiesDescriptor<Genre>, IPromise<IProperties>> selector)
        {
            // Arrange
            var descriptor = Substitute.For<TypeMappingDescriptor<VideoGame>>();

            // Act
            descriptor.MapVideoGameGraph();

            // Assert
            descriptor.Properties(Arg.Any<Func<PropertiesDescriptor<Genre>, IPromise<IProperties>>>())
                .Received()
                .Properties(selector);
        }

        [Theory]
        [ClassData(typeof(TypeMappingDescriptorLanguageAvailabilityTestData))]
        public void MapVideoGameGraph_ShouldMapLanguageAvailabilityObjectProperly(Func<PropertiesDescriptor<LanguageAvailability>, IPromise<IProperties>> selector)
        {
            // Arrange
            var descriptor = Substitute.For<TypeMappingDescriptor<VideoGame>>();

            // Act
            descriptor.MapVideoGameGraph();

            // Assert
            descriptor.Properties(Arg.Any<Func<PropertiesDescriptor<LanguageAvailability>, IPromise<IProperties>>>())
                .Received()
                .Properties(selector);
        }

        [Theory]
        [ClassData(typeof(TypeMappingDescriptorLanguageTestData))]
        public void MapVideoGameGraph_ShouldMapLanguageObjectProperly(Func<PropertiesDescriptor<Data.Models.Language>, IPromise<IProperties>> selector)
        {
            // Arrange
            var descriptor = Substitute.For<TypeMappingDescriptor<VideoGame>>();

            // Act
            descriptor.MapVideoGameGraph();

            // Assert
            descriptor.Properties(Arg.Any<Func<PropertiesDescriptor<Data.Models.Language>, IPromise<IProperties>>>())
                .Received()
                .Properties(selector);
        }

        [Theory]
        [ClassData(typeof(TypeMappingDescriptorSystemRequirementsTestData))]
        public void MapVideoGameGraph_ShouldMapSystemRequirementsObjectProperly(Func<PropertiesDescriptor<SystemRequirements>, IPromise<IProperties>> selector)
        {
            // Arrange
            var descriptor = Substitute.For<TypeMappingDescriptor<VideoGame>>();

            // Act
            descriptor.MapVideoGameGraph();

            // Assert
            descriptor.Properties(Arg.Any<Func<PropertiesDescriptor<SystemRequirements>, IPromise<IProperties>>>())
                .Received()
                .Properties(selector);
        }

        [Theory]
        [ClassData(typeof(TypeMappingDescriptorPlatformTestData))]
        public void MapVideoGameGraph_ShouldMapPlatformObjectProperly(Func<PropertiesDescriptor<Platform>, IPromise<IProperties>> selector)
        {
            // Arrange
            var descriptor = Substitute.For<TypeMappingDescriptor<VideoGame>>();

            // Act
            descriptor.MapVideoGameGraph();

            // Assert
            descriptor.Properties(Arg.Any<Func<PropertiesDescriptor<Platform>, IPromise<IProperties>>>())
                .Received()
                .Properties(selector);
        }
    }
}
