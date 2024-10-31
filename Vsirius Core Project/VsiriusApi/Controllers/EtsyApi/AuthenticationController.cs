using EtsyIntegrationDataAPI._Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace VsiriusApi.Controllers.EtsyApi
{
    [ApiExplorerSettings(GroupName = "Etsy")]
    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationEtsyService _authenticationEtsyService;

        public AuthenticationController(IAuthenticationEtsyService authenticationEtsyService)
        {
            _authenticationEtsyService = authenticationEtsyService;
        }

        /// <summary>
        /// Redirect_uri
        /// </summary>
        /// <remarks>
        /// Esty call this endpoint
        /// </remarks>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpGet()]
        public async Task<IActionResult> GrantAccess([FromQuery] string code, [FromQuery] string state)
        {
            var res = await _authenticationEtsyService.GrantAccess(code, state);
            return Ok(res);
        }
    }
}
