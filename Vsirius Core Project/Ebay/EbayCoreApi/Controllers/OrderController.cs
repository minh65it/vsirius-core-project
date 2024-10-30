using EbayCoreApi._Services.Interfaces;
using EbayCoreApi.Common.Helpers;
using EbayCoreApi.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace EbayCoreApi.Controllers
{
    [Route("api/ebay/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IEbayOrderService _orderService;

        public OrderController(IEbayOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<PaginationUtil<OrderDto>> SearchProducts(
          [FromQuery] SearchOrderParam search,
          [FromQuery] PaginationParam pagination)
        {
            if (pagination == null)
            {
                pagination = new PaginationParam
                {
                    PageSize = 1,
                    PageNumber = 1
                };
            }
            else
            {
                // Xử lý nếu PageSize hoặc PageNumber nhỏ hơn 1
                if (pagination.PageSize < 1)
                {
                    pagination.PageSize = 1;
                }
                else if (pagination.PageSize > 1000)
                {
                    pagination.PageSize = 1000;
                }

                if (pagination.PageNumber < 1)
                {
                    pagination.PageNumber = 1;
                }
            }
            return await _orderService.SearchOrders(pagination, search);
        }

        [HttpGet]
        [Route("api/order/{orderId}")]
        public async Task<OrderDto> GetProduct(string orderId)
        {
            return await _orderService.GetOrder(orderId);
        }
    }
}
