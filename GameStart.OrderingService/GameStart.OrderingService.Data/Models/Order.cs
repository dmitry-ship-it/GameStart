using GameStart.Shared.Data;

namespace GameStart.OrderingService.Data.Models
{
    public class Order : IEntity
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public DateTime DateTime { get; set; }

        public virtual ICollection<Item> Items { get; set; }

        public virtual Address Address { get; set; }
    }
}
