using AmazonIntegrationDataApi._Services.Interfaces;
using AmazonIntegrationDataApi._Services.Services;
using AmazonIntegrationDataApi.Dtos.MongoDB;
using AmazonIntegrationDataApi.Helpers.MongoDB;
using Microsoft.AspNetCore.Mvc;

namespace AmazonIntegrationDataApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReturnOrderController : ControllerBase
    {
        private readonly IReturnOrderAmazon _returnOrderAmazon;
        private static bool IsGetNewReturnOrders = false;

        public ReturnOrderController(IReturnOrderAmazon returnOrderAmazon)
        {
            _returnOrderAmazon = returnOrderAmazon;
        }

        /// <summary>
        /// Get return order in amazon in 30 days
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetNewReturnOrders")]
        public async Task<IActionResult> GetNewReturnOrders()
        {
            if (IsGetNewReturnOrders)
            {
                return BadRequest("GetNewReturnOrders is running");
            }
            else
            {
                IsGetNewReturnOrders = true;
                await _returnOrderAmazon.GetNewReturnOrders();
                IsGetNewReturnOrders = false;
                return Ok("GetNewReturnOrders is done");

            }
        }

        /// <summary>
        /// Get GetReturn Orders
        /// </summary>
        /// <param name="paginationParam"></param>
        /// <param name="searchParam"></param>
        /// <returns></returns>
        [HttpGet("GetReturnOrders")]
        public async Task<IActionResult> GetReturnOrders([FromQuery] PaginationParam paginationParam, [FromQuery] ReturnFBMOrderParams searchParam)
        {
            var result = await _returnOrderAmazon.GetReturnOrders(paginationParam, searchParam);
            return Ok(result);
        }

        [HttpGet("GetDetail")]
        public async Task<IActionResult> GetDetail(string amazonRMAID)
        {
            var result = await _returnOrderAmazon.GetDetail(amazonRMAID);
            return Ok(result);
        }
    }
}
