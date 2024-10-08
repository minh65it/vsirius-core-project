using EtsyIntegrationDataAPI._Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EtsyIntegrationDataAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationEtsyController : ControllerBase
    {
        private readonly IAuthenticationEtsyService _authenticationEtsyService;

        public AuthenticationEtsyController(IAuthenticationEtsyService authenticationEtsyService)
        {
            _authenticationEtsyService = authenticationEtsyService;
        }

        /// <summary>
        /// Request an authorization code
        /// </summary>
        /// <remarks>
        /// scope=transactions_r%20transactions_w <br/>
        /// <br/>
        /// "address_r","address_w","billing_r","cart_r","cart_w","email_r","favorites_r","favorites_w","feedback_r","listings_d","listings_r","listings_w","profile_r","profile_w","recommend_r","recommend_w","shops_r","shops_w","transactions_r","transactions_w"
        /// <br/>
        /// <br/>
        /// Trả ra message = URL dùng để Grant Access, Grant Access sẽ trả ra Code và state truy cập vào https://localhost:7045/Authentication (Redirect_uri)
        /// </remarks>
        /// <param name="filterId"></param>
        /// <returns></returns>
        [HttpPost("RequestAnAuthorizationCode")]
        public async Task<IActionResult> RequestAnAuthorizationCode([FromBody] List<string> scope)
        {
            var res = await _authenticationEtsyService.RequestAnAuthorizationCode(scope);
            return Ok(res);
        }

        /// <summary>
        /// Get a access token from Esty
        /// </summary>
        /// <remarks>
        /// Cần truyền vào code, code này lấy từ data trả từ https://developer.etsy.com/documentation/essentials/authentication/#step-2-grant-access <br/>
        /// <br/>
        /// <br/>
        /// Bước này tạo Access Token
        /// </remarks>
        /// <param name="code"></param>
        /// <param name="code_verifier"></param>
        /// <returns></returns>
        [HttpPost("RequestAnAccessToken")]
        public async Task<IActionResult> RequestAnAccessToken([FromQuery] string? code, [FromQuery] string? code_verifier)
        {
            var res = await _authenticationEtsyService.RequestAnAccessToken(code, code_verifier);
            return Ok(res);
        }

        /// <summary>
        /// Get a new refresh token from Esty
        /// </summary>
        /// <remarks>
        /// Endponit này sẽ trả ra refesh token mới 
        /// </remarks>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        [HttpPost("RequestingARefreshToken")]
        public async Task<IActionResult> RequestingARefreshToken([FromQuery] string refreshToken)
        {
            var res = await _authenticationEtsyService.RequestingARefreshToken(refreshToken);
            return Ok(res);
        }
    }
}
