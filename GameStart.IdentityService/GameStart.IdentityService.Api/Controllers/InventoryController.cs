using GameStart.IdentityService.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameStart.IdentityService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryManager manager;

        public InventoryController(InventoryManager manager)
        {
            this.manager = manager;
        }

        [Authorize]
        [HttpGet("{gameId:Guid}")]
        public async Task<IActionResult> GetItemAsync([FromRoute] Guid gameId,
            CancellationToken cancellationToken = default)
        {
            var result = await manager.GetByGameIdAsync(gameId,
                HttpContext.User.Claims, cancellationToken);

            return result is null ? NotFound() : Ok(result);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return Ok(await manager.GetByUserClaimsAsync(HttpContext.User.Claims, cancellationToken));
        }

        [Authorize]
        [HttpDelete("{gameId:Guid}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid gameId,
            CancellationToken cancellationToken = default)
        {
            var isDeleted = await manager.DeleteGameByUserClaimsAsync(gameId, HttpContext.User.Claims, cancellationToken);

            return isDeleted ? NoContent() : NotFound();
        }
    }
}
