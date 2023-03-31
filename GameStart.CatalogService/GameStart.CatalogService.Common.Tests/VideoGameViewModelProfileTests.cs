using AutoMapper;
using GameStart.CatalogService.Common.Mapping;
using GameStart.CatalogService.Common.ViewModels;
using GameStart.CatalogService.Data.Models;
using GameStart.CatalogService.Data.Repositories;
using System.Linq.Expressions;

namespace GameStart.CatalogService.Common.Tests
{
    public class VideoGameViewModelProfileTests
    {
        private readonly CancellationToken cancellationToken = new();
        private readonly IRepositoryWrapper repositoryMock;
        private readonly SystemRequirementsViewModel testSystemRequirements;

        public VideoGameViewModelProfileTests()
        {
            var systemRequirementsRepository = Substitute.For<IRepository<SystemRequirements>>();
            systemRequirementsRepository.FindAllAsync(false, cancellationToken).Returns(Enumerable.Empty<SystemRequirements>());

            var publishersRepository = Substitute.For<IRepository<Publisher>>();
            publishersRepository.FindByConditionAsync(Arg.Any<Expression<Func<Publisher, bool>>>(), cancellationToken).Returns(Enumerable.Empty<Publisher>());

            var platformsRepository = Substitute.For<IRepository<Platform>>();
            platformsRepository.FindAllAsync(false, cancellationToken).Returns(Enumerable.Empty<Platform>());

            var languagesRepository = Substitute.For<IRepository<Language>>();
            languagesRepository.FindAllAsync(false, cancellationToken).Returns(Enumerable.Empty<Language>());

            var languageAvailabilitiesRepository = Substitute.For<IRepository<LanguageAvailability>>();
            languageAvailabilitiesRepository.FindAllAsync(false, cancellationToken).Returns(Enumerable.Empty<LanguageAvailability>());

            var genresRepository = Substitute.For<IRepository<Genre>>();
            genresRepository.FindAllAsync(false, cancellationToken).Returns(Enumerable.Empty<Genre>());

            var developersRepository = Substitute.For<IRepository<Developer>>();
            developersRepository.FindAllAsync(false, cancellationToken).Returns(Enumerable.Empty<Developer>());

            repositoryMock = Substitute.For<IRepositoryWrapper>();
            repositoryMock.SystemRequirements.Returns(systemRequirementsRepository);
            repositoryMock.Publishers.Returns(publishersRepository);
            repositoryMock.Platforms.Returns(platformsRepository);
            repositoryMock.Languages.Returns(languagesRepository);
            repositoryMock.LanguageAvailabilities.Returns(languageAvailabilitiesRepository);
            repositoryMock.Genres.Returns(genresRepository);
            repositoryMock.Developers.Returns(developersRepository);

            testSystemRequirements = new()
            {
                Graphics = "Test graphics",
                Memory = "Test memory",
                Network = "Test network",
                OS = "Test OS",
                Platform = "Test platform",
                Processor = "Test processor",
                Storage = "Test storage"
            };
        }

        [Fact]
        public void CreateMap_ShouldMapStringToPlatformWithName()
        {
            // Arrange
            const string platformName = "Test platform";
            var profile = new VideoGameViewModelProfile(null);
            var config = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            var mapper = config.CreateMapper();

            // Act
            var platform = mapper.Map<string, Platform>(platformName);

            // Assert
            platform.Should().NotBeNull();
            platform.Name.Should().Be(platformName);
        }

        [Fact]
        public void CreateMap_ShouldMapStringToDeveloperWithName()
        {
            // Arrange
            const string developerName = "Test developer";
            var profile = new VideoGameViewModelProfile(null);
            var config = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            var mapper = config.CreateMapper();

            // Act
            var developer = mapper.Map<string, Developer>(developerName);

            // Assert
            developer.Should().NotBeNull();
            developer.Name.Should().Be(developerName);
        }

