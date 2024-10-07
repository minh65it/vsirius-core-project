using Microsoft.AspNetCore.Mvc;
using OrderProcessApi._Services.IServices.Qgold;
using OrderProcessApi._Services.IServices;
using OrderProcessApi.DTOs;

namespace OrderProcessApi.Controllers.Qgold
{
    [Route("api/[controller]")]
    [ApiController]
    public class QAQgoldController : ControllerBase
    {
        private readonly IQAService _mailQAService;
        private readonly IKeyService _keyService;

        public QAQgoldController(IQAService mailQAService, IKeyService keyService)
        {
            _mailQAService = mailQAService;
            _keyService = keyService;
        }

        /// <summary>
        /// Send question from customer to Qgold
        /// </summary>
        /// <remarks>
        /// client: OrderStullerQgoldApi, 90b34412-b58e-4f88-ab30-0c1da8a139d8-OrderAPI
        /// </remarks>
        /// <param name="question"></param>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SendMailQuestion([FromBody] QgoldQADTO question, [FromHeader] string clientId, [FromHeader] string clientSecret)
        {
            if (_keyService.ValidateCredentials(clientId, clientSecret))
            {
                return Ok(await _mailQAService.SendMailQuestion(question));
            }
            else
            {
                return Unauthorized();
            }
        }

        /// <summary>
        /// Get detail question
        /// </summary>
        /// <remarks>
        /// client: OrderStullerQgoldApi, 90b34412-b58e-4f88-ab30-0c1da8a139d8-OrderAPI
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <returns></returns>
        [HttpGet("DetailQuestion")]
        public async Task<IActionResult> DetailQuestion([FromQuery] string id, [FromHeader] string clientId, [FromHeader] string clientSecret)
        {
            if (_keyService.ValidateCredentials(clientId, clientSecret))
            {
                return Ok(await _mailQAService.DetailQuestion(id));
            }
            else
            {
                return Unauthorized();
            }
        }

        /// <summary>
        /// Get list question
        /// </summary>
        /// <remarks>
        /// client: OrderStullerQgoldApi, 90b34412-b58e-4f88-ab30-0c1da8a139d8-OrderAPI
        /// </remarks>
        /// <param name="seachParam"></param>
        /// <param name="paginationParam"></param>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetQuestion([FromQuery] SearchQgoldQAParam seachParam, [FromQuery] PaginationParam paginationParam,
            [FromHeader] string clientId, [FromHeader] string clientSecret)
        {
            if (_keyService.ValidateCredentials(clientId, clientSecret))
            {
                return Ok(await _mailQAService.GetQuestion(paginationParam,seachParam));
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
