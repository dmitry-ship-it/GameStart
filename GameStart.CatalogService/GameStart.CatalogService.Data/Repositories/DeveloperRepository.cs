using GameStart.CatalogService.Data.Models;
using GameStart.Shared.Data;

namespace GameStart.CatalogService.Data.Repositories
{
    public class DeveloperRepository : RepositoryBase<Developer, CatalogDbContext>
    {
        public DeveloperRepository(CatalogDbContext catalogDbContext)
            : base(catalogDbContext)
        {
        }
    }
}
