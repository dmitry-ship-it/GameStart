using GameStart.CatalogService.Data.Models;

namespace GameStart.CatalogService.Data.Repositories
{
    public class PublisherRepository : RepositoryBase<Publisher>
    {
        public PublisherRepository(CatalogDbContext catalogDbContext) : base(catalogDbContext)
        {
        }
    }
}
