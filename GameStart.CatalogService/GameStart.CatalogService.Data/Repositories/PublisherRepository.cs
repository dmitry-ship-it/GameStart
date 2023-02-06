using GameStart.CatalogService.Data.Models;
using GameStart.Shared.Data;

namespace GameStart.CatalogService.Data.Repositories
{
    public class PublisherRepository : RepositoryBase<Publisher, CatalogDbContext>
    {
        public PublisherRepository(CatalogDbContext catalogDbContext)
            : base(catalogDbContext)
        {
        }
    }
}
