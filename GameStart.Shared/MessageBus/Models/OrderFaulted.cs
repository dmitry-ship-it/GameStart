namespace GameStart.Shared.MessageBus.Models
{
    public class OrderFaulted
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
    }
}
