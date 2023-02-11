﻿namespace GameStart.Shared.MessageBus.Models
{
    public class OrderSubmitted
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
