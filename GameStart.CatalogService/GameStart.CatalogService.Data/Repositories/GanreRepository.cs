using GameStart.CatalogService.Data.Models;

namespace GameStart.CatalogService.Data.Repositories
{
    public class GanreRepository : RepositoryBase<Ganre>
    {
        public GanreRepository(CatalogDbContext catalogDbContext) : base(catalogDbContext)
        {
        }
    }
}
