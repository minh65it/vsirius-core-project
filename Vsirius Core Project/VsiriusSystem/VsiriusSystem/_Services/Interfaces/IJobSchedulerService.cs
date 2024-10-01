using VsiriusSystem.Dtos;
using VsiriusSystem.Helpers.Utilities;

namespace VsiriusSystem._Services.Interfaces
{
    public interface IJobSchedulerService
    {
        Task<PaginationUtility<JobInfoDto>> GetAll(PaginationParam pagination, SearchJobInfoDto search);
        Task<JobInfoDto> GetDetail(string jobId);
        Task<OperationResult> Delete(string jobId);
        Task<OperationResult> Create(CreateJobInfoDto job);
        Task<OperationResult> Update(JobInfoDto job);
        Task<OperationResult> UpdateJobClient(JobInfoDto job);
    }
}
