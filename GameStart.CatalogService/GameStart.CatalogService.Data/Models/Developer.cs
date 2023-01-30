using System.Text.Json.Serialization;

namespace GameStart.CatalogService.Data.Models
{
    public class Developer : BaseModel
    {
        [JsonIgnore]
        public virtual ICollection<VideoGame> VideoGames { get; set; }
    }
}