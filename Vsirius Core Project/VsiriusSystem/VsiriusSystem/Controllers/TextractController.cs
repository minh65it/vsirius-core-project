using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VsiriusSystem._Services.Interfaces;

namespace VsiriusSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TextractController : ControllerBase
    {
        private readonly ITextractAwsService _amazonService;
        private readonly IKeyService _keyService;
        public TextractController(ITextractAwsService amazonService, IKeyService keyService)
        {
            _amazonService = amazonService;
            _keyService = keyService;
        }
        /// <summary>
        /// read text from img
        /// </summary>
        /// <remarks>
        /// "TextExtract": "622bfb44-25a1-463d-95d0-3db565adcfb9-TextExtract"
        /// </remarks>
        /// <param name="file"></param>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ScanForText-Amazon1")]
        public async Task<IActionResult> ScanForText(
            IFormFile file,
            [FromHeader] string clientId, 
            [FromHeader] string clientSecret)
        {
            if (_keyService.ValidateCredentials(clientId, clientSecret))
            {
                string text = await _amazonService.AmazoneScanForText(file);
                return Ok(text);
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
