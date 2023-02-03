using GameStart.OrderingService.Core.Entities;

namespace GameStart.OrderingService.Infrastructure.Repositories
{
    public class ItemRepository : Repository<Item>
    {
        public ItemRepository(OrdersDbContext context) : base(context)
        {
        }
    }
}
