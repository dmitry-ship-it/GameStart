using GameStart.CatalogService.Data.Models;
using GameStart.Shared.Data;

namespace GameStart.CatalogService.Data.Repositories
{
    public class GanreRepository : RepositoryBase<Ganre, CatalogDbContext>
    {
        public GanreRepository(CatalogDbContext catalogDbContext)
            : base(catalogDbContext)
        {
        }
    }
}
