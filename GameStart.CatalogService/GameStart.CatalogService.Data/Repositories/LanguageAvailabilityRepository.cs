﻿using GameStart.CatalogService.Data.Models;
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
            bool includeGraph = true,
            CancellationToken cancellationToken = default)
        {
            return includeGraph
                ? await GetLanguages().ToListAsync(cancellationToken)
                : await Context.LanguageAvailabilities.AsNoTracking().ToListAsync(cancellationToken);
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
