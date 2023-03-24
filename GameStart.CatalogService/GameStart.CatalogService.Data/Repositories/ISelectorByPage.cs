using GameStart.CatalogService.Data.Models;

namespace GameStart.CatalogService.Data.Repositories
{
    public interface ISelectorByPage<T> where T : class, IEntity
    {
        Task<IEnumerable<T>> GetByPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    }
}
