using GameStart.CatalogService.Data.Models;
using GameStart.Shared.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GameStart.CatalogService.Data.Repositories
{
    public class LanguageAvailabilityRepository : RepositoryBase<LanguageAvailability, CatalogDbContext>
    {
        public LanguageAvailabilityRepository(CatalogDbContext catalogDbContext)
            : base(catalogDbContext)
        {
        }

        public override async Task<IEnumerable<LanguageAvailability>> FindAllAsync(
            CancellationToken cancellationToken = default)
        {
            return await GetLanguages()
                .ToListAsync(cancellationToken);
        }

        public override async Task<IEnumerable<LanguageAvailability>> FindByConditionAsync(
            Expression<Func<LanguageAvailability, bool>> expression,
            CancellationToken cancellationToken = default)
        {
            return await GetLanguages()
                .Where(expression)
                .ToListAsync(cancellationToken);
        }

        private IQueryable<LanguageAvailability> GetLanguages()
        {
            return Context.LanguageAvailabilities
                .Include(entity => entity.Language)
                .AsNoTracking();
        }
    }
}
