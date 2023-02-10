namespace GameStart.Shared.MessageBus.Models
{
    public class OrderCreated : IMessageBusMessage
    {
        public Guid UserId { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
