using GameStart.CatalogService.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GameStart.CatalogService.Data.Repositories
{
    public class LanguageAvailabilityRepository : RepositoryBase<LanguageAvailability>
    {
        public LanguageAvailabilityRepository(CatalogDbContext catalogDbContext) : base(catalogDbContext)
        {
        }

        public override async Task<IEnumerable<LanguageAvailability>> FindAllAsync(CancellationToken cancellationToken = default)
        {
            return await GetLanguages()
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public override async Task<IEnumerable<LanguageAvailability>> FindByConditionAsync(Expression<Func<LanguageAvailability, bool>> expression, CancellationToken cancellationToken = default)
        {
            return await GetLanguages()
                .Where(expression)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        private IQueryable<LanguageAvailability> GetLanguages()
        {
            return CatalogDbContext.LanguageAvailabilities.Include(entity => entity.Language);
        }
    }
}
