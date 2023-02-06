using GameStart.CatalogService.Data.Models;
using GameStart.Shared.Data;

namespace GameStart.CatalogService.Data.Repositories
{
    public class PlatformRepository : RepositoryBase<Platform, CatalogDbContext>
    {
        public PlatformRepository(CatalogDbContext catalogDbContext)
            : base(catalogDbContext)
        {
        }
    }
}
