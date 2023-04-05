using GameStart.IdentityService.Data.Models;
using GameStart.IdentityService.Data.Repositories;
using GameStart.Shared.Extensions;
using System.Security.Claims;

namespace GameStart.IdentityService.Common
{
    public class InventoryManager
    {
        private readonly IRepository<InventoryItem> repository;

        public InventoryManager(IRepository<InventoryItem> repository)
        {
            this.repository = repository;
        }

        public virtual async Task<IEnumerable<InventoryItem>> GetAllAsync(
            IEnumerable<Claim> claims,
            CancellationToken cancellationToken = default)
        {
            var userId = claims.GetUserId();

            return await repository.FindByConditionAsync(entity => entity.User.Id == userId, cancellationToken);
        }

        public virtual async Task<InventoryItem> GetByGameIdAsync(Guid gameId,
            IEnumerable<Claim> claims,
            CancellationToken cancellationToken = default)
        {
            var userId = claims.GetUserId();

            return (await repository.FindByConditionAsync(
                entity => entity.GameId == gameId && entity.User.Id == userId,
                cancellationToken)).FirstOrDefault();
        }

        public virtual async Task<bool> DeleteGameByUserAsync(Guid gameId,
            IEnumerable<Claim> claims,
            CancellationToken cancellationToken = default)
        {
            var userId = claims.GetUserId();

            var game = (await repository.FindByConditionAsync(
                entity => entity.GameId == gameId && entity.User.Id == userId,
                cancellationToken)).FirstOrDefault();

            if (game is null)
            {
                return false;
            }

            await repository.DeleteAsync(game, cancellationToken);

            return true;
        }
    }
}
