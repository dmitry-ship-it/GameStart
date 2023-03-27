namespace GameStart.Shared.MessageBus.Models.OrderModels
{
    public class OrderItem
    {
        public Guid GameId { get; set; }

        public string Title { get; set; }

        public string GameKey { get; set; }

        public DateTimeOffset PurchaseDateTime { get; set; }
    }
}
