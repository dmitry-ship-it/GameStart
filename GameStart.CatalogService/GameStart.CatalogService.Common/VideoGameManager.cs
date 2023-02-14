using AutoMapper;
using GameStart.CatalogService.Common.Caching;
using GameStart.CatalogService.Common.ViewModels;
using GameStart.CatalogService.Data.Models;
using GameStart.CatalogService.Data.Repositories;

namespace GameStart.CatalogService.Common
{
    public class VideoGameManager
    {
        private readonly string AllVideoGamesCacheKey = typeof(VideoGame).Name;

        private readonly IRepositoryWrapper repository;
        private readonly IMapper mapper;
        private readonly IRedisCacheService cache;

        public VideoGameManager(IRepositoryWrapper repository, IMapper mapper, IRedisCacheService cache)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.cache = cache;
        }

        public async Task<VideoGame> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var cached = await cache.GetAsync<VideoGame>(id.ToString(), cancellationToken);

            if (cached is not null)
            {
                return cached;
            }

            var found = (await repository.VideoGames.FindByConditionAsync(
                videoGame => videoGame.Id == id, cancellationToken)).FirstOrDefault();

            if (found is not null)
            {
                await cache.SetAsync(found.Id.ToString(), found, cancellationToken);
            }

            return found;
        }

        public async Task<IEnumerable<VideoGame>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var cached = await cache.GetAsync<IEnumerable<VideoGame>>(AllVideoGamesCacheKey, cancellationToken);

            if (cached is not null)
            {
                return cached;
            }

            var videoGames = await repository.VideoGames.FindAllAsync(cancellationToken: cancellationToken);

            if (videoGames?.Any() == true)
            {
                await cache.SetAsync(AllVideoGamesCacheKey, videoGames, cancellationToken);
            }

            return videoGames;
        }

        public async Task AddAsync(VideoGameViewModel viewModel, CancellationToken cancellationToken = default)
        {
            var videoGame = mapper.Map<VideoGame>(viewModel);
            await repository.VideoGames.CreateAsync(videoGame, cancellationToken);
            await cache.DeleteAsync(AllVideoGamesCacheKey, cancellationToken);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var videoGame = await GetByIdAsync(id, cancellationToken);

            if (videoGame is null)
            {
                return false;
            }

            await cache.DeleteAsync(id.ToString(), cancellationToken);
            await cache.DeleteAsync(AllVideoGamesCacheKey, cancellationToken);

            await repository.VideoGames.DeleteAsync(videoGame, cancellationToken);

            return true;
        }

        public async Task<bool> UpdateAsync(Guid id, VideoGameViewModel model,
            CancellationToken cancellationToken = default)
        {
            var videoGame = await GetByIdAsync(id, cancellationToken);

            if (videoGame is null)
            {
                return false;
            }

            mapper.Map(model, videoGame);

            await repository.VideoGames.UpdateAsync(videoGame, cancellationToken);

            await cache.SetAsync(id.ToString(), videoGame, cancellationToken);
            await cache.DeleteAsync(AllVideoGamesCacheKey, cancellationToken);

            return true;
        }
    }
}
