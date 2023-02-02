using GameStart.CatalogService.Data.Models;

namespace GameStart.CatalogService.Data.Repositories
{
    public class DeveloperRepository : RepositoryBase<Developer>
    {
        public DeveloperRepository(CatalogDbContext catalogDbContext) : base(catalogDbContext)
        {
        }
    }
}
