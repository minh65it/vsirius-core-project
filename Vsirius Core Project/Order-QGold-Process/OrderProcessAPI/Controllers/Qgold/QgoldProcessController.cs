using Microsoft.AspNetCore.Mvc;
using OrderProcessApi._Services.IServices.Qgold;
using OrderProcessApi.DTOs;

namespace OrderProcessApi.Controllers.Qgold
{
    [Route("api/qgoldOrder/[controller]")]
    [ApiController]
    public class QgoldProcessController : ControllerBase
    {
        private readonly IQgoldProcessService _qgoldProcessService;

        public QgoldProcessController(IQgoldProcessService qgoldProcessService)
        {
            _qgoldProcessService = qgoldProcessService;
        }
        [HttpGet("GetReOrders")]
        public async Task<IActionResult> GetReOrders(
            [FromQuery] PaginationParam paginationParam,
            [FromQuery] SearchQgoldProcessParam seachParam)
        {
            return Ok(await _qgoldProcessService.GetReOrders(paginationParam, seachParam));
        }
        [HttpGet("GetOrders")]
        public async Task<IActionResult> GetOrders(
            [FromQuery] PaginationParam paginationParam,
            [FromQuery] SearchQgoldProcessParam seachParam)
        {
            return Ok(await _qgoldProcessService.GetOrders(paginationParam, seachParam));
        }

        [HttpGet("Detail")]
        public async Task<IActionResult> GetOrders(string id)
        {
            return Ok(await _qgoldProcessService.GetDetailOrder(id));
        }

        [HttpDelete("DeleteOrders")]
        public async Task<IActionResult> DeleteOrders(List<string> IDs)
        {
            return Ok(await _qgoldProcessService.DeleteOrders(IDs));
        }
    }
}
