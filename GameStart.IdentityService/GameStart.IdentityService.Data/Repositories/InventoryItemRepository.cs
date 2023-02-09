using GameStart.IdentityService.Data.Models;

namespace GameStart.IdentityService.Data.Repositories
{
    public class InventoryItemRepository : RepositoryBase<InventoryItem>
    {
        public InventoryItemRepository(AccountsDbContext context) : base(context)
        {
        }
    }
}
