using AutoMapper;
using Newtonsoft.Json;
using PhillyLuxEcommerce.Dtos;
using PhillyLuxEcommerce.Helpers.Utilities;
using PhillyLuxEcommerce.Services.Interfaces;
using PhillyLuxEcommerce.ViewModels;
using System.Text;

namespace PhillyLuxEcommerce.Services
{
    public class PageService : IPageService
    {
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogService _logService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PageService(IMapper mapper, IHttpClientFactory httpClientFactory, ILogService logService, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
            _logService = logService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<OperationResult> AddContact(ContactVM contactVM)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("Contact");

                var resultContact = _mapper.Map<ContactDto>(contactVM);

                resultContact.Id = Guid.Parse("00000000-0000-0000-0000-000000000000");
                resultContact.CreateTime = DateTime.Now;
                resultContact.UpdateTime = DateTime.Now;
                resultContact.Status = false;
                var json = JsonConvert.SerializeObject(resultContact);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync("Create", content);
                if (response.IsSuccessStatusCode)
                {
                    return new OperationResult { Success = true };
                }

                return new OperationResult { Success = true };
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<List<PageInfoDto>>? GetListPage()
        {
            List<PageInfoDto> result = null;
            try
            {
                var httpClient = _httpClientFactory.CreateClient("GetAllPages");

                HttpResponseMessage response = await httpClient.GetAsync("");

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<List<PageInfoDto>>(responseBody);
                }
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
            }
            return result;
        }

        public async Task<OperationResult> Page(int code)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("GetPage");

                HttpResponseMessage response = await httpClient.GetAsync($"?id={code}");

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<PageInfoDto>(responseBody);

                    return new OperationResult { Success = true, Data = result };
                }
                return new OperationResult { Success = false };
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }
    }
}
