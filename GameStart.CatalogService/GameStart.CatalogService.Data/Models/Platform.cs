using GameStart.Shared.Data;
using System.Text.Json.Serialization;

namespace GameStart.CatalogService.Data.Models
{
    public class Platform : BaseModel
    {
        [JsonIgnore]
        public virtual ICollection<SystemRequirements> SystemRequirements { get; set; }
    }
}
