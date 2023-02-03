using GameStart.Shared.Data;
using System.Text.Json.Serialization;

namespace GameStart.CatalogService.Data.Models
{
    public class LanguageAvailability : IEntity
    {
        public Guid Id { get; set; }

        public bool AvailableForInterface { get; set; }

        public bool AvailableForAudio { get; set; }

        public bool AvailableForSubtitles { get; set; }

        public virtual Language Language { get; set; }

        [JsonIgnore]
        public virtual ICollection<VideoGame> VideoGames { get; set; }
    }
}
