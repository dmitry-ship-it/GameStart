using GameStart.CatalogService.Data.Models;

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
