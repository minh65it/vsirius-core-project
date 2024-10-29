using Microsoft.AspNetCore.Mvc;
using WalmartIntegrationDataApi._Services.Interfaces;
using WalmartIntegrationDataApi.Common.Helpers;
using WalmartIntegrationDataApi.Dtos.Order;
using WalmartIntegrationDataApi.Dtos.Return;

namespace WalmartIntegrationDataApi.Controllers
{
    [Route("api/walmart/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderWalmartService _processWalmartService;

        public OrderController(IOrderWalmartService processWalmartService)
        {
            _processWalmartService = processWalmartService;
        }

        [HttpGet("returns")]
        public async Task<PaginationUtility<ReturnOrderDTO>?> GetReturnOrder(
            [FromQuery] PaginationParam paginationParam,
            [FromQuery] SearchReturnParam search)
        {
            return await _processWalmartService.GetReturnOrders(paginationParam, search);
        }

        [HttpGet("detailreturn")]
        public async Task<ReturnOrderDTO?> GetDetailReturnOrder(string id)
        {
            return await _processWalmartService.GetDetailReturnOrder(id);
        }

        [HttpGet("GetOrders")]
        public async Task<PaginationUtility<OrderDetailDTO>> GetOrders(
            [FromQuery] PaginationParam paginationParam, 
            [FromQuery] WalmartOrderSearchParam seachParam)
        {
            return await _processWalmartService.GetOrders(paginationParam, seachParam);
        }

        [HttpGet("DetailOrder")]
        public async Task<OrderDetailDTO?> GetOrders(string id)
        {
            return await _processWalmartService.GetDetailOrder(id);
        }

        [HttpDelete("DeleteOrders")]
        public async Task<OperationResult> DeleteOrders(List<string> IDs)
        {
            return await _processWalmartService.DeleteOrders(IDs);
        }
        /// <summary>
        /// Get list unshipped OrderID
        /// </summary>
        /// <returns></returns>
        [HttpGet("UnshippedOrderIds")]
        public async Task<List<UnshipOrderDto>> GetUnshippedOrderIds([FromQuery] UnshippedOrderIdsParam param)
        {
            return await _processWalmartService.GetUnshippedOrderIds(param);
        }
    }
}
