using GameStart.OrderingService.Core.Abstractions;
using GameStart.OrderingService.Core.Entities;

namespace GameStart.OrderingService.Infrastructure.Repositories
{
    public class ItemRepository : Repository<Item>, IItemRepository
    {
        public ItemRepository(OrdersDbContext context) : base(context)
        {
        }
    }
}
