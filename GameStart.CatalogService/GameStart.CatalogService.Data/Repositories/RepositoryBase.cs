using GameStart.CatalogService.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GameStart.CatalogService.Data.Repositories
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : class, IEntity
    {
        protected CatalogDbContext CatalogDbContext { get; set; }

        protected RepositoryBase(CatalogDbContext catalogDbContext)
        {
            CatalogDbContext = catalogDbContext;
        }

        public virtual async Task CreateAsync(T entity, CancellationToken cancellationToken = default)
        {
            await CatalogDbContext.Set<T>().AddAsync(entity, cancellationToken);
            await CatalogDbContext.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            CatalogDbContext.Set<T>().Remove(entity);
            await CatalogDbContext.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task<IEnumerable<T>> FindAllAsync(CancellationToken cancellationToken = default)
        {
            return await CatalogDbContext.Set<T>()
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public virtual async Task<IEnumerable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
        {
            return await CatalogDbContext.Set<T>()
                .Where(expression)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            CatalogDbContext.Set<T>().Update(entity);
            await CatalogDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
