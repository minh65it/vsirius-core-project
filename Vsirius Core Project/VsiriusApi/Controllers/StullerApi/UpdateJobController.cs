using Microsoft.AspNetCore.Mvc;
using QuartzInfrastructure;
using StullerDataApi.Jobs;

namespace VsiriusApi.Controllers.Stuller
{
    [ApiExplorerSettings(GroupName = "Stuller")]
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
                case "Stuller_MapStullerToPIM":
                    await _updateJobsService.UpdateJob<Stuller_MapStullerToPIM>(job);
                    break;
                case "Stuller_PullDataStuller":
                    await _updateJobsService.UpdateJob<Stuller_PullDataStuller>(job);
                    break;
                default:
                    break;
            }
            return Ok("Done");
        }
    }
}
