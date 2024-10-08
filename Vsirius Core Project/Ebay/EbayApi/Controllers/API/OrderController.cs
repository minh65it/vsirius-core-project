using EbayApi.Dtos;
using EbayApi.Services.Interfaces;
using EbayApi.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace EbayApi.Controllers.API
{
    public class OrderController : ApiController
    {
        private readonly IEbayOrderService _orderService;

        public OrderController(IEbayOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<PaginationUtil<OrderDto>> SearchProducts(
          [FromUri] SearchParam search,
          [FromUri] PaginationParam pagination)
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
