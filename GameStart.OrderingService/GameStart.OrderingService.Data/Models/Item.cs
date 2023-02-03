using GameStart.Shared.Data;

namespace GameStart.OrderingService.Data.Models
{
    public class Item : IEntity
    {
        public Guid Id { get; set; }

        public Guid GameId { get; set; }

        public bool IsPhysicalCopy { get; set; }
    }
}
