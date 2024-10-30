using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PimApi.Jobs;
using QuartzInfrastructure;

namespace VsiriusApi.Controllers.PimApi
{
    [ApiExplorerSettings(GroupName = "Pim")]
    [Route("[controller]")]
    [ApiController]
    public class UpdateJobController : ControllerBase
    {
        private readonly IUpdateJobsService _updateJobsService;

        public UpdateJobController(IUpdateJobsService updateJobsService)
        {
            _updateJobsService = updateJobsService;
        }

        [HttpPut]
        public async Task<IActionResult> Update(JobInfoDto job)
        {
            switch (job.Tittle)
            {
                case "PIM_ETLOrder":
                    await _updateJobsService.UpdateJob<PIM_ETLOrder>(job);
                    break;
                default:
                    break;
            }
            return Ok("Done");
        }
    }
}
