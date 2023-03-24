using System.Text.Json;

namespace GameStart.CatalogService.Common.Services
{
    public interface IJsonSafeOptionsProvider
    {
        JsonSerializerOptions JsonSerializerOptions { get; }
    }
}
