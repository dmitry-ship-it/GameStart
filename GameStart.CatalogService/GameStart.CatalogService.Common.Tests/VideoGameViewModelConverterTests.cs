using AutoMapper;
using GameStart.CatalogService.Common.Mapping;
using GameStart.CatalogService.Common.ViewModels;
using GameStart.CatalogService.Data.Models;
using GameStart.CatalogService.Data.Repositories;
using System.Linq.Expressions;

namespace GameStart.CatalogService.Common.Tests
{
    public class VideoGameViewModelConverterTests
    {
        private readonly IRepositoryWrapper repository;
        private readonly IMapper mapper;
        private readonly VideoGameViewModelConverter converter;

        public VideoGameViewModelConverterTests()
        {
            repository = Substitute.For<IRepositoryWrapper>();
            mapper = Substitute.For<IMapper>();
            converter = new VideoGameViewModelConverter(repository);
        }

        [Fact]
        public void Convert_WhenDestinationIsNull_ShouldReturnNewVideoGame()
        {
            // Arrange
            var source = new VideoGameViewModel { Title = "Test Game" };
            VideoGame? destination = null;

            // Act
            var result = converter.Convert(source, destination, default);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void Convert_WhenSourceTitleIsNotNull_ShouldUpdateTitle()
        {
            // Arrange
            var source = new VideoGameViewModel { Title = "New Title" };
            var destination = new VideoGame { Title = "Old Title" };

            // Act
            var result = converter.Convert(source, destination, default);

            // Assert
            result.Title.Should().Be("New Title");
        }

        [Fact]
        public void Convert_WhenSourceTitleIsNull_ShouldNotUpdateTitle()
        {
            // Arrange
            var source = new VideoGameViewModel { Title = null };
            var destination = new VideoGame { Title = "Old Title" };

            // Act
            var result = converter.Convert(source, destination, default);

            // Assert
            result.Title.Should().Be("Old Title");
        }

        [Fact]
        public void Convert_WhenSourceDescriptionIsNotNull_ShouldUpdateDescription()
        {
            // Arrange
            var source = new VideoGameViewModel { Description = "New Description" };
            var destination = new VideoGame { Description = "Old Description" };

            // Act
            var result = converter.Convert(source, destination, default);

            // Assert
            result.Description.Should().Be("New Description");
        }

        [Fact]
        public void Convert_WhenSourceDescriptionIsNull_ShouldNotUpdateDescription()
        {
            // Arrange
            var source = new VideoGameViewModel { Description = null };
            var destination = new VideoGame { Description = "Old Description" };

            // Act
            var result = converter.Convert(source, destination, default);

            // Assert
            result.Description.Should().Be("Old Description");
        }

        [Fact]
        public void Convert_WhenSourceCopyrightIsNotNull_ShouldUpdateCopyright()
        {
            // Arrange
            var source = new VideoGameViewModel { Copyright = "New Copyright" };
            var destination = new VideoGame { Copyright = "Old Copyright" };

            // Act
            var result = converter.Convert(source, destination, default);

            // Assert
            result.Copyright.Should().Be("New Copyright");
        }

        [Fact]
        public void Convert_WhenSourceCopyrightIsNull_ShouldNotUpdateCopyright()
        {
            // Arrange
            var source = new VideoGameViewModel { Copyright = null };
            var destination = new VideoGame { Copyright = "Old Copyright" };

            // Act
            var result = converter.Convert(source, destination, default);

            // Assert
            result.Copyright.Should().Be("Old Copyright");
        }

        [Fact]
        public void Convert_WhenSourceReleaseDateIsNotNull_ShouldUpdateReleaseDate()
        {
            // Arrange
            var source = new VideoGameViewModel { ReleaseDate = new DateTime(2022, 1, 1) };
            var destination = new VideoGame { ReleaseDate = new DateTime(2021, 1, 1) };

            // Act
            var result = converter.Convert(source, destination, default);

            // Assert
            result.ReleaseDate.Should().Be(new DateTime(2022, 1, 1));
        }

        [Fact]
        public void Convert_WhenSourceReleaseDateIsNull_ShouldNotUpdateReleaseDate()
        {
            // Arrange
            var source = new VideoGameViewModel { ReleaseDate = null };
            var destination = new VideoGame { ReleaseDate = new DateTime(2021, 1, 1) };

            // Act
            var result = converter.Convert(source, destination, default);

            // Assert
            result.ReleaseDate.Should().Be(new DateTime(2021, 1, 1));
        }

        [Fact]
        public void Convert_WhenSourcePriceIsNotNull_ShouldUpdatePrice()
        {
            // Arrange
            var source = new VideoGameViewModel { Price = 49.99M };
            var destination = new VideoGame { Price = 39.99M };

            // Act
            var result = converter.Convert(source, destination, default);

            // Assert
            result.Price.Should().Be(49.99M);
        }

        [Fact]
        public void Convert_WhenSourcePriceIsNull_ShouldNotUpdatePrice()
        {
            // Arrange
            var source = new VideoGameViewModel() { Price = null };
            var destination = new VideoGame { Price = 39.99M };

            // Act
            var result = converter.Convert(source, destination, default);

            // Assert
            result.Price.Should().Be(39.99M);
        }

        [Fact]
        public void Convert_WhenSourceSystemRequirementsIsNull_ShouldNotMapSystemRequirements()
        {
            // Arrange
            var source = new VideoGameViewModel { SystemRequirements = null };
            var destination = new VideoGame { SystemRequirements = new List<SystemRequirements>() };

            // Act
            var result = converter.Convert(source, destination, default);

            // Assert
            result.SystemRequirements.Should().BeEmpty();
        }

        [Fact]
        public void Convert_WhenSourcePublisherIsNotNull_ShouldMapPublisher()
        {
            // Arrange
            var source = new VideoGameViewModel { Publisher = "Test Publisher" };
            var destination = new VideoGame();

            var publisher = new Publisher { Name = "Test Publisher" };
            repository.Publishers.FindByConditionAsync(Arg.Any<Expression<Func<Publisher, bool>>>())
                .Returns(new List<Publisher> { publisher });

            // Act
            var result = converter.Convert(source, destination, default);

            // Assert
            result.Publisher.Should().Be(publisher);
        }

        [Fact]
        public void Convert_WhenSourcePublisherIsNotInDatabase_ShouldCreateNewPublisher()
        {
            // Arrange
            var source = new VideoGameViewModel { Publisher = "New Publisher" };
            var destination = new VideoGame();

            repository.Publishers.FindByConditionAsync(Arg.Any<Expression<Func<Publisher, bool>>>())
                .Returns(new List<Publisher>());

            // Act
            var result = converter.Convert(source, destination, default);

            // Assert
            result.Publisher.Name.Should().Be("New Publisher");
        }

        [Fact]
        public void Convert_WhenSourceDevelopersIsNull_ShouldNotMapDevelopers()
        {
            // Arrange
            var source = new VideoGameViewModel { Developers = null };
            var destination = new VideoGame();

            // Act
            var result = converter.Convert(source, destination, default);

            // Assert
            result.Publisher.Should().BeNull();
        }

        [Fact]
        public void Convert_WhenSourceGenresIsNull_ShouldNotMapGenres()
        {
            // Arrange
            var source = new VideoGameViewModel { Genres = null };
            var destination = new VideoGame();

            // Act
            var result = converter.Convert(source, destination, default);

            // Assert
            result.Genres.Should().BeNull();
        }

        [Fact]
        public void Convert_WhenSourceLanguagesIsNull_ShouldNotMapLanguageAvailabilities()
        {
            // Arrange
            var source = new VideoGameViewModel { Languages = null };
            var destination = new VideoGame();

            // Act
            var result = converter.Convert(source, destination, default);

            // Assert
            result.LanguageAvailabilities.Should().BeNull();
        }
    }
}
