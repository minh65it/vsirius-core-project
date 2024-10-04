using AmazonIntegrationDataApi._Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AmazonIntegrationDataApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TriggerOrderAmazonProcessorController : ControllerBase
    {
        private readonly ITriggerOrderAmazonProcessor _orderAmazonProcessor;
        private static bool IsGetNewOrder = false;
        private static bool IsSubmitOrderToQgold = false;
        private static bool IsSubmitOrderToStuller = false;
        private static bool IsUpdateTrackingOrder = false;
        private static bool IsGetOrderShipped = false;

        public TriggerOrderAmazonProcessorController(ITriggerOrderAmazonProcessor orderAmazonProcessor)
        {
            _orderAmazonProcessor = orderAmazonProcessor;
        }

        /// <summary>
        /// Get Order1, Order2 to Order (Amazon)
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetNewOrder")]
        public async Task<IActionResult> GetNewOrder()
        {
            if (IsGetNewOrder)
            {
                return BadRequest("GetNewOrder is running");
            }
            else
            {
                IsGetNewOrder = true;
                await _orderAmazonProcessor.GetNewOrder();
                IsGetNewOrder = false;
                return Ok("GetNewOrder is done");

            }
        }

        /// <summary>
        /// Submit order to Qgold
        /// </summary>
        /// <returns></returns>
        [HttpPost("SubmitOrderToQgold")]
        public async Task<IActionResult> SubmitOrderToQgold()
        {
            if (IsSubmitOrderToQgold)
            {
                return BadRequest("SubmitOrderToQgold is running");
            }
            else
            {
                IsSubmitOrderToQgold = true;
                await _orderAmazonProcessor.SubmitOrderToQgold();
                IsSubmitOrderToQgold = false;
                return Ok("SubmitOrderToQgold is done");

            }
        }

        /// <summary>
        /// Submit order to Stuller
        /// </summary>
        /// <returns></returns>
        [HttpPost("SubmitOrderToStuller")]
        public async Task<IActionResult> SubmitOrderToStuller()
        {
            if (IsSubmitOrderToStuller)
            {
                return BadRequest("SubmitOrderToStuller is running");
            }
            else
            {
                IsSubmitOrderToStuller = true;
                await _orderAmazonProcessor.SubmitOrderToStuller();
                IsSubmitOrderToStuller = false;
                return Ok("SubmitOrderToStuller is done");

            }
        }

        /// <summary>
        /// Update tracking from Qgold, Stuller to Amazon
        /// </summary>
        /// <returns></returns>
        [HttpPost("UpdateTrackingOrder")]
        public async Task<IActionResult> UpdateTrackingOrder()
        {
            if (IsUpdateTrackingOrder)
            {
                return BadRequest("UpdateTrackingOrder is running");
            }
            else
            {
                IsUpdateTrackingOrder = true;
                await _orderAmazonProcessor.UpdateTrackingOrder();
                IsUpdateTrackingOrder = false;
                return Ok("UpdateTrackingOrder is done");

            }
        }

        /// <summary>
        /// Get all order shipped in AmazonSeller
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetOrderShipped")]
        public async Task<IActionResult> GetOrderShipped()
        {
            if (IsGetOrderShipped)
            {
                return BadRequest("GetOrderShipped is running");
            }
            else
            {
                IsGetOrderShipped = true;
                await _orderAmazonProcessor.GetOrderShipped();
                IsGetOrderShipped = false;
                return Ok("GetOrderShipped is done");

            }
        }
    }
}
