using GameStart.CatalogService.Data.Models;

namespace GameStart.CatalogService.Data.Repositories
{
    public class LanguageRepository : RepositoryBase<Language>
    {
        public LanguageRepository(CatalogDbContext catalogDbContext) : base(catalogDbContext)
        {
        }
    }
}
