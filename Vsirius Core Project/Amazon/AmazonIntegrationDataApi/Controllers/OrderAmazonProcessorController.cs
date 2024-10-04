using AmazonIntegrationDataApi._Services.Interfaces;
using AmazonIntegrationDataApi.Dtos.OrderAmazonProcessor;
using AmazonIntegrationDataApi.Helpers.MongoDB;
using AmazonIntegrationDataApi.Models.OrderAmazonProcessor;
using Microsoft.AspNetCore.Mvc;

namespace AmazonIntegrationDataApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderAmazonProcessorController : ControllerBase
    {
        private readonly IOrderAmazonProcessor _orderAmazonProcessor;

        public OrderAmazonProcessorController(IOrderAmazonProcessor orderAmazonProcessor)
        {
            _orderAmazonProcessor = orderAmazonProcessor;
        }
        /// <summary>
        /// Get all order in Data Amazon
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="seachParam"></param>
        /// <returns></returns>
        [HttpGet("GetOrders")]
        public async Task<IActionResult> GetOrders(
            [FromQuery] PaginationParam pagination, 
            [FromQuery] AmazonProcessSearchParam seachParam)
        {
            return Ok(await _orderAmazonProcessor.GetOrders(pagination, seachParam));
        }
        /// <summary>
        /// Detail 1 order in Database Amazon Order
        /// </summary>
        /// <param name="sellerOrderId"></param>
        /// <returns></returns>
        [HttpGet("GetDetailOrder")]
        public async Task<IActionResult> GetDetailOrder([FromQuery] string sellerOrderId)
        {
            return Ok(await _orderAmazonProcessor.GetDetailOrder(sellerOrderId));
        }
        /// <summary>
        /// Re-submit order failed
        /// </summary>
        /// <remarks>
        /// Delete Amazon.OrderSubmitted and save new order Qgold.ReSubmit
        /// </remarks>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPut("ReSubmit")]
        public async Task<IActionResult> ReSubmit([FromBody] QgoldFtpOrderObject order)
        {
            return Ok(await _orderAmazonProcessor.ReSubmit(order));
        }
        /// <summary>
        /// Get list unshipped OrderID
        /// </summary>
        /// <returns></returns>
        [HttpGet("UnshippedOrderIds")]
        public async Task<List<UnshipOrderDto>> GetUnshippedOrderIds([FromQuery] UnshippedOrderIdsParam param)
        {
            return await _orderAmazonProcessor.GetUnshippedOrderIds(param);
        }
    }
}
