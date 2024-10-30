using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using PimApi._Services.Interfaces;
using PimApi.Dtos.Variation;

namespace PimApi.Controllers
{
    [Route("api/pim/[controller]")]
    [ApiController]
    public class VariationController : ControllerBase
    {
        private readonly IVariationService _variationService;

        private bool IsTriggerMapVariationRunning = false;

        public VariationController(IVariationService variationService)
        {
            _variationService = variationService;
        }

        [HttpGet]
        public async Task<List<VariationDto>> GetAllVariation()
        {
            return await _variationService.GetAll();
        }

        /// <summary>
        /// Trigger Map Variation 
        /// </summary>
        /// <returns></returns>
        [HttpPost("TriggerMapVariation")]
        public async Task<IActionResult> TriggerMapVariation()
        {
            if (IsTriggerMapVariationRunning)
            {
                return Ok("Process is running");
            }
            else
            {
                IsTriggerMapVariationRunning = true;
                await _variationService.UploadVariationFromPIM();
                IsTriggerMapVariationRunning = false;
                return Ok("Process run finish");
            }
        }
    }
}
