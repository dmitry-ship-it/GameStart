using GameStart.CatalogService.Data.Models;
using Nest;
using System.Collections;

namespace GameStart.CatalogService.Common.Tests.TestData
{
    public class TypeMappingDescriptorSystemRequirementsTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new Func<PropertiesDescriptor<SystemRequirements>, IPromise<IProperties>>[] { property => property.Keyword(key => key.Name(name => name.Id)) };
            yield return new Func<PropertiesDescriptor<SystemRequirements>, IPromise<IProperties>>[] { property => property.Text(text => text.Name(name => name.OS)) };
            yield return new Func<PropertiesDescriptor<SystemRequirements>, IPromise<IProperties>>[] { property => property.Text(text => text.Name(name => name.Processor)) };
            yield return new Func<PropertiesDescriptor<SystemRequirements>, IPromise<IProperties>>[] { property => property.Text(text => text.Name(name => name.Memory)) };
            yield return new Func<PropertiesDescriptor<SystemRequirements>, IPromise<IProperties>>[] { property => property.Text(text => text.Name(name => name.Graphics)) };
            yield return new Func<PropertiesDescriptor<SystemRequirements>, IPromise<IProperties>>[] { property => property.Text(text => text.Name(name => name.Network)) };
            yield return new Func<PropertiesDescriptor<SystemRequirements>, IPromise<IProperties>>[] { property => property.Text(text => text.Name(name => name.Storage)) };
            yield return new Func<PropertiesDescriptor<SystemRequirements>, IPromise<IProperties>>[] { property => property.Nested<Platform>(child => child.Name(name => name.Platform)) };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
