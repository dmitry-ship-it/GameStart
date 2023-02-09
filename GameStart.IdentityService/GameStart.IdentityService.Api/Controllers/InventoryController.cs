using GameStart.IdentityService.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Principal;

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
    }
}
