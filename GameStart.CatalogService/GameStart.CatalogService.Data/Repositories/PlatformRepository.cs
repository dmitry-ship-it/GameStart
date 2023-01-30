using GameStart.CatalogService.Data.Models;

namespace GameStart.CatalogService.Data.Repositories
{
    public class PlatformRepository : Repository<Platform>
    {
        public PlatformRepository(CatalogDbContext catalogDbContext) : base(catalogDbContext)
        {
        }
    }
}
