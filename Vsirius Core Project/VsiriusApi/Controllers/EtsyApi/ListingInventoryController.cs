using EtsyIntegrationDataAPI._Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace VsiriusApi.Controllers.EtsyApi
{
    [ApiExplorerSettings(GroupName = "Etsy")]
    [Route("api/[controller]")]
    [ApiController]
    public class ListingInventoryController : ControllerBase
    {
        private readonly IListingInventoryService _listingInventoryService;

        public ListingInventoryController(IListingInventoryService listingInventoryService)
        {
            _listingInventoryService = listingInventoryService;
        }

        /// <summary>
        /// Update inventory and price all products in Esty
        /// </summary>
        /// <returns></returns>
        [HttpPut("UpdateAllListingInventory")]
        public async Task<IActionResult> UpdateAllListingInventory()
        {
            var result = await _listingInventoryService.UpdateAllListingInventory();
            return Ok(result);
        }
    }
}
