using GameStart.IdentityService.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GameStart.IdentityService.Data.Repositories
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : class, IEntity
    {
        protected AccountsDbContext Context { get; }

        protected RepositoryBase(AccountsDbContext context)
        {
            Context = context;
        }

        public async Task CreateAsync(T entity, CancellationToken cancellationToken = default)
        {
            Context.Set<T>().Add(entity);
            await Context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            Context.Set<T>().Remove(entity);
            await Context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<T>> FindAllAsync(CancellationToken cancellationToken = default)
        {
            return await Context.Set<T>().AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression,
            CancellationToken cancellationToken = default)
        {
            return await Context.Set<T>().Where(expression).AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            Context.Set<T>().Update(entity);
            await Context.SaveChangesAsync(cancellationToken);
        }
    }
}
