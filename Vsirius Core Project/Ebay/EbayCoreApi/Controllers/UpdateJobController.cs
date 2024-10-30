using EbayApi.JobInfratructure.Jobs;
using EbayCoreApi.JobServices.Jobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuartzInfrastructure;

namespace EbayCoreApi.Controllers
{
    [Route("api/ebay/[controller]")]
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
                case "Ebay_GetOrderFromEbay":
                    await _updateJobsService.UpdateJob<Ebay_GetOrderFromEbay>(job);
                    break;
                case "Ebay_MapperPimProductToEbayProduct":
                    await _updateJobsService.UpdateJob<Ebay_MapperPimProductToEbayProduct>(job);
                    break;
                case "Ebay_SubmitEbayOrder":
                    await _updateJobsService.UpdateJob<Ebay_SubmitEbayOrder>(job);
                    break;
                case "Ebay_UpdatePriceQuantityMarketplace":
                    await _updateJobsService.UpdateJob<Ebay_UpdatePriceQuantityMarketplace>(job);
                    break;
                case "Ebay_UpdateTrackingOrder":
                    await _updateJobsService.UpdateJob<Ebay_UpdateTrackingOrder>(job);
                    break;
                case "Ebay_UploadProductToSeller":
                    await _updateJobsService.UpdateJob<Ebay_UploadProductToSeller>(job);
                    break;
                default:
                    break;
            }
            return Ok("Done");
        }
    }
}
