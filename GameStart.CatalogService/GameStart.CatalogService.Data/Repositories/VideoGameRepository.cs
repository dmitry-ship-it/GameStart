using GameStart.CatalogService.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GameStart.CatalogService.Data.Repositories
{
    public class VideoGameRepository : Repository<VideoGame>
    {
        public VideoGameRepository(CatalogDbContext catalogDbContext) : base(catalogDbContext)
        {
        }

        public override async Task<IEnumerable<VideoGame>> FindAllAsync(CancellationToken cancellationToken = default)
        {
            return await GetVideoGames().AsNoTracking().ToListAsync(cancellationToken);
        }

        public override async Task<IEnumerable<VideoGame>> FindByConditionAsync(Expression<Func<VideoGame, bool>> expression, CancellationToken cancellationToken = default)
        {
            return await GetVideoGames().Where(expression).AsNoTracking().ToListAsync(cancellationToken);
        }

        private IQueryable<VideoGame> GetVideoGames()
        {
            return CatalogDbContext.VideoGames
                .Include(entity => entity.Ganres)
                .Include(entity => entity.Developers)
                .Include(entity => entity.Publisher)
                .Include(entity => entity.InterfaceLanguages)
                .Include(entity => entity.AudioLanguages)
                .Include(entity => entity.SubtitlesLanguages)
                .Include(entity => entity.Platforms)
                    .ThenInclude(platform => platform.SystemRequirements);
        }
    }
}
