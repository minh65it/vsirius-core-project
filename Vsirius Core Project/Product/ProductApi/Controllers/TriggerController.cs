using Microsoft.AspNetCore.Mvc;
using ProductApi._Services.Interfaces;
using ProductApi.Dtos;

namespace ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TriggerController : ControllerBase
    {
        private readonly IMapVSiriusDBToVSiriusWebDB _mapVSiriusDBToVSiriusWebDB;

        public TriggerController(IMapVSiriusDBToVSiriusWebDB mapVSiriusDBToVSiriusWebDB)
        {
            _mapVSiriusDBToVSiriusWebDB = mapVSiriusDBToVSiriusWebDB;
        }

        private bool IsMapToVSiriusDB = false;
        private bool IsUpdateFilter = false;

        /// <summary>
        /// Map data từ ProductInformationManager sang VsiriusWeb
        /// </summary>
        /// <returns></returns>
        [HttpPost("MapToVSiriusWebDB")]
        public async Task<IActionResult> MapToAmazonDB()
        {
            if (IsMapToVSiriusDB)
            {
                return BadRequest("MapToVSiriusWebDB is running");
            }
            else
            {
                IsMapToVSiriusDB = true;
                await _mapVSiriusDBToVSiriusWebDB.MapToVSiriusDB();
                IsMapToVSiriusDB = false;
                return Ok("MapToVSiriusWebDB is done");

            }
        }

        /// <summary>
        /// Update lại filter
        /// </summary>
        /// <returns></returns>
        [HttpPost("UpdateFilter")]
        public async Task<IActionResult> UpdateFilter()
        {
            if (IsUpdateFilter)
            {
                return BadRequest("UpdateFilter is running");
            }
            else
            {
                IsUpdateFilter = true;
                await _mapVSiriusDBToVSiriusWebDB.UpdateFilter();
                IsUpdateFilter = false;
                return Ok("UpdateFilter is done");

            }
        }
    }
}
