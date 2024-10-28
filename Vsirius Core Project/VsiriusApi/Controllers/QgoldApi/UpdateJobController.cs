using Microsoft.AspNetCore.Mvc;
using QgoldIntegrationDataApi.Jobs;
using QuartzInfrastructure;

namespace VsiriusApi.Controllers.QgoldApi
{
    [ApiExplorerSettings(GroupName = "Qgold")]
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
                case "Qgold_MappingQgoldToPIM":
                    await _updateJobsService.UpdateJob<Qgold_MappingQgoldToPIM>(job);
                    break;
                case "Qgold_PullProduct":
                    await _updateJobsService.UpdateJob<Qgold_PullProduct>(job);
                    break;
                default:
                    break;
            }
            return Ok("Done");
        }
    }
}
