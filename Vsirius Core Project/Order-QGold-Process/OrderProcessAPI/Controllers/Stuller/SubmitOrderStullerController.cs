//using Microsoft.AspNetCore.Mvc;
//using OrderProcessApi._Services.IServices;
//using OrderProcessApi._Services.IServices.Stuller;
//using OrderProcessDomain.Stuller;

//namespace OrderProcessApi.Controllers.Stuller
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class SubmitOrderStullerController : ControllerBase
//    {
//        private readonly ISubmitOrderStullerService _submitOrder;
//        private readonly IKeyService _keyService;

//        public SubmitOrderStullerController(ISubmitOrderStullerService submitOrder, IKeyService keyService)
//        {
//            _submitOrder = submitOrder;
//            _keyService = keyService;
//        }

//        /// <summary>
//        /// Submit Order
//        /// </summary>
//        /// <remarks>
//        /// clientId: OrderStullerQgoldApi; clientSecret: 90b34412-b58e-4f88-ab30-0c1da8a139d8-OrderAPI
//        /// </remarks>
//        /// <param name="order"></param>
//        /// <param name="clientId"></param>
//        /// <param name="clientSecret"></param>
//        /// <returns></returns>
//        [HttpPost]
//        public async Task<IActionResult> SubmitStuller([FromBody] SubmitOrderParam order, [FromHeader] string clientId, [FromHeader] string clientSecret)
//        {
//            if (_keyService.ValidateCredentials(clientId, clientSecret))
//            {
//                return Ok(await _submitOrder.SubmitStuller(order));
//            }
//            else
//            {
//                return Unauthorized();
//            }
//        }
//    }
//}
