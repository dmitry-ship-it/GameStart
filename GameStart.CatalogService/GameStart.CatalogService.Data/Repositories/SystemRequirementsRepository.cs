using GameStart.CatalogService.Data.Models;
using GameStart.Shared.Data;

namespace GameStart.CatalogService.Data.Repositories
{
    public class SystemRequirementsRepository : RepositoryBase<SystemRequirements, CatalogDbContext>
    {
        public SystemRequirementsRepository(CatalogDbContext catalogDbContext)
            : base(catalogDbContext)
        {
        }
    }
}
