using GameStart.OrderingService.Core.Abstractions;
using GameStart.OrderingService.Core.Entities;

namespace GameStart.OrderingService.Infrastructure.Repositories
{
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        public AddressRepository(OrdersDbContext context) : base(context)
        {
        }
    }
}
