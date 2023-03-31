using GameStart.CatalogService.Data.Models;
using Nest;
using System.Collections;

namespace GameStart.CatalogService.Common.Tests.TestData
{
    public class TypeMappingDescriptorLanguageAvailabilityTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new Func<PropertiesDescriptor<LanguageAvailability>, IPromise<IProperties>>[] { property => property.Keyword(key => key.Name(name => name.Id)) };
            yield return new Func<PropertiesDescriptor<LanguageAvailability>, IPromise<IProperties>>[] { property => property.Boolean(boolean => boolean.Name(name => name.AvailableForInterface)) };
            yield return new Func<PropertiesDescriptor<LanguageAvailability>, IPromise<IProperties>>[] { property => property.Boolean(boolean => boolean.Name(name => name.AvailableForAudio)) };
            yield return new Func<PropertiesDescriptor<LanguageAvailability>, IPromise<IProperties>>[] { property => property.Boolean(boolean => boolean.Name(name => name.AvailableForSubtitles)) };
            yield return new Func<PropertiesDescriptor<LanguageAvailability>, IPromise<IProperties>>[] { property => property.Nested<Data.Models.Language>(child => child.Name(name => name.Language)) };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
