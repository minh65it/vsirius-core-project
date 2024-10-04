using AmazonIntegrationDataApi._Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AmazonIntegrationDataApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapVSiriusDBToAmazonDBController : ControllerBase
    {
        private readonly IMapVSiriusDBToAmazonDB _mapVSiriusDBToAmazonDB;
        private bool IsMapToAmazonDB = false;

        public MapVSiriusDBToAmazonDBController(IMapVSiriusDBToAmazonDB mapVSiriusDBToAmazonDB)
        {
            _mapVSiriusDBToAmazonDB = mapVSiriusDBToAmazonDB;
        }
        /// <summary>
        /// Dùng endpoint này map từ PIM sang data AmazonSeller
        /// </summary>
        /// <returns></returns>
        [HttpPost("MapToAmazonDB")]
        public async Task<IActionResult> MapToAmazonDB()
        {
            if (IsMapToAmazonDB)
            {
                return BadRequest("MapToAmazonDB is running");
            }
            else
            {
                IsMapToAmazonDB = true;
                await _mapVSiriusDBToAmazonDB.MapToAmazonDB();
                IsMapToAmazonDB = false;
                return Ok("MapToAmazonDB is done");

            }
        }
    }
}
