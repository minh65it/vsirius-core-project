//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using OrderProcessApi._Services.IServices.Amazon;
//using OrderProcessApi.DTOs;
//using OrderProcessDomain.AmazonSeller;

//namespace OrderProcessApi.Controllers.Amazon
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AmazonProcessController : ControllerBase
//    {
//        private readonly IAmazonProcessService _amazonProcessService;

//        public AmazonProcessController(IAmazonProcessService amazonProcessService)
//        {
//            _amazonProcessService = amazonProcessService;
//        }

//        [HttpGet("GetOrders")]
//        public async Task<IActionResult> GetOrders([FromQuery] PaginationParam paginationParam, [FromQuery] AmazonProcessSearchParam seachParam)
//        {
//            return Ok(await _amazonProcessService.GetOrders(paginationParam, seachParam));
//        }

//        [HttpGet("Detail")]
//        public async Task<IActionResult> GetOrders(string id)
//        {
//            return Ok(await _amazonProcessService.GetDetailOrder(id));
//        }
//        [HttpDelete("DeleteOrders")]
//        public async Task<IActionResult> DeleteOrders(List<string> IDs)
//        {
//            return Ok(await _amazonProcessService.DeleteOrders(IDs));
//        }

//        /// <summary>
//        /// Re-submit order failed
//        /// </summary>
//        /// <remarks>
//        /// Delete Amazon.OrderSubmitted and save new order Qgold.ReSubmit
//        /// </remarks>
//        /// <param name="order"></param>
//        /// <returns></returns>
//        [HttpPut("ReSubmit")]
//        public async Task<IActionResult> ReSubmit([FromBody] QgoldFtpOrderObject order)
//        {
//            return Ok(await _amazonProcessService.ReSubmit(order));
//        }

//        /// <summary>
//        /// Get new orders from amazon seller
//        /// </summary>
//        /// <returns></returns>
//        [HttpPost("GetNewOrders")]
//        public async Task<IActionResult> GetNewOrders()
//        {
//            return Ok(await _amazonProcessService.GetNewOrders());
//        }
//    }
//}
