using Microsoft.EntityFrameworkCore;

namespace GameStart.OrderingService.Data
{
    public class OrdersDbContext : DbContext
    {
        public OrdersDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
