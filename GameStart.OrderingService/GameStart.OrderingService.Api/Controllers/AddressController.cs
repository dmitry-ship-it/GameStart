using GameStart.OrderingService.Application.DtoModels;
using GameStart.OrderingService.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameStart.OrderingService.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService addressService;

        public AddressController(IAddressService addressService)
        {
            this.addressService = addressService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(CancellationToken cancellationToken = default)
        {
            return Ok(await addressService.GetByUserIdAsync(HttpContext.User.Claims, cancellationToken));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] AddressDto address,
            CancellationToken cancellationToken = default)
        {
            await addressService.CreateAsync(address, cancellationToken);

            return Ok();
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] AddressDto address,
            CancellationToken cancellationToken = default)
        {
            var isUpdated = await addressService.UpdateAsync(id, address, cancellationToken);

            return isUpdated ? Ok() : NotFound();
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id,
            CancellationToken cancellationToken = default)
        {
            var isDeleted = await addressService.DeleteAsync(id, cancellationToken);

            return isDeleted ? NoContent() : NotFound();
        }
    }
}
