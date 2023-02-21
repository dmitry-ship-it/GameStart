using AutoMapper;
using GameStart.CatalogService.Common.ViewModels;
using GameStart.CatalogService.Data.Models;
using GameStart.CatalogService.Data.Repositories;

namespace GameStart.CatalogService.Common.Mapping
{
    public class VideoGameViewModelConverter : ITypeConverter<VideoGameViewModel, VideoGame>
    {
        private readonly IRepositoryWrapper repository;

        public VideoGameViewModelConverter(IRepositoryWrapper repository)
        {
            this.repository = repository;
        }

        public VideoGame Convert(
            VideoGameViewModel source,
            VideoGame destination,
            ResolutionContext context)
        {
            destination ??= new();

            destination.Title = source.Title ?? destination.Title;
            destination.Description = source.Description ?? destination.Description;
            destination.Copyright = source.Copyright ?? destination.Copyright;
            destination.ReleaseDate = source.ReleaseDate ?? destination.ReleaseDate;
            destination.Price = source.Price ?? destination.Price;

            destination.SystemRequirements = source.SystemRequirements is not null
                ? MapSystemRequirements(source.SystemRequirements, context).ToArray()
                : destination.SystemRequirements;

            destination.Publisher = source.Publisher is not null
                ? MapPublisher(source.Publisher)
                : destination.Publisher;

            destination.Developers = source.Developers is not null
                ? MapDevelopers(source.Developers, context).ToArray()
                : destination.Developers;

            destination.Genres = source.Genres is not null
                ? MapGenres(source.Genres, context).ToArray()
                : destination.Genres;

            destination.LanguageAvailabilities = source.Languages is not null
                ? MapLanguages(source.Languages, context).ToArray()
                : destination.LanguageAvailabilities;

            return destination;
        }

        private IEnumerable<SystemRequirements> MapSystemRequirements(
            IList<SystemRequirementsViewModel> systemRequirements,
            ResolutionContext context)
        {
            var found = repository.Platforms.FindAllAsync().Result;

            return systemRequirements.Select(selector =>
            {
                var result = context.Mapper.Map<SystemRequirementsViewModel, SystemRequirements>(selector);

                result.Platform = found.FirstOrDefault(platform =>
                    platform.Name == result.Platform.Name, result.Platform);

                return result;
            });
        }

        private Publisher MapPublisher(string publisher)
        {
            var found = repository.Publishers.FindByConditionAsync(
                dbPublisher => dbPublisher.Name == publisher).Result;

            return found.FirstOrDefault() ?? new Publisher { Name = publisher };
        }

        private IEnumerable<Developer> MapDevelopers(IList<string> developers, ResolutionContext context)
        {
            var found = repository.Developers.FindAllAsync().Result;

            return developers.Select(selector =>
            {
                var result = context.Mapper.Map<Developer>(selector);

                return found.FirstOrDefault(developer => developer.Name == result.Name, result);
            });
        }

        private IEnumerable<Genre> MapGenres(IList<string> genres, ResolutionContext context)
        {
            var found = repository.Genres.FindAllAsync().Result;

            return genres.Select(selector =>
            {
                var result = context.Mapper.Map<Genre>(selector);

                return found.FirstOrDefault(genre => genre.Name == result.Name, result);
            });
        }

        private IEnumerable<LanguageAvailability> MapLanguages(IList<LanguageViewModel> languages, ResolutionContext context)
        {
            var found = repository.Languages.FindAllAsync().Result;

            return languages.Select(selector =>
            {
                var result = context.Mapper.Map<LanguageAvailability>(selector);

                result.Language = found.FirstOrDefault(entity =>
                    entity.Name == result.Language.Name, result.Language);

                return result;
            });
        }
    }
}
