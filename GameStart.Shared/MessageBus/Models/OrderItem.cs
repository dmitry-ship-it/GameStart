namespace GameStart.Shared.MessageBus.Models
{
    public class OrderItem : IMessageBusMessage
    {
        public Guid GameId { get; set; }

        public string GameKey { get; set; }

        public DateTime PurchaseDateTime { get; set; }
    }
}
