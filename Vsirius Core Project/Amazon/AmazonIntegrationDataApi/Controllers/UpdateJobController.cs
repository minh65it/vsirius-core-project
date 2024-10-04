using AmazonIntegrationDataApi.Jobs;
using Microsoft.AspNetCore.Mvc;
using QuartzInfrastructure;

namespace AmazonIntegrationDataApi.Controllers
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
                case "Amazon_GetNewAmazonOrder":
                    await _updateJobsService.UpdateJob<Amazon_GetNewAmazonOrder>(job);
                    break;
                case "Amazon_MapPIMToAmazonDB":
                    await _updateJobsService.UpdateJob<Amazon_MapPIMToAmazonDB>(job);
                    break;
                case "Amazon_ReturnAmazonOrder":
                    await _updateJobsService.UpdateJob<Amazon_ReturnAmazonOrder>(job);
                    break;
                case "Amazon_SubmitOrderAmazonToQgold":
                    await _updateJobsService.UpdateJob<Amazon_SubmitOrderAmazonToQgold>(job);
                    break;
                case "Amazon_UpdateInventoryToAmazon":
                    await _updateJobsService.UpdateJob<Amazon_UpdateInventoryToAmazon>(job);
                    break;
                case "Amazon_UpdatePriceToAmazon":
                    await _updateJobsService.UpdateJob<Amazon_UpdatePriceToAmazon>(job);
                    break;
                case "Amazon_UpdateTrackingOrderAmazon":
                    await _updateJobsService.UpdateJob<Amazon_UpdateTrackingOrderAmazon>(job);
                    break;
                case "Amazon_UploadProductToAmazon":
                    await _updateJobsService.UpdateJob<Amazon_UploadProductToAmazon>(job);
                    break;
                default:
                    break;
            }
            return Ok("Done");
        }
    }
}
