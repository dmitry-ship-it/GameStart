namespace GameStart.Shared.MessageBusModels
{
    public class OrderCreatedMessageModel
    {
        public Guid UserId { get; set; }

        public ICollection<OrderItemMessageModel> OrderItems { get; set; }
    }
}
