namespace GameStart.Shared.MessageBus.Models.OrderModels
{
    public class OrderItem
    {
        public Guid Id { get; set; }

        public Guid GameId { get; set; }

        public string Title { get; set; }

        public string GameKey { get; set; }

        public bool IsPhysicalCopy { get; set; }

        public DateTimeOffset PurchaseDateTime { get; set; }
    }
}
