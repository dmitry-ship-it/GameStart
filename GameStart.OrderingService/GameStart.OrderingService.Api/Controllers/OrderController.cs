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
        public async Task<IActionResult> GetAsync([FromRoute] Guid userId)
        {
            return Ok(await orderService.GetByUserIdAsync(userId));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] OrderDto order)
        {
            await orderService.CreateAsync(order);

            return Ok();
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            var isDeleted = await orderService.DeleteAsync(id);

            return isDeleted ? NoContent() : NotFound();
        }
    }
}
