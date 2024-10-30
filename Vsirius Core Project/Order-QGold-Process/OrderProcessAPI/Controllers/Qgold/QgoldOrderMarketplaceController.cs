using Microsoft.AspNetCore.Mvc;
using OrderProcessApi._Services.IServices;
using OrderProcessApi._Services.IServices.Qgold;
using OrderProcessApi._Services.Services.Qgold;
using OrderProcessApi.DTOs;
using OrderProcessDomain.Qgold;

namespace OrderProcessApi.Controllers.Qgold
{
    [Route("api/qgoldOrder/[controller]")]
    [ApiController]
    public class QgoldOrderMarketplaceController : ControllerBase
    {
        private readonly IQgoldOrderMarketplaceService _orderMarketplace;
        private readonly IKeyService _keyService;

        public QgoldOrderMarketplaceController(IQgoldOrderMarketplaceService orderMarketplace, IKeyService keyService)
        {
            _orderMarketplace = orderMarketplace;
            _keyService = keyService;
        }
        /// <summary>
        /// để update collection Order Marketplace, Re-Order trong database Qgold mongoDB
        /// </summary>
        /// <remarks>
        /// clientId: OrderStullerQgoldApi; clientSecret: 90b34412-b58e-4f88-ab30-0c1da8a139d8-OrderAPI
        /// </remarks>
        /// <param name="order"></param>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateOrderViaName([FromBody] List<OrderMarketplaceParams> order, [FromHeader] string clientId, [FromHeader] string clientSecret)
        {
            if (_keyService.ValidateCredentials(clientId, clientSecret))
            {
                return Ok(await _orderMarketplace.UpdateOrderViaName(order));
            }
            else
            {
                return Unauthorized();
            }
        }

        /// <summary>
        /// Get all order marketplace
        /// </summary>
        /// <param name="seachParam"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet("GetOrders")]
        public async Task<IActionResult> GetOrders(
            [FromQuery] OrderMarketplaceParams seachParam,
            [FromQuery] PaginationParam page)
        {
            return Ok(await _orderMarketplace.GetOrder(seachParam, page));
        }

        /// <summary>
        /// Get detail by seller id
        /// </summary>
        /// <param name="orderSellerId"></param>
        /// <returns></returns>
        [HttpGet("DetailOrderBySellerId")]
        public async Task<IActionResult> DetailOrderBySellerId(
            [FromQuery] string orderSellerId)
        {
            return Ok(await _orderMarketplace.DetailOrderBySellerId(orderSellerId));
        }
        /// <summary>
        /// get detail by order id
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("DetailOrderById")]
        public async Task<IActionResult> DetailOrderById(
            [FromQuery] string orderId)
        {
            return Ok(await _orderMarketplace.DetailOrderById(orderId));
        }
    }
}
