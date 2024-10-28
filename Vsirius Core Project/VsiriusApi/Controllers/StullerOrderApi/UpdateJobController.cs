using Microsoft.AspNetCore.Mvc;
using QuartzInfrastructure;
using StullerOrderProcessAPI.Jobs;

namespace VsiriusApi.Controllers.StullerOrderApi
{
    [ApiExplorerSettings(GroupName = "StullerOrder")]
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
                case "Stuller_GetOrder":
                    await _updateJobsService.UpdateJob<Stuller_GetOrder>(job);
                    break;
                default:
                    break;
            }
            return Ok("Done");
        }
    }
}
