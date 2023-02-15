using GameStart.CatalogService.Data.Models;
using Nest;

namespace GameStart.CatalogService.Common.Elasticsearch.Extensions
{
    public static class CreateIndexDescriptorExtension
    {
        public static CreateIndexDescriptor MapVideoGame(this CreateIndexDescriptor descriptor)
        {
            return descriptor.Map<VideoGame>(map => map.Properties(property => property
                .Keyword(key => key.Name(name => name.Id))
                .Text(text => text.Name(name => name.Title))
                .Text(text => text.Name(name => name.Description))
                .Text(text => text.Name(name => name.Copyright))
                .Date(date => date.Name(name => name.ReleaseDate))
                .Number(number => number.Name(name => name.Price))
                .Object<Publisher>(child => child.Name(name => name.Publisher).Properties(publisher => publisher
                    .Keyword(key => key.Name(name => name.Id))
                    .Text(text => text.Name(name => name.Name))
                    .Object<VideoGame>(child => child.Name(name => name.VideoGames).Enabled(false))
                ))
                .Nested<Developer>(child => child.Name(name => name.Developers).Properties(developer => developer
                    .Keyword(key => key.Name(name => name.Id))
                    .Text(text => text.Name(name => name.Name))
                    .Object<VideoGame>(child => child.Name(name => name.VideoGames).Enabled(false))
                ))
                .Nested<Ganre>(child => child.Name(name => name.Ganres).Properties(ganre => ganre
                    .Keyword(key => key.Name(name => name.Id))
                    .Text(text => text.Name(name => name.Name))
                    .Object<VideoGame>(child => child.Name(name => name.VideoGames).Enabled(false))
                ))
                .Nested<LanguageAvailability>(child => child.Name(name => name.LanguageAvailabilities).Properties(ganre => ganre
                    .Keyword(key => key.Name(name => name.Id))
                    .Boolean(boolean => boolean.Name(name => name.AvailableForInterface))
                    .Boolean(boolean => boolean.Name(name => name.AvailableForAudio))
                    .Boolean(boolean => boolean.Name(name => name.AvailableForSubtitles))
                    .Nested<Data.Models.Language>(child => child.Name(name => name.Language).Properties(language => language
                        .Keyword(key => key.Name(name => name.Id))
                        .Text(text => text.Name(name => name.Name))
                        .Object<LanguageAvailability>(child => child.Name(name => name.LanguageAvailabilities).Enabled(false))
                    ))
                    .Object<VideoGame>(child => child.Name(name => name.VideoGames).Enabled(false))
                ))
                .Nested<SystemRequirements>(child => child.Name(name => name.SystemRequirements).Properties(requirements => requirements
                    .Keyword(key => key.Name(name => name.Id))
                    .Text(text => text.Name(name => name.OS))
                    .Text(text => text.Name(name => name.Processor))
                    .Text(text => text.Name(name => name.Memory))
                    .Text(text => text.Name(name => name.Graphics))
                    .Text(text => text.Name(name => name.Network))
                    .Text(text => text.Name(name => name.Storage))
                    .Nested<Platform>(child => child.Name(name => name.Platform).Properties(platform => platform
                        .Keyword(key => key.Name(name => name.Id))
                        .Text(text => text.Name(name => name.Name))
                        .Object<Platform>(child => child.Name(name => name.SystemRequirements).Enabled(false))
                    ))
                    .Object<VideoGame>(child => child.Name(name => name.VideoGame).Enabled(false))
                ))
            ));
        }
    }
}
