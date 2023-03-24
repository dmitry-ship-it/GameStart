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

        [HttpGet]
        public async Task<IActionResult> GetAsync(CancellationToken cancellationToken = default)
        {
            return Ok(await orderService.GetByUserIdAsync(HttpContext.User.Claims, cancellationToken));
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            var order = await orderService.GetByIdAsync(id, HttpContext.User.Claims, cancellationToken);

            return order is null ? NotFound() : Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] OrderDto order,
            CancellationToken cancellationToken = default)
        {
            var createdId = await orderService.CreateAsync(order, HttpContext.User.Claims, cancellationToken);

            return Accepted(createdId);
        }
    }
}
