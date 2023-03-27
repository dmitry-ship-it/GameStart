using GameStart.OrderingService.Core.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GameStart.OrderingService.Infrastructure.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : class, IEntity
    {
        protected OrdersDbContext Context { get; set; }

        protected Repository(OrdersDbContext context)
        {
            Context = context;
        }

        public virtual async Task CreateAsync(T entity,
            CancellationToken cancellationToken = default)
        {
            await Context.Set<T>().AddAsync(entity, cancellationToken);
            await Context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task DeleteAsync(T entity,
            CancellationToken cancellationToken = default)
        {
            Context.Set<T>().Remove(entity);
            await Context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            return await Context.Set<T>()
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public virtual async Task<IEnumerable<T>> GetByConditionAsync(
            Expression<Func<T, bool>> expression,
            CancellationToken cancellationToken = default)
        {
            return await Context.Set<T>()
                .Where(expression)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public virtual async Task UpdateAsync(T entity,
            CancellationToken cancellationToken = default)
        {
            Context.Set<T>().Attach(entity).State = EntityState.Modified;
            await Context.SaveChangesAsync(cancellationToken);
        }
    }
}
