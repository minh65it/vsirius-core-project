using Microsoft.AspNetCore.Mvc;
using VsiriusSystem._Services.Interfaces;
using VsiriusSystem.Dtos;
using VsiriusSystem.Helpers.Utilities;

namespace VsiriusSystem.Controllers
{
    [Route("api/system/[controller]")]
    [ApiController]
    public class JobSchedulerController : ControllerBase
    {
        private readonly IJobSchedulerService _jobSchedulerService;

        public JobSchedulerController(IJobSchedulerService jobSchedulerService)
        {
            _jobSchedulerService = jobSchedulerService;
        }
        [HttpGet]
        public async Task<PaginationUtility<JobInfoDto>> GetAll([FromQuery]PaginationParam pagination, [FromQuery] SearchJobInfoDto search)
        {
            return await _jobSchedulerService.GetAll(pagination, search);
        }
        [HttpGet("{jobId}")]
        public async Task<JobInfoDto> GetDetail(string jobId)
        {
            return await _jobSchedulerService.GetDetail(jobId);
        }
        [HttpDelete]
        public async Task<OperationResult> Delete(string jobId)
        {
            return await _jobSchedulerService.Delete(jobId);
        }
        [HttpPost]
        public async Task<OperationResult> Create(CreateJobInfoDto job)
        {
            return await _jobSchedulerService.Create(job);
        }
        [HttpPut]
        public async Task<OperationResult> Update(JobInfoDto job)
        {
            return await _jobSchedulerService.Update(job);
        }

        /// <summary>
        /// Update job từ BO hoặc các bên khác
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        [HttpPut("UpdateJobClient")]
        public async Task<OperationResult> UpdateJobClient(JobInfoDto job)
        {
            return await _jobSchedulerService.UpdateJobClient(job);
        }
    }
}
