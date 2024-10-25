using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrdersAPI._Services.Interfaces;
using OrdersAPI.Dtos;
using OrdersAPI.Helpers.Utilities;

namespace VsiriusAPI.Controllers.OrderApi
{
    [ApiExplorerSettings(GroupName = "Order")]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderStatusController : ControllerBase
    {
        private readonly IOderStatusService _service;

        public OrderStatusController(IOderStatusService service)
        {
            _service = service;
        }

        /// <summary>
        /// Tạo trạng thái đơn hàng
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("Create")]
        public async Task<IActionResult> Create(OrderStatusDto dto)
        {
            return Ok(await _service.Create(dto));
        }

        /// <summary>
        /// Cập nhật dữ liệu trạng thái đơn hàng
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(OrderStatusDto dto)
        {
            return Ok(await _service.Update(dto));
        }

        /// <summary>
        /// Lấy dữ liệu chi tiết của trang thái đơn hàng
        /// </summary>
        /// <param name="keyword">Nhập keyword tìm kiếm</param>
        /// <returns></returns>
        [HttpGet("GetData")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination, [FromQuery] string keyword)
        {
            return Ok(await _service.GetData(pagination, keyword));
        }

        /// <summary>
        /// Lấy danh sách trạng thái đơn hàng
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Customer, Admin")]
        [HttpGet("GetListStatus")]
        public async Task<IActionResult> GetListStatus()
        {
            return Ok(await _service.GetListStatus());
        }

        /// <summary>
        /// Xóa trạng thái đơn hàng
        /// </summary>
        /// <param name="id">Nhập id trạng thái đơn hàng</param>
        /// <returns></returns>
        [HttpDelete("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _service.Delete(id));
        }


        /// <summary>
        /// Seed data status: 
        /// </summary>
        /// <remarks>
        /// Pending, Approved, Shipping, OrderDelivered, Cancel
        /// </remarks>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("SeedDataStatus")]
        public async Task<IActionResult> SeedDataStatus()
        {
            return Ok(await _service.SeedDataStatus());
        }
    }
}
