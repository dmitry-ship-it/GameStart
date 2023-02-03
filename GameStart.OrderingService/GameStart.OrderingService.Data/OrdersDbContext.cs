using GameStart.OrderingService.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStart.OrderingService.Data
{
    public class OrdersDbContext : DbContext
    {
        public OrdersDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<Address> Addresses { get; set; }
    }
}
