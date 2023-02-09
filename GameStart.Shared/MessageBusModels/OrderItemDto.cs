namespace GameStart.Shared.MessageBusModels
{
    public class OrderItemMessageModel
    {
        public Guid GameId { get; set; }

        public string GameKey { get; set; }

        public DateTime PurchaseDateTime { get; set; }
    }
}
