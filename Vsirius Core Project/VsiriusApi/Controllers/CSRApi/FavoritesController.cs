using CSRApi._Services.Interfaces;
using CSRApi.Dtos;
using CSRApi.Helpers.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace VsiriusApi.Controllers.CSRApi
{
    [ApiExplorerSettings(GroupName = "CSR")]
    [Route("api/[controller]")]
    [ApiController]
    public class FavoritesController : ControllerBase
    {
        private readonly IFavouritesService _service;

        public FavoritesController(IFavouritesService service)
        {
            _service = service;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(FavouritesDto dto)
        {
            return Ok(await _service.Create(dto));
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination,string userId)
        {
            return Ok(await _service.GetData(pagination,userId));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(string userId)
        {
            return Ok(await _service.GetAll(userId));
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            return Ok(await _service.Delete(id));
        }
    }
}
