﻿using GameStart.CatalogService.Data.Models;
using GameStart.Shared.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GameStart.CatalogService.Data.Repositories
{
    public class VideoGameRepository : RepositoryBase<VideoGame, CatalogDbContext>
    {
        public VideoGameRepository(CatalogDbContext catalogDbContext)
            : base(catalogDbContext)
        {
        }

        public override async Task CreateAsync(VideoGame entity, CancellationToken cancellationToken = default)
        {
            Context.Attach(entity).State = EntityState.Added;
            await Context.SaveChangesAsync(cancellationToken);
        }

        public override async Task<IEnumerable<VideoGame>> FindAllAsync(CancellationToken cancellationToken = default)
        {
            return await GetVideoGames().AsNoTrackingWithIdentityResolution().ToListAsync(cancellationToken);
        }

        public override async Task<IEnumerable<VideoGame>> FindByConditionAsync(
            Expression<Func<VideoGame, bool>> expression,
            CancellationToken cancellationToken = default)
        {
            return await GetVideoGames().Where(expression)
                .AsNoTrackingWithIdentityResolution().ToListAsync(cancellationToken);
        }

        public override async Task DeleteAsync(VideoGame entity, CancellationToken cancellationToken = default)
        {
            Context.Remove(entity);
            Context.RemoveRange(entity.LanguageAvailabilities);
            Context.RemoveRange(entity.SystemRequirements);

            await Context.SaveChangesAsync(cancellationToken);
        }

        public override async Task UpdateAsync(VideoGame entity, CancellationToken cancellationToken = default)
        {
            Context.Attach(entity).State = EntityState.Modified;
            await Context.SaveChangesAsync(cancellationToken);
        }

        private IQueryable<VideoGame> GetVideoGames()
        {
            return Context.VideoGames
                .Include(entity => entity.Ganres)
                .Include(entity => entity.Developers)
                .Include(entity => entity.Publisher)
                .Include(entity => entity.LanguageAvailabilities)
                    .ThenInclude(entity => entity.Language)
                .Include(entity => entity.SystemRequirements)
                    .ThenInclude(requirements => requirements.Platform)
                .AsSplitQuery();
        }
    }
}
