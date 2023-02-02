using AutoMapper;
using GameStart.CatalogService.Common.ViewModels;
using GameStart.CatalogService.Data.Models;
using GameStart.CatalogService.Data.Repositories;

namespace GameStart.CatalogService.Common.Mapping
{
    public class VideoGameViewModelProfile : Profile
    {
        public VideoGameViewModelProfile(IRepositoryWrapper repository)
        {
            CreateMap<string, Platform>().ForMember(platform => platform.Name,
                options => options.MapFrom(source => source));

            CreateMap<string, Developer>().ForMember(developer => developer.Name,
                options => options.MapFrom(source => source));

            CreateMap<string, Ganre>().ForMember(ganre => ganre.Name,
                options => options.MapFrom(source => source));

            CreateMap<LanguageViewModel, LanguageAvailability>().ForPath(entity => entity.Language.Name,
                options => options.MapFrom(source => source.Name));

            CreateMap<SystemRequirementsViewModel, SystemRequirements>();

            CreateMap<VideoGameViewModel, VideoGame>().ConvertUsing(new VideoGameViewModelConverter(repository));
        }
    }
}
