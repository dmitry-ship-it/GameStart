using GameStart.CatalogService.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GameStart.CatalogService.Data.Repositories
{
    public class VideoGameRepository : RepositoryBase<VideoGame, CatalogDbContext>, ISelectorByPage<VideoGame>, IGameCounter
    {
        public VideoGameRepository(CatalogDbContext catalogDbContext)
            : base(catalogDbContext)
        {
        }

        public override async Task CreateAsync(VideoGame entity, CancellationToken cancellationToken = default)
        {
            Context.Attach(entity).State = EntityState.Added;
            await Context.SaveChangesAsync(cancellationToken);
            Context.ChangeTracker.Clear();
        }

        public override async Task<IEnumerable<VideoGame>> FindAllAsync(bool includeGraph = true, CancellationToken cancellationToken = default)
        {
            return includeGraph
                ? await GetVideoGames().ToListAsync(cancellationToken)
                : await Context.VideoGames.AsNoTracking().ToListAsync(cancellationToken);
        }

        public override async Task<IEnumerable<VideoGame>> FindByConditionAsync(
            Expression<Func<VideoGame, bool>> expression,
            CancellationToken cancellationToken = default)
        {
            return await GetVideoGames().Where(expression)
                .ToListAsync(cancellationToken);
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

        public async Task<IEnumerable<VideoGame>> GetByPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            return await GetVideoGames().Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return await Context.VideoGames.CountAsync(cancellationToken);
        }

        private IQueryable<VideoGame> GetVideoGames()
        {
            return Context.VideoGames
                .Include(entity => entity.Genres)
                .Include(entity => entity.Developers)
                .Include(entity => entity.Publisher)
                .Include(entity => entity.LanguageAvailabilities)
                    .ThenInclude(entity => entity.Language)
                .Include(entity => entity.SystemRequirements)
                    .ThenInclude(requirements => requirements.Platform)
                .AsSplitQuery()
                .AsNoTracking();
        }
    }
}
