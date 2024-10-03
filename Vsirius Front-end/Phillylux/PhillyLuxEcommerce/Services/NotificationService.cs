using Newtonsoft.Json;
using PhillyLuxEcommerce.Dtos;
using PhillyLuxEcommerce.Helpers.Utilities;
using PhillyLuxEcommerce.Services.Interfaces;
using System.Text;

namespace PhillyLuxEcommerce.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogService _logService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public NotificationService(IHttpClientFactory httpClientFactory, ILogService logService, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _logService = logService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<NotificationDto> GetTopNotification()
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("TopNotificaion");

                var httpResponseMessage = await httpClient.GetAsync("");

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string json = await httpResponseMessage.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<NotificationDto>(json);
                    return data;
                }
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
            }

            return null;
        }
        public async Task<PaginationUtility<NotificationDto>> GetAllNotification(string email, int PageNumber = 1, int PageSize = 10)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("AllNotification");

                var httpResponseMessage = await httpClient.GetAsync($"?email={email}&PageNumber={PageNumber}&PageSize={PageSize}");

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string json = await httpResponseMessage.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<PaginationUtility<NotificationDto>>(json);
                    return data;
                }
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
            }

            return null;
        }

        public async Task<List<NotificationDto>> GetNotificationNotRead(string email)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("NotReadNotification");

                var httpResponseMessage = await httpClient.GetAsync($"?email={email}");

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string json = await httpResponseMessage.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<List<NotificationDto>>(json);
                    return data;
                }
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
            }
            return null;
        }

        public async Task<OperationResult> UpdateNotificationByUser(string email, string notificationId)
        {
            try
            {
                //var obj = new { Email = email, NotificationId = notificationId };

                var httpClient = _httpClientFactory.CreateClient("MarkNotification");
                //var json = JsonConvert.SerializeObject(obj);
                //var content = new StringContent(json, Encoding.UTF8, "application/json");
                var content = new StringContent("", Encoding.UTF8, "application/json");

                var httpResponseMessage = await httpClient.PutAsync($"?email={email}&notificationId={notificationId}", content);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var reponse = await httpResponseMessage.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<OperationResult>(reponse);
                    return data;
                }
                return new OperationResult(false);
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new OperationResult(false, ex.Message);
            }
        }
    }
}
