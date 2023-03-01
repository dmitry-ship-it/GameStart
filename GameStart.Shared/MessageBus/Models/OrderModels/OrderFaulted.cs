namespace GameStart.Shared.MessageBus.Models.OrderModels
{
    public class OrderFaulted
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
    }
}
