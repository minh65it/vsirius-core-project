using VsiriusSystem.Dtos;
using VsiriusSystem.Helpers.Params;
using VsiriusSystem.Helpers.Utilities;

namespace VsiriusSystem._Services.Interfaces
{
    public interface ILogService
    {
        Task<PaginationUtility<LogDto>> GetListLog(PaginationParam pagination, LogParams logParam, string? keyword, bool isPaging = true);
        Task<LogDto> GetLog(Guid logId);
        Task<OperationResult> CreateLog(LogDto logViewModel);
        Task<List<string>> FilterProjectName();
        Dictionary<int, string> GetLevelLog();
        Task<List<string>> FilterURL(string projectName);
    }
}
