using EtsyIntegrationDataAPI.Jobs;
using Microsoft.AspNetCore.Mvc;
using QuartzInfrastructure;

namespace EtsyIntegrationDataAPI.Controllers
{
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
                case "Esty_MapPIMToEstyDB":
                    await _updateJobsService.UpdateJob<Etsy_MapPIMToEtsyDB>(job);
                    break;
                case "Esty_CreateListDraftAndPublishListing":
                    await _updateJobsService.UpdateJob<Etsy_CreateListDraftAndPublishListing>(job);
                    break;
                case "Esty_UpdateAllListingInventory":
                    await _updateJobsService.UpdateJob<Etsy_UpdateAllListingInventory>(job);
                    break;
                case "Esty_GetNewOrderEsty":
                    await _updateJobsService.UpdateJob<Etsy_GetNewOrderEtsy>(job);
                    break;
                case "Etsy_UpdateProperties":
                    await _updateJobsService.UpdateJob<Etsy_UpdateProperties>(job);
                    break;
                case "Etsy_UpdateTrackingOrder":
                    await _updateJobsService.UpdateJob<Etsy_UpdateTrackingOrder>(job);
                    break;
                default:
                    break;
            }
            return Ok("Done");
        }
    }
}
