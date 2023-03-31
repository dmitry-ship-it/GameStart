using Nest;
using System.Collections;

namespace GameStart.CatalogService.Common.Tests.TestData
{
    public class TypeMappingDescriptorLanguageTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new Func<PropertiesDescriptor<Data.Models.Language>, IPromise<IProperties>>[] { property => property.Keyword(key => key.Name(name => name.Id)) };
            yield return new Func<PropertiesDescriptor<Data.Models.Language>, IPromise<IProperties>>[] { property => property.Text(text => text.Name(name => name.Name)) };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
