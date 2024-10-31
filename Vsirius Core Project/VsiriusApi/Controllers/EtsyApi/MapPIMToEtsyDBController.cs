using EtsyIntegrationDataAPI._Services.Interfaces;
using EtsyIntegrationDataAPI.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace VsiriusApi.Controllers.EtsyApi
{
    [ApiExplorerSettings(GroupName = "Etsy")]
    [Route("api/[controller]")]
    [ApiController]
    public class MapPIMToEtsyDBController : ControllerBase
    {
        private readonly IMapPIMToEtsyDBService _mapPIMToEtsyDBService;
        private static bool IsMapToEtsyDB = false;
        public MapPIMToEtsyDBController(IMapPIMToEtsyDBService mapPIMToEtsyDBService)
        {
            _mapPIMToEtsyDBService = mapPIMToEtsyDBService;
        }

        /// <summary>
        /// Map các sản phẩm từ PIM sang Etsy
        /// </summary>
        /// <remarks>
        /// Map Product <br/>
        /// Map Images <br/>
        /// Map Material <br/>
        /// Map Tag <br/>
        /// Map Style <br/>
        /// Map Properties <br/>
        /// </remarks>
        /// <returns></returns>
        [HttpPost()]
        [ProducesResponseType(typeof(OperationResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RequestAnAuthorizationCode()
        {
            if (IsMapToEtsyDB)
            {
                return BadRequest("MapToEtsyDB is running");
            }
            else
            {
                IsMapToEtsyDB = true;
                await _mapPIMToEtsyDBService.MapToEtsyDB();
                IsMapToEtsyDB = false;
                return Ok("MapToEtsyDB is done");

            }
        }
    }
}
