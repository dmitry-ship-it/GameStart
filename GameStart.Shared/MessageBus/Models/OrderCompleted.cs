﻿namespace GameStart.Shared.MessageBus.Models
{
    public class OrderCompleted
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public decimal TotalPrice { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
