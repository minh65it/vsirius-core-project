//using Microsoft.AspNetCore.Mvc;
//using OrderProcessApi._Services.IServices.Qgold;
//using OrderProcessApi.DTOs;

//namespace OrderProcessApi.Controllers.Qgold
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ReSubmitOrderController : ControllerBase
//    {
//        private readonly IReSubmitOrderService _reSubmitOrderService;

//        public ReSubmitOrderController(IReSubmitOrderService reSubmitOrderService)
//        {
//            _reSubmitOrderService = reSubmitOrderService;
//        }

//        /// <summary>
//        /// Add new reOrder Qgold
//        /// </summary>
//        /// <param name="dto"></param>
//        /// <returns></returns>
//        [HttpPost]
//        public async Task<IActionResult> AddReSubmitOrder([FromBody] OrderMarketplaceDto dto)
//        {
//            return Ok(await _reSubmitOrderService.AddReSubmitOrder(dto));
//        }
//    }
//}
