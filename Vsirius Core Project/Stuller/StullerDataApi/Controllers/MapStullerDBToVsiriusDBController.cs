using Microsoft.AspNetCore.Mvc;
using StullerDataApi.Services.IServices;

namespace StullerDataApi.Controllers
{
    [Route("api/stuller/[controller]")]
    [ApiController]
    public class MapStullerDBToVsiriusDBController : ControllerBase
    {
        private readonly IMapStullerDBToVsiriusDB _mapStullerDBToVsiriusDB;
        private static bool IsRunning = false;

        public MapStullerDBToVsiriusDBController(IMapStullerDBToVsiriusDB mapStullerDBToVsiriusDB)
        {
            _mapStullerDBToVsiriusDB = mapStullerDBToVsiriusDB;
        }

        [HttpPost]
        public async Task<IActionResult> MapToVsiriusDB()
        {
            if (IsRunning)
            {
                return BadRequest("MapToVsiriusDB is running");
            }
            else
            {
                IsRunning = true;
                await _mapStullerDBToVsiriusDB.MapToVsiriusDB();
                IsRunning = false;
                return Ok("MapToVsiriusDB is done");

            }
        }
    }
}
