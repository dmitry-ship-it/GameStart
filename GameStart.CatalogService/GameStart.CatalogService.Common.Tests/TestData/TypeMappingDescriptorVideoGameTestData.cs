using GameStart.CatalogService.Data.Models;
using Nest;
using System.Collections;

namespace GameStart.CatalogService.Common.Tests.TestData
{
    public class TypeMappingDescriptorVideoGameTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new Func<PropertiesDescriptor<VideoGame>, IPromise<IProperties>>[] { property => property.Keyword(key => key.Name(name => name.Id)) };
            yield return new Func<PropertiesDescriptor<VideoGame>, IPromise<IProperties>>[] { property => property.Text(text => text.Name(name => name.Title)) };
            yield return new Func<PropertiesDescriptor<VideoGame>, IPromise<IProperties>>[] { property => property.Text(text => text.Name(name => name.Description)) };
            yield return new Func<PropertiesDescriptor<VideoGame>, IPromise<IProperties>>[] { property => property.Text(text => text.Name(name => name.Copyright)) };
            yield return new Func<PropertiesDescriptor<VideoGame>, IPromise<IProperties>>[] { property => property.Date(date => date.Name(name => name.ReleaseDate)) };
            yield return new Func<PropertiesDescriptor<VideoGame>, IPromise<IProperties>>[] { property => property.Number(number => number.Name(name => name.Price).Type(NumberType.Double)) };
            yield return new Func<PropertiesDescriptor<VideoGame>, IPromise<IProperties>>[] { property => property.Object<Publisher>(child => child.Name(name => name.Publisher)) };
            yield return new Func<PropertiesDescriptor<VideoGame>, IPromise<IProperties>>[] { property => property.Nested<Developer>(child => child.Name(name => name.Developers)) };
            yield return new Func<PropertiesDescriptor<VideoGame>, IPromise<IProperties>>[] { property => property.Nested<Genre>(child => child.Name(name => name.Genres)) };
            yield return new Func<PropertiesDescriptor<VideoGame>, IPromise<IProperties>>[] { property => property.Nested<LanguageAvailability>(child => child.Name(name => name.LanguageAvailabilities)) };
            yield return new Func<PropertiesDescriptor<VideoGame>, IPromise<IProperties>>[] { property => property.Nested<SystemRequirements>(child => child.Name(name => name.SystemRequirements)) };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
