using GameStart.OrderingService.Core.Abstractions;
using System.Text.Json.Serialization;

namespace GameStart.OrderingService.Core.Entities
{
    public class Address : IEntity
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string Сountry { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string House { get; set; }

        public string Flat { get; set; }

        public string PostCode { get; set; }

        [JsonIgnore]
        public virtual ICollection<Order> Orders { get; set; }
    }
}
