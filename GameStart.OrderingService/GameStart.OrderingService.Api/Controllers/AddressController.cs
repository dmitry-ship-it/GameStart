using GameStart.OrderingService.Application.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameStart.OrderingService.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetAddress([FromRoute] Guid id)
        {
            throw new NotImplementedException();
        }

        [HttpPost("{id:Guid}")]
        public async Task<IActionResult> CreateAddress([FromRoute] Guid id, [FromBody] AddressModel address)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> UpdateAddress([FromRoute] Guid id, [FromBody] AddressModel address)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteAddress([FromRoute] Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
