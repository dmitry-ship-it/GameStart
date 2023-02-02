using GameStart.CatalogService.Common;
using GameStart.CatalogService.Common.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameStart.CatalogService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly VideoGameManager manager;

        public CatalogController(VideoGameManager manager)
        {
            this.manager = manager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return Ok(await manager.GetAllAsync(cancellationToken));
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            var result = await manager.GetByIdAsync(id, cancellationToken);

            return result is null ? NotFound() : Ok(result);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] VideoGameViewModel model, CancellationToken cancellationToken = default)
        {
            await manager.AddAsync(model, cancellationToken);

            return Ok();
        }

        [Authorize]
        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] VideoGameViewModel viewModel, CancellationToken cancellationToken = default)
        {
            var isUpdated = await manager.UpdateAsync(id, viewModel, cancellationToken);

            return isUpdated ? Ok() : NotFound();
        }

        [Authorize]
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            var isDeleted = await manager.DeleteAsync(id, cancellationToken);

            return isDeleted ? NoContent() : NotFound();
        }
    }
}
