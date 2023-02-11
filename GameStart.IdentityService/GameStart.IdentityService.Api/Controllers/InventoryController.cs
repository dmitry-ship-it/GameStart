using GameStart.IdentityService.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameStart.IdentityService.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryManager manager;

        public InventoryController(InventoryManager manager)
        {
            this.manager = manager;
        }

        [HttpGet("{gameId:Guid}")]
        public async Task<IActionResult> GetItemAsync([FromRoute] Guid gameId,
            CancellationToken cancellationToken = default)
        {
            var result = await manager.GetByGameIdAsync(gameId,
                HttpContext.User.Claims, cancellationToken);

            return result is null ? NotFound() : Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return Ok(await manager.GetAllAsync(HttpContext.User.Claims, cancellationToken));
        }

        [HttpDelete("{gameId:Guid}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid gameId,
            CancellationToken cancellationToken = default)
        {
            var isDeleted = await manager.DeleteGameByUser(gameId, HttpContext.User.Claims, cancellationToken);

            return isDeleted ? NoContent() : NotFound();
        }
    }
}
