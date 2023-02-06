using GameStart.Shared.Data;
using System.Text.Json.Serialization;

namespace GameStart.CatalogService.Data.Models
{
    public class Publisher : BaseModel
    {
        [JsonIgnore]
        public virtual ICollection<VideoGame> VideoGames { get; set; }
    }
}
