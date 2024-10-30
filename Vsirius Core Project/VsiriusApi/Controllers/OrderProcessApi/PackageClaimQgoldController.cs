using Microsoft.AspNetCore.Mvc;
using OrderProcessApi._Services.IServices;
using OrderProcessApi._Services.IServices.Qgold;
using OrderProcessApi.DTOs;

namespace VsiriusApi.Controllers.OrderProcessApi
{
    [ApiExplorerSettings(GroupName = "QgoldOrder")]
    [Route("api/[controller]")]
    [ApiController]
    public class PackageClaimQgoldController : ControllerBase
    {
        private readonly IPackageClaimService _packageClaimService;
        private readonly IKeyService _keyService;

        public PackageClaimQgoldController(IPackageClaimService packageClaimService, IKeyService keyService)
        {
            _packageClaimService = packageClaimService;
            _keyService = keyService;
        }

        /// <summary>
        /// send mail claims order to Qgold
        /// </summary>
        /// <remarks>
        /// client: OrderStullerQgoldApi, 90b34412-b58e-4f88-ab30-0c1da8a139d8-OrderAPI
        /// </remarks>
        /// <param name="order"></param>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SendMailLostPackage([FromBody] QgoldOrderDetailDTO order, [FromHeader] string clientId, [FromHeader] string clientSecret)
        {
            if (_keyService.ValidateCredentials(clientId, clientSecret))
            {
                return Ok(await _packageClaimService.SendMailLostPackage(order));
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
