using GameStart.IdentityService.Data.Models;
using GameStart.IdentityService.Data.Repositories;
using System.Security.Claims;
using System.Security.Principal;

namespace GameStart.IdentityService.Common
{
    public class InventoryManager
    {
        private readonly IRepository<InventoryItem> repository;

        public InventoryManager(IRepository<InventoryItem> repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<InventoryItem>> GetAllAsync(
            IEnumerable<Claim> claims,
            CancellationToken cancellationToken = default)
        {
            var userId = GetUserIdFromClaims(claims);

            return await repository.FindByConditionAsync(entity => entity.User.Id == userId, cancellationToken);
        }

        public async Task<InventoryItem> GetByGameIdAsync(Guid gameId,
            IEnumerable<Claim> claims,
            CancellationToken cancellationToken = default)
        {
            var userId = GetUserIdFromClaims(claims);

            return (await repository.FindByConditionAsync(
                entity => entity.GameId == gameId && entity.User.Id == userId,
                cancellationToken)).FirstOrDefault();
        }

        private static Guid GetUserIdFromClaims(IEnumerable<Claim> claims)
        {
            var found = claims.First(claim => claim.Type == ClaimTypes.NameIdentifier);

            return Guid.Parse(found.Value);
        }
    }
}
