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
        public void Convert_ShouldReturnNewVideoGame_WhenDestinationIsNull()
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
        public void Convert_ShouldUpdateTitle_WhenSourceTitleIsNotNull()
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
        public void Convert_ShouldNotUpdateTitle_WhenSourceTitleIsNull()
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
        public void Convert_ShouldUpdateDescription_WhenSourceDescriptionIsNotNull()
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
        public void Convert_ShouldNotUpdateDescription_WhenSourceDescriptionIsNull()
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
        public void Convert_ShouldUpdateCopyright_WhenSourceCopyrightIsNotNull()
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
        public void Convert_ShouldNotUpdateCopyright_WhenSourceCopyrightIsNull()
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
        public void Convert_ShouldUpdateReleaseDate_WhenSourceReleaseDateIsNotNull()
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
        public void Convert_ShouldNotUpdateReleaseDate_WhenSourceReleaseDateIsNull()
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
        public void Convert_ShouldUpdatePrice_WhenSourcePriceIsNotNull()
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
        public void Convert_ShouldNotUpdatePrice_WhenSourcePriceIsNull()
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
        public void Convert_ShouldNotMapSystemRequirements_WhenSourceSystemRequirementsIsNull()
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
        public void Convert_ShouldMapPublisher_WhenSourcePublisherIsNotNull()
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
        public void Convert_ShouldCreateNewPublisher_WhenSourcePublisherIsNotInDatabase()
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
        public void Convert_ShouldNotMapDevelopers_WhenSourceDevelopersIsNull()
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
        public void Convert_ShouldNotMapGenres_WhenSourceGenresIsNull()
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
        public void Convert_ShouldNotMapLanguageAvailabilities_WhenSourceLanguagesIsNull()
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
