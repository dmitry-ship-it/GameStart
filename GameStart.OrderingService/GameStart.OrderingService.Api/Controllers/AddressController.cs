using GameStart.OrderingService.Application.DtoModels;
using GameStart.OrderingService.Application.Services;
using GameStart.OrderingService.Core.Entities;
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

        [HttpGet("{userId:Guid}")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid userId)
        {
            return Ok(await addressService.GetByUserIdAsync(userId));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] AddressDto address)
        {
            await addressService.CreateAsync(address);

            return Ok();
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] AddressDto address)
        {
            var isUpdated = await addressService.UpdateAsync(id, address);

            return isUpdated ? Ok() : NotFound();
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            var isDeleted = await addressService.DeleteAsync(id);

            return isDeleted ? NoContent() : NotFound();
        }
    }
}
