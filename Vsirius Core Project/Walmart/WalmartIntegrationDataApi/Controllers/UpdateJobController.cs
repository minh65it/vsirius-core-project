using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuartzInfrastructure;
using WalmartIntegrationDataApi.Job;

namespace WalmartIntegrationDataApi.Controllers
{
    [Route("walmart/[controller]")]
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
                case "Walmart_ApplyShippingTemplate":
                    await _updateJobsService.UpdateJob<Walmart_ApplyShippingTemplate>(job);
                    break;
                case "Walmart_GetOrder":
                    await _updateJobsService.UpdateJob<Walmart_GetOrder>(job);
                    break;
                case "Walmart_GetReturnOrder":
                    await _updateJobsService.UpdateJob<Walmart_GetReturnOrder>(job);
                    break;
                case "Walmart_MapVsiriusToWalmart":
                    await _updateJobsService.UpdateJob<Walmart_MapVsiriusToWalmart>(job);
                    break;
                case "Walmart_SubmitOrderToQgold":
                    await _updateJobsService.UpdateJob<Walmart_SubmitOrderToQgold>(job);
                    break;
                case "Walmart_UpdateInventory":
                    await _updateJobsService.UpdateJob<Walmart_UpdateInventory>(job);
                    break;
                case "Walmart_UpdatePrice":
                    await _updateJobsService.UpdateJob<Walmart_UpdatePrice>(job);
                    break;
                case "Walmart_UpdateTrackingOrder":
                    await _updateJobsService.UpdateJob<Walmart_UpdateTrackingOrder>(job);
                    break;
                case "Walmart_UploadProductToSeller":
                    await _updateJobsService.UpdateJob<Walmart_UploadProductToSeller>(job);
                    break;
                default:
                    break;
            }
            return Ok("Done");
        }
    }
}
