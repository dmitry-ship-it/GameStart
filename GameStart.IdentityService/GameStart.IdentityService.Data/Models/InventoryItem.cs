using System.Text.Json.Serialization;

namespace GameStart.IdentityService.Data.Models
{
    public class InventoryItem : IEntity
    {
        public Guid Id { get; set; }

        public Guid GameId { get; set; }

        public string GameKey { get; set; }

        public DateTime PurchaseDateTime { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }
    }
}
