namespace PhillyLuxEcommerce.Services.Interfaces
{
    public interface ILogService
    {
        Task<bool> CreateLog(string url, string exception, LogLevel? level = LogLevel.Information, string? message = "");
    }
}
