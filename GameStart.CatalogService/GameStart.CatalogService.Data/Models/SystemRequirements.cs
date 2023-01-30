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

        [JsonIgnore]
        public virtual ICollection<Platform> Platforms { get; set; }
    }
}
