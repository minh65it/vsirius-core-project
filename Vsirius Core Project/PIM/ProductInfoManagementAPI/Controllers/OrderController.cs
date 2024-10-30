using Microsoft.AspNetCore.Mvc;
using PimApi._Services.Interfaces;
using PimApi.Dtos.OrderDto;
using PimApi.Helpers.Utilities;

namespace PimApi.Controllers
{
    [Route("api/pim/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetOrder([FromQuery] PaginationParam pagination,  [FromQuery] SearchOrderParams searchParams)
        {
            var result = await _orderService.GetOrder(pagination, searchParams);
            return Ok(result);
        }
        [HttpGet("GetProductTop")]
        public async Task<IActionResult> GetProductTop([FromQuery] PaginationParam pagination, [FromQuery] SearchOrderParams searchParams)
        {
            var result = await _orderService.GetProductTop(pagination, searchParams);
            return Ok(result);
        }
    }
}
