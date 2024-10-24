using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Jobs;
using QuartzInfrastructure;

namespace VsiriusApi.Controllers.ProductApi
{
    [ApiExplorerSettings(GroupName = "Product")]
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
                case "VSiriusWeb_MapPIMToVSiriusWebDB":
                    await _updateJobsService.UpdateJob<VSiriusWeb_MapPIMToVSiriusWebDB>(job);
                    break;
                case "VSiriusWeb_UpdateFilter":
                    await _updateJobsService.UpdateJob<VSiriusWeb_UpdateFilter>(job);
                    break;
                default:
                    break;
            }
            return Ok("Done");
        }
    }
}
