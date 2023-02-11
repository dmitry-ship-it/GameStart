using AutoMapper;
using GameStart.CatalogService.Common.ViewModels;
using GameStart.CatalogService.Data.Models;
using GameStart.CatalogService.Data.Repositories;

namespace GameStart.CatalogService.Common
{
    public class VideoGameManager
    {
        private readonly IRepositoryWrapper repository;
        private readonly IMapper mapper;

        public VideoGameManager(IRepositoryWrapper repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<VideoGame> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var found = await repository.VideoGames.FindByConditionAsync(
                videoGame => videoGame.Id == id, cancellationToken);

            return found.FirstOrDefault();
        }

        public async Task<IEnumerable<VideoGame>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await repository.VideoGames.FindAllAsync(cancellationToken: cancellationToken);
        }

        public async Task AddAsync(VideoGameViewModel viewModel, CancellationToken cancellationToken = default)
        {
            var videoGame = mapper.Map<VideoGame>(viewModel);
            await repository.VideoGames.CreateAsync(videoGame, cancellationToken);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var videoGame = await GetByIdAsync(id, cancellationToken);

            if (videoGame is null)
            {
                return false;
            }

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

            return true;
        }
    }
}
