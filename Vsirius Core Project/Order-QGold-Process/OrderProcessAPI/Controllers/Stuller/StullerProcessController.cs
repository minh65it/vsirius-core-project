//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using OrderProcessApi._Services.IServices.Qgold;
//using OrderProcessApi.DTOs;

//namespace OrderProcessApi.Controllers.Stuller
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class StullerProcessController : ControllerBase
//    {
//        private readonly IStullerProcessService _stullerProcessService;

//        public StullerProcessController(IStullerProcessService StullerProcessService)
//        {
//            _stullerProcessService = StullerProcessService;
//        }

//        [HttpGet("GetOrders")]
//        public async Task<IActionResult> GetOrders(
//            [FromQuery] PaginationParam paginationParam,
//            [FromQuery] SearchStullerProcessParam seachParam)
//        {
//            return Ok(await _stullerProcessService.GetOrders(paginationParam, seachParam));
//        }

//        [HttpGet("Detail")]
//        public async Task<IActionResult> GetOrders(string id)
//        {
//            return Ok(await _stullerProcessService.GetDetailOrder(id));
//        }

//        [HttpDelete("DeleteOrders")]
//        public async Task<IActionResult> DeleteOrders(List<string> IDs)
//        {
//            return Ok(await _stullerProcessService.DeleteOrders(IDs));
//        }
//    }
//}
