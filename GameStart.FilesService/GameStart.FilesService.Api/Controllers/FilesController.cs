using GameStart.FilesService.Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace GameStart.FilesService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly IFileService fileService;

        public FilesController(IFileService fileService)
        {
            this.fileService = fileService;
        }

        [HttpGet("{fileName}")]
        public async Task<IActionResult> GetFileAsync([FromRoute] string fileName, CancellationToken cancellationToken = default)
        {
            var found = await fileService.GetFileAsync(fileName, cancellationToken);

            return found is null ? NotFound() : File(found, MediaTypeNames.Application.Octet, true);
        }

        [Authorize]
        [HttpPost("{fileName}")]
        public async Task<IActionResult> SaveFileAsync(
            [FromForm] IFormFile file,
            [FromRoute] string fileName,
            CancellationToken cancellationToken = default)
        {
            await fileService.SaveFileAsync(file, fileName, cancellationToken);

            return Ok();
        }
    }
}
