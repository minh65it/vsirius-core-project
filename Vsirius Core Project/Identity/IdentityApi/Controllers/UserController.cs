using IdentityApi._Services.Interfaces;
using IdentityApi.Dtos.User;
using IdentityApi.Helpers.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApi.Controllers
{
    [Route("api/identity/[controller]")]
    [ApiController]
    
    public class UserController : ControllerBase
    {
        readonly private IUserService _userService;
        readonly private IUrlHelper _urlHelper;
        public UserController(IUserService userService, IUrlHelper urlHelper)
        {
            _userService = userService;
            _urlHelper = urlHelper;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserForLoginParam param)
        {
            var user = await _userService.Login(param);

            if (user == null)
                return Unauthorized();
            return Ok(user);
        }

        /// <summary>
        /// Get the paginated list of users, default is 10, page 1
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        [HttpGet("GetAllUser")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUser([FromQuery] PaginationParam pagination, [FromQuery] string textSearch)
        {
            var result = await _userService.GetAllUser(pagination, textSearch);
            return Ok(result);
        }

        /// <summary>
        /// Register user information, with correct email and phone number format, no image is null
        /// </summary>
        /// <remarks>
        /// cần có thông tin HttpContext.Request.Headers["OriginUrl"] để quay về trang frontend
        /// </remarks>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserDto dto)
        {
            // Lấy giá trị của một header cụ thể
            string originUrl = HttpContext.Request.Headers["OriginUrl"];

            // Xử lý logic dựa trên thông tin header
            var result = await _userService.CreateUser(dto, _urlHelper, originUrl);
            return Ok(result);
        }

        /// <summary>
        /// Email verification, email is the registered email account
        /// </summary>
        /// <remarks>
        /// cần có thông tin HttpContext.Request.Headers["OriginUrl"] để quay về trang frontend
        /// </remarks>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost("ConfirmEmail")]
        [Authorize(Roles = "Customer, Admin")]
        public async Task<IActionResult> ConfirmEmail([FromBody] string email)
        {
            // Lấy giá trị của một header cụ thể
            string originUrl = HttpContext.Request.Headers["OriginUrl"];

            // Xử lý logic dựa trên thông tin header
            await _userService.ConfirmEmail(email, _urlHelper, originUrl);
            return Ok();
        }

        /// <summary>
        /// Send a message to the user's phone number, which is found in the user information. Enter email to get user information
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost("SendSMS")]
        [Authorize(Roles = "Customer, Admin")]
        public async Task<IActionResult> SendSMS([FromBody] string email)
        {
            await _userService.SendSMS(email);
            return Ok();
        }

        /// <summary>
        /// Verify the user's phone number, email to find that account information, code to verify phone number
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("VerifySMS")]
        [Authorize(Roles = "Customer, Admin")]
        public async Task<IActionResult> VerifySMS([FromBody] VerifySMS dto)
        {
            var result = await _userService.VerifySMS(dto);
            return Ok(result);
        }
        /// <summary>
        /// Need information: id, phone, passwordhash
        /// </summary>
        /// <remarks>
        /// "id": "dad8445a-507e-4ac2-ab5d-9438232747d4",  "email": "abc@gmail.com", "phoneNumber": "+10975567777","passwordHash": "AQAAAAIAAYagAAAAEEyF6nmmP/w7yRQyuLbfCwdB7X+uLSNVDoVHqWcPUZ26TG6eHkXsPpdfkol5MN7DbQ==","avatar": null,...
        /// </remarks>
        /// <param name="userUpdate"></param>
        /// <returns></returns>
        [HttpPut("UpdateUser")]
        [Authorize(Roles = "Customer, Admin")]
        public async Task<IActionResult> UpdateUser([FromBody] UserDto userUpdate)
        {
            var result = await _userService.UpdateUser(userUpdate);
            return Ok(result);
        }

        /// <summary>
        /// Update password: id user, new password, old password
        /// </summary>
        /// <param name="userUpdate"></param>
        /// <returns></returns>
        [HttpPut("UpdatePassword")]
        [Authorize(Roles = "Customer, Admin")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDto userUpdate)
        {
            var result = await _userService.UpdatePassword(userUpdate);
            return Ok(result);
        }
        /// <summary>
        /// Update password: id user, new password
        /// </summary>
        /// <remarks>
        /// if new password is null then default password is "welcome123";
        /// </remarks>
        /// <param name="userUpdate"></param>
        /// <returns></returns>
        [HttpPut("UpdatePasswordByAdmin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdatePasswordByAdmin([FromBody] UpdatePasswordDto userUpdate)
        {
            var result = await _userService.UpdatePasswordByAdmin(userUpdate);
            return Ok(result);
        }

        /// <summary>
        /// Delete the user account through the ID stored in the database
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("DeleteUser")]
        [Authorize(Roles = "Customer, Admin")]
        public async Task<IActionResult> DeleteEmployee([FromBody] UserDto dto)
        {
            var result = await _userService.DeleteUser(dto.Id.ToString());
            return Ok(result);
        }
        [HttpPost("LogOut")]
        public async Task<IActionResult> LogOut()
        {
            var result = await _userService.LogOut();
            return Ok(result);
        }
        [HttpGet("GetUserDetail")]
        [Authorize(Roles = "Customer, Admin")]
        public async Task<IActionResult> GetUserDetail([FromQuery] string mail)
        {
            var result = await _userService.GetUserDetail(mail);
            return Ok(result);
        }

        /// <summary>
        /// Get information about states in the US
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetState")]
        public IActionResult GetState()
        {
            var result =  _userService.GetState();
            return Ok(result);
        }
    }
}