        [Fact]
        public void CreateMap_ShouldMapStringToGenreWithName()
        {
            // Arrange
            const string genreName = "Test genre";
            var profile = new VideoGameViewModelProfile(null);
            var config = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            var mapper = config.CreateMapper();

            // Act
            var genre = mapper.Map<string, Genre>(genreName);

            // Assert
            genre.Should().NotBeNull();
            genre.Name.Should().Be(genreName);
        }

        [Fact]
        public void CreateMap_ShouldMapLanguageViewModelToLanguageAvailabilityWithName()
        {
            // Arrange
            const string languageName = "Test language";
            var profile = new VideoGameViewModelProfile(null);
            var config = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            var mapper = config.CreateMapper();
            var languageViewModel = new LanguageViewModel { Name = languageName };

            // Act
            var languageAvailability = mapper.Map<LanguageViewModel, LanguageAvailability>(languageViewModel);

            // Assert
            languageAvailability.Should().NotBeNull();
            languageAvailability.Language.Should().NotBeNull();
            languageAvailability.Language.Name.Should().Be(languageName);
        }

        [Fact]
        public void CreateMap_ShouldMapSystemRequirementsViewModelToSystemRequirements()
        {
            // Arrange
            var profile = new VideoGameViewModelProfile(null);
            var config = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            var mapper = config.CreateMapper();
            var systemRequirementsViewModel = testSystemRequirements;

            // Act
            var systemRequirements = mapper.Map<SystemRequirementsViewModel, SystemRequirements>(systemRequirementsViewModel);

            // Assert
            systemRequirements.Should().NotBeNull();
            systemRequirements.Graphics.Should().Be(systemRequirementsViewModel.Graphics);
            systemRequirements.Memory.Should().Be(systemRequirementsViewModel.Memory);
            systemRequirements.Network.Should().Be(systemRequirementsViewModel.Network);
            systemRequirements.OS.Should().Be(systemRequirementsViewModel.OS);
            systemRequirements.Platform.Should().BeEquivalentTo(new Platform { Name = systemRequirementsViewModel.Platform });
            systemRequirements.Processor.Should().Be(systemRequirementsViewModel.Processor);
            systemRequirements.Storage.Should().Be(systemRequirementsViewModel.Storage);
        }

        [Fact]
        public void CreateMap_ShouldUseVideoGameViewModelConverterToConvertToVideoGame()
        {
            // Arrange
            var repository = Substitute.For<IRepositoryWrapper>();
            var profile = new VideoGameViewModelProfile(repository);
            var config = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            var mapper = config.CreateMapper();
            var videoGameViewModel = new VideoGameViewModel
            {
                Title = "Test title",
                Description = "Test description",
                Copyright = "Test copyright",
                Price = 59.99M,
                ReleaseDate = new DateTime(2015, 5, 25, 23, 45, 11),
                Publisher = "Test publisher",
                Developers = new[] { "Test developer 1", "Test developer 2" },
                Genres = new[] { "Test genre 1", "Test genre 2" },
                Languages = new LanguageViewModel[]
                {
                    new()
                    {
                        Name = "Test language 1",
                        AvailableForInterface = false,
                        AvailableForAudio = true,
                        AvailableForSubtitles = true
                    },
                    new()
                    {
                        Name = "Test language 2",
                        AvailableForInterface = true,
                        AvailableForAudio = false,
                        AvailableForSubtitles = true
                    }
                },
                SystemRequirements = new[] { testSystemRequirements }
            };

            // Act
            var videoGame = mapper.Map<VideoGameViewModel, VideoGame>(videoGameViewModel);

            // Assert
            videoGame.Should().NotBeNull();
            videoGame.Title.Should().Be(videoGameViewModel.Title);
            repository.Received().Publishers.FindByConditionAsync(Arg.Any<Expression<Func<Publisher, bool>>>());
            repository.Received().Developers.FindAllAsync();
            repository.Received().Genres.FindAllAsync();
            repository.Received().Languages.FindAllAsync();
            repository.Received().Platforms.FindAllAsync();
        }
    }
}
