using IdentityApi._Services.Interfaces;
using IdentityApi.Dtos.Auth;
using IdentityApi.Helpers.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VsiriusApi.Controllers.IdentityApi
{
    [ApiExplorerSettings(GroupName = "Identity")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, Employee")]
    public class EmployeeController : ControllerBase
    {
        readonly private IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService EmployeeService)
        {
            _employeeService = EmployeeService;
        }

        /// <summary>
        /// The created account must have the correct email and phone number format, the image without it will be null
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("CreateEmployee")]
        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeDto dto)
        {
            var result = await _employeeService.CreateEmployee(dto);
            return Ok(result);
        }

        /// <summary>
        /// create 1 permission with a certain name
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole([FromBody] RolesEmployeeDto roles)
        {
            var result = await _employeeService.CreateRole(roles);
            return Ok(result);
        }
        /// <summary>
        /// create multiple default permissions
        /// </summary>
        /// <remarks>
        /// Admin,
        /// Employee,
        /// Product,
        /// Order,
        /// Contact,
        /// Cart,
        /// System,
        /// Notification,
        /// DrivenHTML
        /// </remarks>
        /// <param name="roles"></param>
        /// <returns></returns>
        [HttpPost("CreateAllRole")]
        public async Task<IActionResult> CreateAllRole()
        {
            var result = await _employeeService.CreateAllRole();
            return Ok(result);
        }
        [HttpDelete("DeleteEmployee")]
        public async Task<IActionResult> DeleteUser([FromQuery] string id)
        {
            var result = await _employeeService.DeleteEmployee(id);
            return Ok(result);
        }

        /// <summary>
        /// Delete permission via ID in the database, admin rights can't be deleted
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteRole")]
        public async Task<IActionResult> DeleteRole([FromQuery] string id)
        {
            var result = await _employeeService.DeleteRole(id);
            return Ok(result);
        }
        [HttpGet("GetAllEmployee")]
        public async Task<IActionResult> GetAllEmployee([FromQuery] PaginationParam pagination, [FromQuery] string textSearch)
        {
            var result = await _employeeService.GetAllEmployee(pagination, textSearch);
            return Ok(result);
        }
        [HttpGet("GetAllRole")]
        public async Task<IActionResult> GetAllRole()
        {
            var result = await _employeeService.GetAllRole();
            return Ok(result);
        }
        [HttpGet("GetDetailRole")]
        public async Task<IActionResult> GetDetailRole(string id)
        {
            var result = await _employeeService.GetDetailRole(id);
            return Ok(result);
        }
        [HttpGet("GetEmployeeDetail")]
        public async Task<IActionResult> GetEmployeeDetail([FromQuery] string mail)
        {
            var result = await _employeeService.GetEmployeeDetail(mail);
            return Ok(result);
        }

        /// <summary>
        /// The update account must have the correct email and phone number format, the image without it will be null
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("UpdateEmployee")]
        public async Task<IActionResult> UpdateEmployee([FromBody] EmployeeDto employeeUpdate)
        {
            var result = await _employeeService.UpdateEmployee(employeeUpdate);
            return Ok(result);
        }

        /// <summary>
        /// Admin rights cannot be edited
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        [HttpPut("UpdateRole")]
        public async Task<IActionResult> UpdateRole([FromBody] RolesEmployeeDto roles)
        {
            var result = await _employeeService.UpdateRole(roles);
            return Ok(result);
        }
    }
}
