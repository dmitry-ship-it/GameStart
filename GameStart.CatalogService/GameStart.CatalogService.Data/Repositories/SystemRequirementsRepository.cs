using GameStart.CatalogService.Data.Models;

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
