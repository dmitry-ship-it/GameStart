using GameStart.CatalogService.Data.Models;
using System.Linq.Expressions;

namespace GameStart.CatalogService.Data.Repositories
{
    public interface IRepository<T> where T : class, IEntity
    {
        Task<IEnumerable<T>> FindAllAsync(bool includeGraph = true, CancellationToken cancellationToken = default);

        Task<IEnumerable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression,
            CancellationToken cancellationToken = default);

        Task CreateAsync(T entity, CancellationToken cancellationToken = default);

        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
    }
}
