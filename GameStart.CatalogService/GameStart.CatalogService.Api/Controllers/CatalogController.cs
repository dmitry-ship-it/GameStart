using GameStart.CatalogService.Common;
using GameStart.CatalogService.Common.Elasticsearch.Search;
using GameStart.CatalogService.Common.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameStart.CatalogService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly IVideoGameManager manager;

        public CatalogController(IVideoGameManager manager)
        {
            this.manager = manager;
        }

        [HttpGet]
        public async Task<IActionResult> GetByPageAsync(
            [FromQuery] int page,
            [FromQuery] int pageSize,
            CancellationToken cancellationToken = default)
        {
            return Ok(await manager.GetByPageAsync(page, pageSize, cancellationToken));
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id,
            CancellationToken cancellationToken = default)
        {
            var result = await manager.GetByIdAsync(id, cancellationToken);

            return result is null ? NotFound() : Ok(result);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] VideoGameViewModel model,
            CancellationToken cancellationToken = default)
        {
            await manager.AddAsync(model, cancellationToken);

            return Ok();
        }

        [Authorize]
        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id,
            [FromBody] VideoGameViewModel viewModel,
            CancellationToken cancellationToken = default)
        {
            var isUpdated = await manager.UpdateAsync(id, viewModel, cancellationToken);

            return isUpdated ? Ok() : NotFound();
        }

        [Authorize]
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id,
            CancellationToken cancellationToken = default)
        {
            var isDeleted = await manager.DeleteAsync(id, cancellationToken);

            return isDeleted ? NoContent() : NotFound();
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchAsync([FromQuery] VideoGameSearchRequest request,
            CancellationToken cancellationToken = default)
        {
            return Ok(await manager.SearchAsync(request, cancellationToken));
        }

        [HttpGet("developers")]
        public async Task<IActionResult> GetDevelopersAsync(CancellationToken cancellationToken = default)
        {
            return Ok(await manager.GetDevelopersAsync(cancellationToken));
        }

        [HttpGet("genres")]
        public async Task<IActionResult> GetGenresAsync(CancellationToken cancellationToken = default)
        {
            return Ok(await manager.GetGenresAsync(cancellationToken));
        }

        [HttpGet("languages")]
        public async Task<IActionResult> GetLanguagesAsync(CancellationToken cancellationToken = default)
        {
            return Ok(await manager.GetLanguagesAsync(cancellationToken));
        }

        [HttpGet("platforms")]
        public async Task<IActionResult> GetPlatformsAsync(CancellationToken cancellationToken = default)
        {
            return Ok(await manager.GetPlatformsAsync(cancellationToken));
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetGamesCountAsync(CancellationToken cancellationToken = default)
        {
            return Ok(await manager.GetGamesCountAsync(cancellationToken));
        }
    }
}
