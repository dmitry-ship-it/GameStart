﻿using GameStart.OrderingService.Application.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameStart.OrderingService.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetOrder([FromRoute] Guid id)
        {
            throw new NotImplementedException();
        }

        [HttpPost("{id:Guid}")]
        public async Task<IActionResult> CreateOrder([FromRoute] Guid id, [FromBody] CreateOrderRequest request)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteOrder([FromRoute] Guid id)
        {
            throw new NotImplementedException();
        }
    }
}