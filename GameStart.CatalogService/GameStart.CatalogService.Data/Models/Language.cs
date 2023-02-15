using System.Text.Json.Serialization;

namespace GameStart.CatalogService.Data.Models
{
    public class Language : BaseModel
    {
        [JsonIgnore]
        public virtual ICollection<LanguageAvailability> LanguageAvailabilities { get; set; }
    }
}
