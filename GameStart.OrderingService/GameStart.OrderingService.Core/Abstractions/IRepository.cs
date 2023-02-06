using System.Linq.Expressions;

namespace GameStart.OrderingService.Core.Abstractions
{
    public interface IRepository<T> where T : class, IEntity
    {
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<IEnumerable<T>> GetByConditionAsync(Expression<Func<T, bool>> expression,
            CancellationToken cancellationToken = default);

        Task CreateAsync(T entity, CancellationToken cancellationToken = default);

        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
    }
}
