using EtsyIntegrationDataAPI._Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EtsyIntegrationDataAPI.Controllers
{
    [Route("api/oldetsy/[controller]")]
    [ApiController]
    public class ListingImageController : ControllerBase
    {
        private readonly IListingImageService _listingImageService;

        public ListingImageController(IListingImageService listingImageService)
        {
            _listingImageService = listingImageService;
        }

        [HttpPost("UploadListingImage")]
        public async Task<IActionResult> UploadListingImage([FromQuery] int listing_id)
        {
            var result = await _listingImageService.UploadListingImage("Bearer refresh_token", listing_id);
            return Ok(result);
        }
    }
}
