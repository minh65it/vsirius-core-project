using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;
using VsiriusSystem._Services.Interfaces;
using VSiriusSystemLog.CorrelationId;

namespace VSiriusSystemLog.WriteLog
{
    public interface ILoggerCustom
    {
        public Task<bool> CreateLogInformation(string? message = "");
        public Task<bool> CreateLogWarning(string? message = "");
        public Task<bool> CreateLogError(string? exception, string? message = "");
    }
    public class LoggerCustom : ILoggerCustom
    {
        private readonly IConfiguration _configuration;
        private readonly ICorrelationIdGenerator _correlationIdGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogService _logService;
        private readonly IMapper _mapper;

        public LoggerCustom(
            IConfiguration configuration, 
            IHttpContextAccessor httpContextAccessor, 
            ICorrelationIdGenerator correlationIdGenerator,
            ILogService logService,
            IMapper mapper)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _correlationIdGenerator = correlationIdGenerator;
            _logService = logService;
            _mapper = mapper;
        }
        private async Task<bool> CallAPI(LogDto logDto)
        {
            try
            {
               
                HttpClient client = new HttpClient();
                var json = System.Text.Json.JsonSerializer.Serialize(logDto);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                client.DefaultRequestHeaders.Add("X-Correlation-Id", logDto.CorrelationId.ToString());
                client.DefaultRequestHeaders.Add("clientId", _configuration.GetSection("ApiVsiriusCredentialsSystem")["ClientId"]);
                client.DefaultRequestHeaders.Add("clientSecret", _configuration.GetSection("ApiVsiriusCredentialsSystem")["ClientSecret"]);

                HttpResponseMessage response = await client.PostAsync(_configuration["VsiriusSystemApi"] + "Log/WriteLogs", content);
                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                    
                return false;
            }
        }

        private async Task<bool> CallAPI_RFT(LogDto logDto)
        {
            try
            {
                VsiriusSystem.Dtos.LogDto dto = _mapper.Map<VsiriusSystem.Dtos.LogDto>(logDto);
                var result = await _logService.CreateLog(dto);
                return result.Success;
            }
            catch (Exception)
            {
                return false;
            }
            
        }
        public async Task<bool> CreateLogError(string? exception, string? message = "")
        {
            try
            {
                string correlationId = _correlationIdGenerator.Get();
                string url = "vsirius/api";
                var correlationGuid = new Guid(correlationId);
                var logDto = GetValueLog(correlationGuid, message, LogLevel.Error.ToString(), exception, url);

                return await CallAPI_RFT(logDto);
            }
            catch (Exception) { return false; }
        }
        public async Task<bool> CreateLogInformation(string? message = "")
        {
            try
            {
                string correlationId = _correlationIdGenerator.Get();
                string url = "vsirius/api";
                var correlationGuid = new Guid(correlationId);
                var logDto = GetValueLog(correlationGuid, message, LogLevel.Information.ToString(), null, url);

                return await CallAPI_RFT(logDto);
            }
            catch (Exception) { return false; }
        }

        public async Task<bool> CreateLogWarning(string? message = "")
        {
            try
            {
                string correlationId = _correlationIdGenerator.Get();
                string url = "vsirius/api";
                var correlationGuid = new Guid(correlationId);
                var logDto = GetValueLog(correlationGuid, message, LogLevel.Warning.ToString(), null, url);

                return await CallAPI_RFT(logDto);
            }
            catch (Exception) { return false; }
        }

        private LogDto GetValueLog(Guid correlationGuid, string message, string level, string? exception, string url)
        {
            LogDto logViewModel = new LogDto
            {
                Message = message,
                ProjectName = GetProjectName(),
                Level = level,
                Exception = exception,
                URL = url,
                CorrelationId = correlationGuid
            };
            return logViewModel;
        }

        private string GetProjectName()
        {
            string name = _configuration.GetSection("ApiVsiriusCredentialsSystem")["ClientId"];
            return name;
        }
    }
}
