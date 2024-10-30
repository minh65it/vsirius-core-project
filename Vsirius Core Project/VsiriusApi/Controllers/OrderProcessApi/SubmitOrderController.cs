using Microsoft.AspNetCore.Mvc;
using OrderProcessApi._Services.IServices;
using OrderProcessApi._Services.IServices.Qgold;
using OrderProcessDomain.AmazonSeller;

namespace VsiriusApi.Controllers.OrderProcessApi
{
    [ApiExplorerSettings(GroupName = "QgoldOrder")]
    [Route("api/[controller]")]
    [ApiController]
    public class SubmitOrderController : ControllerBase
    {
        private readonly ISubmitOrderQgoldService _submitOrder;
        private readonly IKeyService _keyService;

        public SubmitOrderController(ISubmitOrderQgoldService submitOrder, IKeyService keyService)
        {
            _submitOrder = submitOrder;
            _keyService = keyService;
        }

        /// <summary>
        /// Submit Order to Qgold
        /// </summary>
        /// <remarks>
        /// clientId: OrderStullerQgoldApi; clientSecret: 90b34412-b58e-4f88-ab30-0c1da8a139d8-OrderAPI
        /// </remarks>
        /// <param name="order"></param>
        /// <param name="isResubmit"></param>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SubmitQgold(
            [FromBody] List<QgoldFtpOrderObject> order,
            [FromHeader] string clientId,
            [FromHeader] string clientSecret,
            [FromQuery] bool? isResubmit)
        {
            if (_keyService.ValidateCredentials(clientId, clientSecret))
            {
                return Ok(await _submitOrder.SubmitQgold(order, isResubmit));
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
