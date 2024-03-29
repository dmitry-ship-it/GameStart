﻿using GameStart.IdentityService.Data.Models;
using System.Linq.Expressions;

namespace GameStart.IdentityService.Data.Repositories
{
    public interface IRepository<T> where T : class, IEntity
    {
        Task<IEnumerable<T>> FindAllAsync(CancellationToken cancellationToken = default);

        Task<IEnumerable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression,
            CancellationToken cancellationToken = default);

        Task CreateAsync(T entity, CancellationToken cancellationToken = default);

        Task CreateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
    }
}
