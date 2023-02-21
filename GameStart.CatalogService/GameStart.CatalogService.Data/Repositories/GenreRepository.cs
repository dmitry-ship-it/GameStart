using GameStart.CatalogService.Data.Models;

namespace GameStart.CatalogService.Data.Repositories
{
    public class GenreRepository : RepositoryBase<Genre, CatalogDbContext>
    {
        public GenreRepository(CatalogDbContext catalogDbContext)
            : base(catalogDbContext)
        {
        }
    }
}
