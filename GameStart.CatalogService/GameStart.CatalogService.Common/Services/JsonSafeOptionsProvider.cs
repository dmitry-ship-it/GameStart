using GameStart.CatalogService.Data.EntityConfigurations.ValueConverters;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GameStart.CatalogService.Common.Services
{
    public class JsonSafeOptionsProvider : IJsonSafeOptionsProvider
    {
        public JsonSerializerOptions JsonSerializerOptions
        {
            get
            {
                var jsonOptions = new JsonSerializerOptions();
                jsonOptions.Converters.Add(new DateOnlyJsonConverter());
                jsonOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

                return jsonOptions;
            }
        }
    }
}
