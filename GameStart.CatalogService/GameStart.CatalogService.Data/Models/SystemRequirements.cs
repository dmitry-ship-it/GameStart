using System.Text.Json.Serialization;

namespace GameStart.CatalogService.Data.Models
{
    public class SystemRequirements : IEntity
    {
        public Guid Id { get; set; }

        public string OS { get; set; }

        public string Processor { get; set; }

        public string Memory { get; set; }

        public string Graphics { get; set; }

        public string Network { get; set; }

        public string Storage { get; set; }

        public virtual Platform Platform { get; set; }

        [JsonIgnore]
        public virtual VideoGame VideoGame { get; set; }
    }
}
