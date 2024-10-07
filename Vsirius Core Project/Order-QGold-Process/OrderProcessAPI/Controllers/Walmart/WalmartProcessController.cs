//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using OrderProcessApi._Services.IServices.Walmart;
//using OrderProcessApi.DTOs;
//using OrderProcessDomain.AmazonSeller;

//namespace OrderProcessApi.Controllers.Walmart
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class WalmartProcessController : ControllerBase
//    {
//        private readonly IWalmartProcessService _WalmartProcessService;

//        public WalmartProcessController(IWalmartProcessService WalmartProcessService)
//        {
//            _WalmartProcessService = WalmartProcessService;
//        }

//        [HttpGet("GetOrders")]
//        public async Task<IActionResult> GetOrders([FromQuery] PaginationParam paginationParam, [FromQuery] WalmartProcessSearchParam seachParam)
//        {
//            return Ok(await _WalmartProcessService.GetOrders(paginationParam, seachParam));
//        }

//        [HttpGet("LastestSellerOrder")]
//        public async Task<IActionResult> LastestSellerOrder()
//        {
//            return Ok(await _WalmartProcessService.LastestSellerOrder());
//        }

//        [HttpGet("Detail")]
//        public async Task<IActionResult> GetOrders(string id)
//        {
//            return Ok(await _WalmartProcessService.GetDetailOrder(id));
//        }

//        [HttpDelete("DeleteOrders")]
//        public async Task<IActionResult> DeleteOrders(List<string> IDs)
//        {
//            return Ok(await _WalmartProcessService.DeleteOrders(IDs));
//        }
//        [HttpPut("ReSubmit")]
//        public async Task<IActionResult> ReSubmit([FromBody] QgoldFtpOrderObject order)
//        {
//            return Ok(await _WalmartProcessService.ReSubmit(order));
//        }
//    }
//}
