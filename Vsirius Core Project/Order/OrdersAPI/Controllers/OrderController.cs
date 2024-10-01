using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using OrdersAPI._Services.Interfaces;
using OrdersAPI.Dtos;
using OrdersAPI.Helpers.ConfigSignalR;
using OrdersAPI.Helpers.Params;
using OrdersAPI.Helpers.Utilities;

namespace OrdersAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Customer, Admin")]
    public class OrderController : ControllerBase
    {
        private readonly IVsiriusOrderService _service;
        private readonly IHubContext<OrderHub> _orderHubContext;

        public OrderController(IVsiriusOrderService service, IHubContext<OrderHub> orderHubContext)
        {
            _service = service;
            _orderHubContext = orderHubContext;
        }

        /// <summary>
        /// Tạo đơn hàng
        /// </summary>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(VsiriusOrderDto dto)
        {
            dto.StatusId = 1;
            dto.CreateOrderDate = DateTime.Now;
            var data = await _service.Create(dto);
            await _orderHubContext.Clients.All.SendAsync("OrderNotification", data.Data);
            return Ok(data);
        }

        /// <summary>
        /// Cập nhật đơn hàng
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        public async Task<IActionResult> Update(VsiriusOrderDto dto)
        {
            return Ok(await _service.Update(dto));
        }

        /// <summary>
        /// Lấy danh sách dữ liệu đơn hàng
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("GetData")]
        public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination, [FromQuery] OrderParams param)
        {
            return Ok(await _service.GetData(pagination, param));
        }

        /// <summary>
        /// Lấy danh sách đơn hàng gần nhất
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetTopOrder")]
        public async Task<IActionResult> GetTopOrder()
        {
            return Ok(await _service.TopOrders());
        }

        /// <summary>
        /// Lấy danh sách đơn hàng của người dùng
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet("GetListOrderHistory")]
        public async Task<IActionResult> GetListOrderHistory([FromQuery]PaginationParam pagination,string email)
        {
            return Ok(await _service.GetListOrderHistory(pagination,email));
        }

        /// <summary>
        /// Lấy dữ liệu chi tiết của đơn hàng
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("GetDetail")]
        public async Task<IActionResult> GetDetail(string orderId)
        {
            return Ok(await _service.GetDetail(orderId));
        }
    }
}
