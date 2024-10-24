using CSRApi._Services.Interfaces;
using CSRApi.Dtos;
using CSRApi.Helpers.ConfigSignalR;
using CSRApi.Helpers.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CSRApi.Controllers
{
    [Route("api/csr/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _service;
        private readonly IHubContext<ContactHub> _contactHubContext;
        public ContactsController(IContactService service, IHubContext<ContactHub> contactHubContext)
        {
            _service = service;
            _contactHubContext = contactHubContext;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(ContactDto dto)
        {
            dto.CreateTime = DateTime.Now;
            var data = await _service.Create(dto);
            await _contactHubContext.Clients.All.SendAsync("ContactNotification", data.Data);
            return Ok(data);
        }

        [HttpGet("GetData")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination, string keyword)
        {
            return Ok(await _service.GetData(pagination, keyword));
        }

        [HttpGet("GetDetail")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDetail(string id)
        {
            return Ok(await _service.GetDetail(id));
        }

        [HttpDelete("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            return Ok(await _service.Delete(id));
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(ContactDto dto)
        {
            return Ok(await _service.Update(dto));
        }
    }
}
