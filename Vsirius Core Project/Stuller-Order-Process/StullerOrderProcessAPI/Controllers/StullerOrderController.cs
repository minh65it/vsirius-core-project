using Microsoft.AspNetCore.Mvc;
using StullerOrderProcessAPI._Services.Interfaces;
using StullerOrderProcessAPI.Common.Helpers;
using StullerOrderProcessAPI.Dtos;

namespace StullerOrderProcessAPI.Controllers
{
    [ApiController]
    [Route("stullerOrder/[controller]")]
    public class StullerOrderController : ControllerBase
    {
        private readonly IOrderStullerService _submitOrder;

        public StullerOrderController(IOrderStullerService submitOrder)
        {
            _submitOrder = submitOrder;
        }
        /// <summary>
        /// ds các order của stuller
        /// </summary>
        /// <param name="searchParam"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetOrders(
           [FromQuery] SearchOrderStullerParam searchParam,
           [FromQuery] PaginationParam pagination)
        {
            return Ok(await _submitOrder.GetOrders(pagination, searchParam));
        }

        [HttpGet("Detail")]
        public async Task<IActionResult> GetDetail(string orderID)
        {
            return Ok(await _submitOrder.GetDetailOrder(orderID));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(List<string> orderIDs)
        {
            return Ok(await _submitOrder.DeleteOrders(orderIDs));
        }

        /// <summary>
        /// Submit Order cho stuller ||||||| 
        /// isResubmit: true => BO gọi đến để submit lại các order bị lỗi, false: dành cho lần đầu submit
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="order"></param>
        /// <param name="isResubmit"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SubmitStuller(
            [FromBody] SubmitOrderParam order,
            [FromQuery] bool? isResubmit)
        {
            isResubmit ??= false;
            return Ok(await _submitOrder.SubmitStuller(order, (bool)isResubmit));
        }

        /// <summary>
        /// API dành cho UIPath để gọi lên cập nhật stuller order map với marketplace nào
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut("MapOrder")]
        public async Task<IActionResult> MapOrder(
            [FromBody] List<MapOrderParam> param)
        {
            bool result = await _submitOrder.MapOrder(param);
            return Ok(result);
        }

        [HttpGet("OrderNeedToSubmit")]
        public async Task<IActionResult> GetOrderNeedToSubmit()
        {
            List<UnSubmittedStullerOrder> result = await _submitOrder.GetOrderNeedToSubmit();
            return Ok(result);
        }
    }
}

