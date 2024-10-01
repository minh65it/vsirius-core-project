using OrdersAPI._Services.Interfaces;
using OrdersAPI.Dtos;
using Microsoft.AspNetCore.Mvc;
using OrdersAPI.Helpers.Utilities;

namespace OrdersAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VsiriusCartController : ControllerBase
    {
        private readonly IVsiriusCartService _cartService;
        private readonly IHttpHeaderService _httpHeaderService;
        private readonly IKeyService _keyService;
        public VsiriusCartController(IVsiriusCartService cartService, IHttpHeaderService httpHeaderService, IKeyService keyService)
        {
            _cartService = cartService;
            _httpHeaderService = httpHeaderService;
            _keyService = keyService;
        }

        /// <summary>
        /// Danh sách tất cả các cart
        /// </summary>
        /// <remarks>
        /// Client id: OrderApi | Secret: eyJhbGciOiJub25lIiwidHlwIjoiSldUIn0.eyJkYXRhIjoiN2MwMTgxOTMtN2I4Yi00ODNmLTllZDAtNTUxZjk5NTQ1YzI2LU9yZGVyQXBpIiwibmJmIjoxNjg3NTA3NDAzLCJleHAiOjE2ODc1MTEwMDMsImlhdCI6MTY4NzUwNzQwM30."
        /// </remarks>
        /// <param name="fullTextSearch">Nhập keyword tìm kiếm</param>
        /// <param name="clientId">Nhập Client Id</param>
        /// <param name="clientSecret">Nhập Client Secret</param>
        /// <returns></returns>
        [HttpGet("ListCartAll")]
        public async Task<IActionResult> ListCartAll([FromQuery] PaginationParam pagination, string? fullTextSearch, [FromHeader] string clientId, [FromHeader] string clientSecret)
        {
            if (_keyService.ValidateCredentials(clientId, clientSecret))
            {
                var res = await _cartService.ListCartAll(pagination, fullTextSearch);
                return Ok(res);
            }
            else
            {
                return Unauthorized();
            }
        }

        /// <summary>
        /// Lấy dữ liệu giỏ hàng chi tiết theo giỏ hàng id
        /// </summary>
        /// Client id: OrderApi | Secret: eyJhbGciOiJub25lIiwidHlwIjoiSldUIn0.eyJkYXRhIjoiN2MwMTgxOTMtN2I4Yi00ODNmLTllZDAtNTUxZjk5NTQ1YzI2LU9yZGVyQXBpIiwibmJmIjoxNjg3NTA3NDAzLCJleHAiOjE2ODc1MTEwMDMsImlhdCI6MTY4NzUwNzQwM30."
        /// <param name="cartId">Nhập cartId</param>
        /// <param name="isDelete">Truyền true hoặc false</param>
        /// <param name="clientId">Nhập Client Id</param>
        /// <param name="clientSecret">Nhập Client Secret</param>
        /// <returns></returns>
        [HttpGet("GetCartDetailByCartId")]
        public async Task<IActionResult> GetCartDetailByCartId(Guid cartId, bool isDelete, [FromHeader] string clientId, [FromHeader] string clientSecret)
        {
            if (_keyService.ValidateCredentials(clientId, clientSecret))
            {
                var res = await _cartService.GetCartDetailByCartId(cartId, isDelete);
                 return Ok(res);
            }
            else
            {
                return Unauthorized();
            }

        }
        /// <summary>
        /// Lấy dữ liệu giỏ hàng của người dùng, 
        /// ngoài ra lấy giỏ hàng của người dùng chưa login qua bên người dùng login
        /// </summary>
        /// <remarks>
        /// userId : 5ff72905-7a15-4a86-9462-ff09df6da594 |
        /// email : example@gmail.com |
        /// Client id: OrderApi | Secret: eyJhbGciOiJub25lIiwidHlwIjoiSldUIn0.eyJkYXRhIjoiN2MwMTgxOTMtN2I4Yi00ODNmLTllZDAtNTUxZjk5NTQ1YzI2LU9yZGVyQXBpIiwibmJmIjoxNjg3NTA3NDAzLCJleHAiOjE2ODc1MTEwMDMsImlhdCI6MTY4NzUwNzQwM30."
        /// </remarks>
        /// <param name="userId">Nhập Id người dùng</param>
        /// <param name="email">Email của dùng</param>
        /// <param name="clientId">Nhập Client Id</param>
        /// <param name="clientSecret">Nhập Client Secret</param>
        /// <returns></returns>
        [HttpGet("ListCartByUser")]
        public async Task<IActionResult> ListCartByUser(Guid userId, string? email, [FromHeader] string clientId, [FromHeader] string clientSecret)
        {
            if (_keyService.ValidateCredentials(clientId, clientSecret))
            {
                var res = await _cartService.ListCartByUser(userId, email);
                return Ok(res);
            }
            else
            {
                return Unauthorized();
            }
              
        }

        /// <summary>
        /// Thêm và xóa giỏ hàng 
        /// </summary>
        /// <remarks>
        /// ProductId : S-51252:166:P |
        /// Quantity : 1 |
        /// UserId : 5ff72905-7a15-4a86-9462-ff09df6da594 |
        /// Client id: OrderApi | Secret: eyJhbGciOiJub25lIiwidHlwIjoiSldUIn0.eyJkYXRhIjoiN2MwMTgxOTMtN2I4Yi00ODNmLTllZDAtNTUxZjk5NTQ1YzI2LU9yZGVyQXBpIiwibmJmIjoxNjg3NTA3NDAzLCJleHAiOjE2ODc1MTEwMDMsImlhdCI6MTY4NzUwNzQwM30."
        /// </remarks>
        /// <param name="clientId">Nhập Client Id</param>
        /// <param name="clientSecret">Nhập Client Secret</param>
        /// <returns></returns>
        [HttpPost("AddCartOrDelete")]
        public async Task<IActionResult> AddCartOrDelete([FromForm] VsiriusCartDetailsDto dto, [FromHeader] string clientId, [FromHeader] string clientSecret)
        {
            if (_keyService.ValidateCredentials(clientId, clientSecret))
            {
                var headers = _httpHeaderService.GetHeaders();
                 var res = await _cartService.AddCartOrDelete(dto, headers);
                 return Ok(res);
            }
            else
            {
                return Unauthorized();
            }
        }

    }
}
