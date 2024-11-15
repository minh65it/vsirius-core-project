using CSRApi._Services.Interfaces;
using CSRApi.Dtos;
using CSRApi.Helpers.ConfigSignalR;
using CSRApi.Helpers.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace VsiriusApi.Controllers.CSRApi
{
    [ApiExplorerSettings(GroupName = "CSR")]
    [Route("csr/api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _service;
        private readonly IHubContext<NotificationHub> _notificationHubContext;
        public NotificationsController(INotificationService service, IHubContext<NotificationHub> notificationHubContext)
        {
            _service = service;
            _notificationHubContext = notificationHubContext;
        }

        [HttpPost("Create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(NotificationDto dto)
        {
            dto.CreateTime = DateTime.Now;
            var data = await _service.Create(dto);
            await _notificationHubContext.Clients.All.SendAsync("SendNotification", data.Data);
            return Ok(data);
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination, string keyword)
        {
            return Ok(await _service.GetData(pagination, keyword));
        }

        [HttpGet("GetNotificationByUser")]
        public async Task<IActionResult> GetNotificationByUser([FromQuery] PaginationParam pagination, string email)
        {
            return Ok(await _service.GetNotificationByUser(pagination, email));
        }

      
        [HttpGet("GetNotificationMain")]
        public async Task<IActionResult> GetNotificationMain()
        {
            return Ok(await _service.GetNotificationMain());
        }
        [HttpGet("NotReadNotifications")]
        public async Task<IActionResult> GetNotificationNotRead(string email)
        {
            return Ok(await _service.GetNotificationNotRead(email));
        }


        [HttpDelete("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            return Ok(await _service.Delete(id));
        }

      
        [HttpPut("UpdateView")]
        public async Task<IActionResult> UpdateView(string email, string notificationId)
        {
            return Ok(await _service.UpdateView(email,notificationId));
        }
    }
}
