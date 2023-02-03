using GameStart.OrderingService.Core.Entities;

namespace GameStart.OrderingService.Infrastructure.Repositories
{
    public class AddressRepository : Repository<Address>
    {
        public AddressRepository(OrdersDbContext context) : base(context)
        {
        }
    }
}
