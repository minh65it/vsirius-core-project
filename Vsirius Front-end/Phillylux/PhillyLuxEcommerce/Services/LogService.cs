using PhillyLuxEcommerce.Dtos;
using PhillyLuxEcommerce.Services.Interfaces;

namespace PhillyLuxEcommerce.Services
{
    public class LogService : ILogService
    {
        private readonly IConfiguration _configuration;

        public LogService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> CreateLog(string url, string exception, LogLevel? level = LogLevel.Information, string? message = "")
        {
            try
            {
                var logDto = GetValueLog(message!, level!.ToString()!, exception, url);


                HttpClient client = new HttpClient();
                var json = System.Text.Json.JsonSerializer.Serialize(logDto);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Add("clientId", _configuration.GetSection("ApiVsiriusCredentials")["ClientId"]);
                client.DefaultRequestHeaders.Add("clientSecret", _configuration.GetSection("ApiVsiriusCredentials")["ClientSecret"]);
                HttpResponseMessage response = await client.PostAsync(_configuration["VsiriusSystemApi"] + "WriteLogs", content);

                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return true;
                }

                return false;
            }
            catch (Exception) { return false; }
        }

        private LogDto GetValueLog(string message, string level, string exception, string url)
        {
            LogDto logViewModel = new LogDto
            {
                Message = message,
                ProjectName = GetProjectName(),
                Level = level,
                Exception = exception,
                URL = url
            };
            return logViewModel;
        }

        private string GetProjectName()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string directoryName = Path.GetFileName(currentDirectory);
            return directoryName;
        }
    }
}
