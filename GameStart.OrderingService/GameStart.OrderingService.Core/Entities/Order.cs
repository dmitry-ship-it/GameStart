using GameStart.OrderingService.Core.Abstractions;

namespace GameStart.OrderingService.Core.Entities
{
    public class Order : IEntity
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public DateTimeOffset DateTime { get; set; }

        public decimal TotalPrice { get; set; }

        public string State { get; set; }

        public virtual ICollection<Item> Items { get; set; }

        public virtual Address Address { get; set; }
    }
}
