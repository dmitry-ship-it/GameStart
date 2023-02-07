using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GameStart.Shared.Data
{
    public abstract class RepositoryBase<TEntity, TContext> : IRepository<TEntity>
        where TEntity : class, IEntity
        where TContext : DbContext
    {
        protected TContext Context { get; set; }

        protected RepositoryBase(TContext context)
        {
            Context = context;
        }

        public virtual async Task CreateAsync(TEntity entity,
            CancellationToken cancellationToken = default)
        {
            await Context.Set<TEntity>().AddAsync(entity, cancellationToken);
            await Context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task DeleteAsync(TEntity entity,
            CancellationToken cancellationToken = default)
        {
            Context.Set<TEntity>().Remove(entity);
            await Context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task<IEnumerable<TEntity>> FindAllAsync(
            CancellationToken cancellationToken = default)
        {
            return await Context.Set<TEntity>()
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public virtual async Task<IEnumerable<TEntity>> FindByConditionAsync(
            Expression<Func<TEntity, bool>> expression,
            CancellationToken cancellationToken = default)
        {
            return await Context.Set<TEntity>()
                .Where(expression)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public virtual async Task UpdateAsync(TEntity entity,
            CancellationToken cancellationToken = default)
        {
            Context.Set<TEntity>().Update(entity);
            await Context.SaveChangesAsync(cancellationToken);
        }
    }
}
