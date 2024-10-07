using Microsoft.AspNetCore.Mvc;
using StullerOrderProcessAPI._JobServices.Interfaces;

namespace StullerOrderProcessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TriggerController : ControllerBase
    {
        private static bool IsGetOrderStuller = false;

        private readonly IGetStullerOrderService _getStullerOrderService;

        public TriggerController(IGetStullerOrderService getStullerOrderService)
        {
            _getStullerOrderService = getStullerOrderService;
        }

        [HttpPost("GetOrderStuller")]
        public async Task<IActionResult> GetOrderStuller()
        {
            if (IsGetOrderStuller)
            {
                return BadRequest("Process is running.");
            }
            else
            {
                IsGetOrderStuller = true;
                await _getStullerOrderService.GetStullerOrder();
                IsGetOrderStuller = false;
                return Ok("Trigger success.");
            }
        }
    }
}
