using System.Text.Json.Serialization;

namespace GameStart.CatalogService.Data.Models
{
    public class Platform : BaseModel
    {
        [JsonIgnore]
        public virtual ICollection<VideoGame> VideoGames { get; set; }

        public virtual ICollection<SystemRequirements> MinimalSystemRequirements { get; set; }

        public virtual ICollection<SystemRequirements> RecommendedSystemRequirements { get; set; }
    }
}
