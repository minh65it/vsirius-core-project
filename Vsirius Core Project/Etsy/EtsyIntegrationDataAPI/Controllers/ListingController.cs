using EtsyIntegrationDataAPI._Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EtsyIntegrationDataAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListingController : ControllerBase
    {
        private readonly IListingService _listingService;
        private static bool isCreateListDraftAndPublishListing = false;

        public ListingController(IListingService listingService)
        {
            _listingService = listingService;
        }

        /// <summary>
        /// Upload danh sách sản phẩm lên Esty và published sản phẩm đó 
        /// </summary>
        /// <remarks>
        /// Upload các sản phẩm chưa có trên Esty, 1 lần upload là 2000 sản phẩm
        /// </remarks>
        /// <returns></returns>
        [HttpPost("CreateListDraftAndPublishListing")]
        public async Task<IActionResult> CreateListDraftAndPublishListing()
        {
            if (isCreateListDraftAndPublishListing)
            {
                return BadRequest("CreateListDraftAndPublishListing is running");
            }
            else
            {
                isCreateListDraftAndPublishListing = true;
                await _listingService.CreateListDraftAndPublishListing(1); // số lượng sp upload lên mỗi ngày
                isCreateListDraftAndPublishListing = false;
                return Ok("CreateListDraftAndPublishListing is done");

            }
        }
        /// <summary>
        /// Endpoint này để cập nhật database theo listing đang có trên Etsy
        /// </summary>
        /// <returns></returns>
        [HttpPost("SyncEtsyAndDatabase")]
        public async Task<IActionResult> SyncEtsyAndDatabase()
        {
            var res = await _listingService.SyncEtsyAndDatabase();
            return Ok(res);
        }
    }
}
