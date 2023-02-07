using GameStart.OrderingService.Core.Abstractions;
using System.Text.Json.Serialization;

namespace GameStart.OrderingService.Core.Entities
{
    public class Item : IEntity
    {
        public Guid Id { get; set; }

        public Guid GameId { get; set; }

        public bool IsPhysicalCopy { get; set; }

        [JsonIgnore]
        public virtual Order Order { get; set; }
    }
}
