using GameStart.CatalogService.Data.Models;

namespace GameStart.CatalogService.Data.Repositories
{
    public class SystemRequirementsRepository : Repository<SystemRequirements>
    {
        public SystemRequirementsRepository(CatalogDbContext catalogDbContext) : base(catalogDbContext)
        {
        }
    }
}
