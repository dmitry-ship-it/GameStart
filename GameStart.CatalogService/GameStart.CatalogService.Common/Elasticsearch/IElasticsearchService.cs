using GameStart.CatalogService.Common.Elasticsearch.Search;
using GameStart.CatalogService.Data.Models;

namespace GameStart.CatalogService.Common.Elasticsearch
{
    public interface IElasticsearchService<TEntity, TRequest>
        where TEntity : class, IEntity
        where TRequest : ISearchRequest
    {
        string IndexName { get; }

        Task CheckIndexAsync(CancellationToken cancellationToken = default);

        Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task InsertListAsync(IList<TEntity> entities, CancellationToken cancellationToken = default);

        Task DeleteIndexAsync(CancellationToken cancellationToken = default);

        Task DeleteByIdAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task<IEnumerable<TEntity>> SearchAsync(TRequest request, CancellationToken cancellationToken);
    }
}
