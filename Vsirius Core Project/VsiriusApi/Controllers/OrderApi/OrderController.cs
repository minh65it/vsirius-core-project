
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using OrdersAPI._Services.Interfaces;
using OrdersAPI.Dtos;
using OrdersAPI.Helpers.ConfigSignalR;
using OrdersAPI.Helpers.Params;
using OrdersAPI.Helpers.Utilities;
using VsiriusDataModel.DataModel;

namespace VsiriusAPI.Controllers.OrderApi
{
    [ApiExplorerSettings(GroupName = "Order")]
    [Route("api/order/[controller]")]
    [ApiController]
    [Authorize(Roles = "Customer, Admin")]
    public class OrderController : ControllerBase
    {
        private readonly IVsiriusOrderService _service;
        private readonly IHubContext<OrderHub> _orderHubContext;
        private readonly IConfiguration _configuration;
        private readonly ISendMailService _sendMailService;
        private readonly IFunctionUtilities _functionUtilities;

        public OrderController(
            IVsiriusOrderService service,
            IHubContext<OrderHub> orderHubContext,
            IConfiguration configuration,
            ISendMailService sendMailService,
            IFunctionUtilities functionUtilities)
        {
            _service = service;
            _orderHubContext = orderHubContext;
            _configuration = configuration;
            _sendMailService = sendMailService;
            _functionUtilities = functionUtilities;
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
            if (data.Success)
            {
                var orderInfo = (VsiriusOrders)data.Data;
                var from = _configuration.GetSection("Email:From").Value!;
                var to = _configuration.GetSection("Email:To").Value!;
                var subject = $"You have order from Vsirius Web";
                var body =
                    "This is infomation of order: <br/><br/>" +
                    $"OrderId: {orderInfo.OrderId}<br/>" +
                    $"CartId: {orderInfo.CartId}<br/>" +
                    $"UserId: {orderInfo.UserId}<br/>" +
                    $"FullName: {orderInfo.FullName}<br/>" +
                    $"FirstName: {orderInfo.FirstName}<br/>" +
                    $"LastName: {orderInfo.LastName}<br/>" +
                    $"Email: {orderInfo.Email}<br/>" +
                    $"Phone: {orderInfo.Phone}<br/>" +
                    $"Address1: {orderInfo.Address1}<br/>" +
                    $"Address2: {orderInfo.Address2}<br/>" +
                    $"City: {orderInfo.City}<br/>" +
                    $"State: {orderInfo.State}<br/>" +
                    $"PostCode: {orderInfo.PostCode}<br/>" +
                    $"Country: {orderInfo.Country}<br/>" +
                    $"Shipping: {orderInfo.Shipping}<br/>" +
                    $"PaymentMethod: {orderInfo.PaymentMethod}<br/>" +
                    $"Note: {orderInfo.Note}<br/>" +
                    $"CreateOrderDate: {orderInfo.CreateOrderDate}<br/>" +
                    $"TotalQuantityItem: {orderInfo.TotalQuantityItem}<br/>" +
                    $"TotalPrice: {orderInfo.TotalPrice}<br/>" +
                    $"Tax: {orderInfo.Tax}<br/>" +
                    $"TransactionInfo: {orderInfo.TransactionId}<br/>";
                //send email to system BO
                await _sendMailService.SendToSingleEmailCC(from, to, null, subject, body);

                //send email to customer
                to = orderInfo.Email;
                subject = $"Payment Confirmation - Order #{orderInfo.OrderId}";
                body = $"Dear {orderInfo.FullName}, " +
                    $"<br/>" +
                    $"We are pleased to inform you that your payment for Order #{orderInfo.OrderId} has been successfully processed." +
                    $"<br/><br/>" +
                    $"<b>Order Details:</b>" +
                    $"<ul>" +
                    $"<li>Order Number: #{orderInfo.OrderId}</li>" +
                    $"<li>Order Date: {orderInfo.CreateOrderDate}</li>" +
                    $"<li>Total Amount: $ {orderInfo.TotalPrice}</li>" +
                    $"</ul>" +
                    $"<b>Shipping Information:</b>" +
                    $"<ul>" +
                    $"<li>Shipping Address:" +
                    $"<br/>Address1: {orderInfo.Address1}" +
                    $"<br/>Address2: {orderInfo.Address2}" +
                    $"<br/>City: {orderInfo.City}" +
                    $"<br/>State: {orderInfo.State}" +
                    $"<br/>PostCode: {orderInfo.PostCode}" +
                    $"<br/>Country: {orderInfo.Country}" +
                    $"</li>" +
                    $"<li>Estimated Delivery Date: {DateTime.Parse(orderInfo.CreateOrderDate.ToString()).AddDays(5)}</li>" +
                    $"</ul>" +
                    $"Thank you for shopping with us. We appreciate your business and look forward to serving you again." +
                    $"<br/><br/>If you have any questions or need further assistance, please feel free to contact " +
                    $"our customer support team at admin@vsirius.com or 215-500-2667." +
                    $"<br/><br/>Best regards," +
                    $"<br/>VSirius" +
                    $"<br/>Address 630 Church Ln, Yeadon, PA 19050"
                    ;
                await _sendMailService.SendToSingleEmail(from, to, subject, body);

                var message = $"You have order from Vsirius Web. Order Info: {orderInfo.OrderId}. UTC Date: {DateTime.UtcNow}";
                var lstPhoneToSend = _configuration["AmazonSMS:ToPhoneNumber"].Split('|');
                foreach (var phone in lstPhoneToSend)
                {
                    await _functionUtilities.SendSMSWithAWS(orderInfo.Phone, message);
                }

                // send noti to BO
                await _orderHubContext.Clients.All.SendAsync("OrderNotification", orderInfo);
            }
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
        /// Cập nhật trạng thái đơn hàng
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("UpdateStatusOrder")]
        public async Task<IActionResult> UpdateStatusOrder([FromBody]UpdateStatusOrderDto dto)
        {
            return Ok(await _service.UpdateStatusOrder(dto));
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
        public async Task<IActionResult> GetListOrderHistory([FromQuery] PaginationParam pagination, string email)
        {
            return Ok(await _service.GetListOrderHistory(pagination, email));
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
