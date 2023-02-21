using GameStart.CatalogService.Data.Models;
using Nest;

namespace GameStart.CatalogService.Common.Elasticsearch.Extensions
{
    public static class CreateIndexDescriptorExtension
    {
        public static TypeMappingDescriptor<VideoGame> MapVideoGameGraph(this TypeMappingDescriptor<VideoGame> descriptor)
        {
            return descriptor.Properties(property => property
                .Keyword(key => key.Name(name => name.Id))
                .Text(text => text.Name(name => name.Title))
                .Text(text => text.Name(name => name.Description))
                .Text(text => text.Name(name => name.Copyright))
                .Date(date => date.Name(name => name.ReleaseDate))
                .Number(number => number.Name(name => name.Price).Type(NumberType.Double))
                .Object<Publisher>(child => child.Name(name => name.Publisher))
                .Nested<Developer>(child => child.Name(name => name.Developers))
                .Nested<Genre>(child => child.Name(name => name.Genres))
                .Nested<LanguageAvailability>(child => child.Name(name => name.LanguageAvailabilities))
                .Nested<SystemRequirements>(child => child.Name(name => name.SystemRequirements)))
            .Properties<Publisher>(properties => properties
                .Keyword(key => key.Name(name => name.Id))
                .Text(text => text.Name(name => name.Name)))
            .Properties<Developer>(properties => properties
                .Keyword(key => key.Name(name => name.Id))
                .Text(text => text.Name(name => name.Name)))
            .Properties<Genre>(properties => properties
                .Keyword(key => key.Name(name => name.Id))
                .Text(text => text.Name(name => name.Name)))
            .Properties<LanguageAvailability>(properties => properties
                .Keyword(key => key.Name(name => name.Id))
                .Boolean(boolean => boolean.Name(name => name.AvailableForInterface))
                .Boolean(boolean => boolean.Name(name => name.AvailableForAudio))
                .Boolean(boolean => boolean.Name(name => name.AvailableForSubtitles))
                .Nested<Data.Models.Language>(child => child.Name(name => name.Language)))
            .Properties<Data.Models.Language>(properties => properties
                .Keyword(key => key.Name(name => name.Id))
                .Text(text => text.Name(name => name.Name)))
            .Properties<SystemRequirements>(properties => properties
                .Keyword(key => key.Name(name => name.Id))
                .Text(text => text.Name(name => name.OS))
                .Text(text => text.Name(name => name.Processor))
                .Text(text => text.Name(name => name.Memory))
                .Text(text => text.Name(name => name.Graphics))
                .Text(text => text.Name(name => name.Network))
                .Text(text => text.Name(name => name.Storage))
                .Nested<Platform>(child => child.Name(name => name.Platform)))
            .Properties<Platform>(properties => properties
                .Keyword(key => key.Name(name => name.Id))
                .Text(text => text.Name(name => name.Name)));
        }
    }
}
