using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductAiApi._JobServices.Interfaces;

namespace ProductAiApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TriggerController : ControllerBase
    {
        private readonly ICreateProductAiFromPimService _pimMapService;
        private static bool IsCreateProductAiFromPimUsed = false;

        public TriggerController(ICreateProductAiFromPimService pimMapService)
        {
            _pimMapService = pimMapService;
        }

        [HttpPost("CreateProductAiFromPim")]
        public async Task<IActionResult> CreateProductAiFromPim()
        {
            if (IsCreateProductAiFromPimUsed)
            {
                return BadRequest("Process is running.");
            }
            else
            {
                IsCreateProductAiFromPimUsed = true;
                await _pimMapService.CreateProductAIFromPim();
                IsCreateProductAiFromPimUsed = false;
                return Ok("Trigger success.");
            }
        }
    }
}
