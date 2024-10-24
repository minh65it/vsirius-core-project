using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductApi._Services.Interfaces;
using ProductApi.Dtos;
using ProductApi.Helpers.Utilities;

namespace VsiriusApi.Controllers.ProductApi
{
    [ApiExplorerSettings(GroupName = "Product")]
    [Route("api/[controller]")]
    [ApiController]
    public class VsiriusFilterController : ControllerBase
    {
        private IVsiriusFilterService _filterService;

        public VsiriusFilterController(IVsiriusFilterService filterService)
        {
            _filterService = filterService;
        }

        [HttpGet("GetListFilter")]
        public async Task<IActionResult> GetListFilter([FromQuery] PaginationParam pagination, string? fullTextSearch)
        {
            var res = await _filterService.GetListFilter(pagination, fullTextSearch);
            return Ok(res);
        }

        [HttpGet("GetFilter")]
        public async Task<IActionResult> GetFilter(int filterId)
        {
            var res = await _filterService.GetFilter(filterId);
            return Ok(res);
        }

        [HttpGet("GetFilterByProductType")]
        public async Task<IActionResult> GetFilterByProductType()
        {
            var res = await _filterService.GetFilterByProductType();
            return Ok(res);
        }

        [HttpPost("AddFilterByExcel")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddFilterByExcel(IFormFile fileExcel)
        {
            var res = await _filterService.AddFilterByExcel(fileExcel);
            return Ok(res);
        }

        [HttpPost("AddFilter")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddFilter([FromForm] VsiriusFilterDto dto)
        {
            var res = await _filterService.AddFilter(dto);
            return Ok(res);
        }

        [HttpPut("UpdateFilter")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateFilter([FromForm] VsiriusFilterDto dto)
        {
            var res = await _filterService.UpdateFilter(dto);
            return Ok(res);
        }

        [HttpDelete("DeleteFilter")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteFilter(int filterId)
        {
            var res = await _filterService.DeleteFilter(filterId);
            return Ok(res);
        }


    }
}
