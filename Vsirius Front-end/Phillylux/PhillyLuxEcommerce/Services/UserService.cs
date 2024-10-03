using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json;
using PhillyLuxEcommerce.Dtos;
using PhillyLuxEcommerce.Services.Interfaces;
using PhillyLuxEcommerce.ViewModels;
using System.Security.Claims;
using System.Text;

namespace PhillyLuxEcommerce.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogService _logService;

        public UserService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, ILogService logService)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _logService = logService;
        }
        public async Task<UserDto?> GetUserByEmail()
        {
            var email = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;
            try
            {
                var httpClient = _httpClientFactory.CreateClient("UserInfo");

                var httpResponseMessage = await httpClient.GetAsync($"?mail={email}");

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string json = await httpResponseMessage.Content.ReadAsStringAsync();
                    UserDto? data = JsonConvert.DeserializeObject<UserDto>(json);
                    return data;
                }

            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext!.Request.Path, exception: ex.Message, level: LogLevel.Error);
            }

            return null;
        }

        public async Task<bool> SendOTP()
        {
            try
            {
                var email = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;
                var httpClient = _httpClientFactory.CreateClient("SendOTP");
                var json = JsonConvert.SerializeObject(email);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var httpResponseMessage = await httpClient.PostAsync($"", content);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var reponse = await httpResponseMessage.Content.ReadAsStringAsync();
                    dynamic? data = JsonConvert.DeserializeObject(reponse);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext!.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return false;
            }
        }

        public async Task<(bool, string?)> UpdateUser(UserVM user)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("UpdateUser");
                var json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var httpResponseMessage = await httpClient.PutAsync($"", content);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var reponse = await httpResponseMessage.Content.ReadAsStringAsync();
                    dynamic? data = JsonConvert.DeserializeObject(reponse);

                    return (data?.success, data?.message);
                }
                return (false, "Connect failed");
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext!.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return (false, "Connect failed");
            }
        }

        public async Task<bool> VerifyEmail(string email)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("ConfirmEmail");
                var request = _httpContextAccessor.HttpContext?.Request;
                string url = $"{request?.Scheme}://{request?.Host}";
                httpClient.DefaultRequestHeaders.Add("OriginUrl", url);
                var json = JsonConvert.SerializeObject(email);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var httpResponseMessage = await httpClient.PostAsync($"", content);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var reponse = await httpResponseMessage.Content.ReadAsStringAsync();
                    dynamic? data = JsonConvert.DeserializeObject(reponse);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext!.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return false;
            }
        }

        public async Task<bool> VerifyPhone(string code)
        {
            try
            {
                var email = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;
                var httpClient = _httpClientFactory.CreateClient("ConfirmPhone");

                var requestBody = new
                {
                    email,
                    code,
                };

                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var httpResponseMessage = await httpClient.PostAsync($"", content);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var reponse = await httpResponseMessage.Content.ReadAsStringAsync();
                    dynamic? data = JsonConvert.DeserializeObject(reponse);
                    return data?.success;
                }
                return false;
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext!.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return false;
            }
        }

    }
}
