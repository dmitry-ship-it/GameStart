using GameStart.OrderingService.Core.Abstractions;
using GameStart.OrderingService.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GameStart.OrderingService.Infrastructure.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(OrdersDbContext context) : base(context)
        {
        }

        public override async Task CreateAsync(Order entity, CancellationToken cancellationToken = default)
        {
            Context.Attach(entity).State = EntityState.Added;
            await Context.SaveChangesAsync(cancellationToken);
        }

        public override async Task DeleteAsync(Order entity, CancellationToken cancellationToken = default)
        {
            Context.Remove(entity);
            Context.RemoveRange(entity.Items);
            await Context.SaveChangesAsync(cancellationToken);
        }

        public override async Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await GetOrders().ToListAsync(cancellationToken);
        }

        public override async Task<IEnumerable<Order>> GetByConditionAsync(Expression<Func<Order, bool>> expression, CancellationToken cancellationToken = default)
        {
            return await GetOrders().Where(expression).ToListAsync(cancellationToken);
        }

        private IQueryable<Order> GetOrders()
        {
            return Context.Orders
                .Include(entity => entity.Items)
                .Include(entity => entity.Address)
                .AsNoTracking();
        }
    }
}
