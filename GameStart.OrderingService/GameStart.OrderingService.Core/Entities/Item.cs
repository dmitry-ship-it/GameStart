using GameStart.OrderingService.Core.Abstractions;

namespace GameStart.OrderingService.Core.Entities
{
    public class Item : IEntity
    {
        public Guid Id { get; set; }

        public Guid GameId { get; set; }

        public bool IsPhysicalCopy { get; set; }

        public virtual Order Order { get; set; }
    }
}
