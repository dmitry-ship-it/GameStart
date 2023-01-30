using GameStart.CatalogService.Data.Models;

namespace GameStart.CatalogService.Data.Repositories
{
    public class GanreRepository : Repository<Ganre>
    {
        public GanreRepository(CatalogDbContext catalogDbContext) : base(catalogDbContext)
        {
        }
    }
}
