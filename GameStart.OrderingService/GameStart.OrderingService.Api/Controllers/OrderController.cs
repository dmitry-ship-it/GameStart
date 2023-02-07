using GameStart.OrderingService.Application.DtoModels;
using GameStart.OrderingService.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameStart.OrderingService.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpGet("{userId:Guid}")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid userId,
            CancellationToken cancellationToken = default)
        {
            return Ok(await orderService.GetByUserIdAsync(userId, cancellationToken));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] OrderDto order,
            CancellationToken cancellationToken = default)
        {
            await orderService.CreateAsync(order, cancellationToken);

            return Ok();
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id,
            CancellationToken cancellationToken = default)
        {
            var isDeleted = await orderService.DeleteAsync(id, cancellationToken);

            return isDeleted ? NoContent() : NotFound();
        }
    }
}
