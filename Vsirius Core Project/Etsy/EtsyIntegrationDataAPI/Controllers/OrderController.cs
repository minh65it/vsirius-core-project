using EtsyIntegrationDataAPI._Services.Interfaces;
using EtsyIntegrationDataAPI.Dtos;
using EtsyIntegrationDataAPI.Parameters.OrderParmas;
using EtsyIntegrationDataAPI.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace EtsyIntegrationDataAPI.Controllers
{
    [Route("api/oldetsy/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private static bool isUpdateTrackingOrder = false;
        private static bool isSubmitOrderQgold = false;
        private static bool isSubmitOrderStuller = false;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Lấy thông tin chi tiết đơn hàng từ Etsy 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetOrderDetail")]
        public async Task<IActionResult> GetOrderDetail(long orderId)
        {
            var res = await _orderService.GetOrderDetail(orderId);
            return Ok(res);
        }

        /// <summary>
        /// Lấy danh sách đơn hàng từ Etsy 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllOrder")]
        public IActionResult GetAllOrder([FromQuery] SearchOrderParam search,
          [FromQuery] PaginationParam pagination)
        {
            var res = _orderService.GetAllOrderEtsy(search, pagination);
            return Ok(res);
        }

        /// <summary>
        /// Lấy toàn bộ danh sách đơn hàng từ Etsy
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetShopReceipts")]
        public async Task<IActionResult> GetShopReceipts([FromBody] GetShopReceiptsParams? queries, [FromQuery] int? pageNumber)
        {
            var res = await _orderService.GetShopReceipts(queries, pageNumber);
            return Ok(res);
        }
        /// <summary>
        /// Endpoint dùng đề lấy hết các order mới nhất trên Etsy và lưu trong Database (Order, Shipment, Transaction)
        /// </summary>
        /// <remarks>
        /// Lấy 2 trang gần nhất, was_shipped = false
        /// </remarks>
        /// <returns></returns>
        [HttpGet("GetNewOrderEtsy")]
        public async Task<IActionResult> GetNewOrderEtsy()
        {
            var res = await _orderService.GetNewOrderEtsy();
            return Ok(res);
        }

        /// <summary>
        /// Submit các order đến Qgold
        /// </summary>
        /// <returns></returns>
        [HttpPost("SubmitOrderQgold")]
        public async Task<IActionResult> SubmitOrderQgold()
        {
            if (isSubmitOrderQgold)
            {
                return BadRequest("isSubmitOrderQgold is running");
            }
            else
            {
                isSubmitOrderQgold = true;
                await _orderService.SubmitOrderQgold();
                isSubmitOrderQgold = false;
                return Ok("isSubmitOrderQgold is done");
            }
        }

        /// <summary>
        /// Submit các order đến Stuller
        /// </summary>
        /// <returns></returns>
        [HttpPost("SubmitOrderStuller")]
        public async Task<IActionResult> SubmitOrderStuller()
        {
            if (isSubmitOrderStuller)
            {
                return BadRequest("SubmitOrderStuller is running");
            }
            else
            {
                isSubmitOrderStuller = true;
                await _orderService.SubmitOrderStuller();
                isSubmitOrderStuller = false;
                return Ok("SubmitOrderStuller is done");
            }
        }

        /// <summary>
        /// Cập nhật tracking cho sản phẩm Qgold, Stuller
        /// </summary>
        /// <returns></returns>
        [HttpPost("UpdateTrackingOrder")]
        public async Task<IActionResult> UpdateTrackingOrder()
        {
            if (isUpdateTrackingOrder)
            {
                return BadRequest("isUpdateTrackingOrder is running");
            }
            else
            {
                isUpdateTrackingOrder = true;
                await _orderService.UpdateTrackingOrder();
                isUpdateTrackingOrder = false;
                return Ok("isUpdateTrackingOrder is done");

            }
        }
    }
}
