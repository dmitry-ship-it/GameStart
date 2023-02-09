namespace GameStart.IdentityService.Data.Models
{
    public class InventoryItem : IEntity
    {
        public Guid Id { get; set; }

        public Guid GameId { get; set; }

        public string GameTitle { get; set; }

        public string GameKey { get; set; }

        public DateTime PurchaseDateTime { get; set; }

        public virtual User User { get; set; }
    }
}
