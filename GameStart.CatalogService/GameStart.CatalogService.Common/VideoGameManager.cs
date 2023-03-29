using AutoMapper;
using GameStart.CatalogService.Common.Caching;
using GameStart.CatalogService.Common.Elasticsearch;
using GameStart.CatalogService.Common.Elasticsearch.Search;
using GameStart.CatalogService.Common.ViewModels;
using GameStart.CatalogService.Data.Models;
using GameStart.CatalogService.Data.Repositories;
using GameStart.Shared;

namespace GameStart.CatalogService.Common
{
    public class VideoGameManager : IVideoGameManager
    {
        private readonly string AllVideoGamesCacheKey = typeof(VideoGame).Name;

        private readonly IRepositoryWrapper repository;
        private readonly ISelectorByPage<VideoGame> selectorByPage;
        private readonly IGameCounter gameCounter;
        private readonly IMapper mapper;
        private readonly IRedisCacheService cache;
        private readonly IElasticsearchService<VideoGame, VideoGameSearchRequest> elasticsearch;

        public VideoGameManager(
            IRepositoryWrapper repository,
            ISelectorByPage<VideoGame> selectorByPage,
            IGameCounter gameCounter,
            IMapper mapper,
            IRedisCacheService cache,
            IElasticsearchService<VideoGame, VideoGameSearchRequest> elasticsearch)
        {
            this.repository = repository;
            this.selectorByPage = selectorByPage;
            this.gameCounter = gameCounter;
            this.mapper = mapper;
            this.cache = cache;
            this.elasticsearch = elasticsearch;
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

        public async Task<IEnumerable<VideoGame>> GetByPageAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            CheckPageAndItsSize(page, pageSize);

            var cacheKey = $"{AllVideoGamesCacheKey};{page};{pageSize}";
            var cached = await cache.GetAsync<IEnumerable<VideoGame>>(cacheKey, cancellationToken);

            if (cached is not null)
            {
                return cached;
            }

            var videoGames = await selectorByPage.GetByPageAsync(page, pageSize, cancellationToken);
            await cache.SetAsync(cacheKey, videoGames, cancellationToken);

            return videoGames;
        }

        public async Task AddAsync(VideoGameViewModel viewModel, CancellationToken cancellationToken = default)
        {
            var videoGame = mapper.Map<VideoGame>(viewModel);

            await repository.VideoGames.CreateAsync(videoGame, cancellationToken);

            CutCycles(videoGame);

            await elasticsearch.InsertAsync(videoGame, cancellationToken);
            await cache.SetAsync(videoGame.Id.ToString(), videoGame, cancellationToken);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var videoGame = await GetByIdAsync(id, cancellationToken);

            if (videoGame is null)
            {
                return false;
            }

            await cache.DeleteAsync(id.ToString(), cancellationToken);

            await repository.VideoGames.DeleteAsync(videoGame, cancellationToken);
            await elasticsearch.DeleteByIdAsync(videoGame, cancellationToken);

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
            await elasticsearch.UpdateAsync(videoGame, cancellationToken);

            await cache.SetAsync(id.ToString(), videoGame, cancellationToken);

            return true;
        }

        public async Task<IEnumerable<VideoGame>> SearchAsync(VideoGameSearchRequest request,
            CancellationToken cancellationToken = default)
        {
            return await elasticsearch.SearchAsync(request, cancellationToken);
        }

        public async Task<IEnumerable<Developer>> GetDevelopersAsync(CancellationToken cancellationToken = default)
        {
            return await repository.Developers.FindAllAsync(false, cancellationToken);
        }

        public async Task<IEnumerable<Genre>> GetGenresAsync(CancellationToken cancellationToken = default)
        {
            return await repository.Genres.FindAllAsync(false, cancellationToken);
        }

        public async Task<IEnumerable<Language>> GetLanguagesAsync(CancellationToken cancellationToken = default)
        {
            return await repository.Languages.FindAllAsync(false, cancellationToken);
        }

        public async Task<IEnumerable<Platform>> GetPlatformsAsync(CancellationToken cancellationToken = default)
        {
            return await repository.Platforms.FindAllAsync(false, cancellationToken);
        }

        public async Task<int> GetGamesCountAsync(CancellationToken cancellationToken = default)
        {
            return await gameCounter.CountAsync(cancellationToken);
        }

        private static void CheckPageAndItsSize(int page, int pageSize)
        {
            if (page < 1 || pageSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(page),
                    Constants.CatalogService.ExceptionMessages.InvalidPageOrItsSize);
            }
        }

        /// <summary>
        ///     This method removes possible serialization cycles.
        ///     Use it only for untracked entities.
        /// </summary>
        private static void CutCycles(VideoGame videoGame)
        {
            videoGame.Publisher.VideoGames = null;

            foreach (var developer in videoGame.Developers)
            {
                developer.VideoGames = null;
            }

            foreach (var genre in videoGame.Genres)
            {
                genre.VideoGames = null;
            }

            foreach (var availabilities in videoGame.LanguageAvailabilities)
            {
                availabilities.VideoGames = null;
                availabilities.Language.LanguageAvailabilities = null;
            }

            foreach (var requirements in videoGame.SystemRequirements)
            {
                requirements.VideoGame = null;
                requirements.Platform.SystemRequirements = null;
            }
        }
    }
}
