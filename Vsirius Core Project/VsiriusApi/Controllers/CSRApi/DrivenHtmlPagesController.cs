using CSRApi._Services.Interfaces;
using CSRApi.Dtos;
using CSRApi.Helpers.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VsiriusApi.Controllers.CSRApi
{
    [ApiExplorerSettings(GroupName = "CSR")]
    [Route("api/[controller]")]
    [ApiController]
    public class DrivenHtmlPagesController : ControllerBase
    {
        private IDrivenHtmlPagesService _drivenHtmlPagesService;

        public DrivenHtmlPagesController(IDrivenHtmlPagesService drivenHtmlPagesService)
        {
            _drivenHtmlPagesService = drivenHtmlPagesService;
        }

        [HttpPost("Create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(DrivenHtmlPagesDto dto)
        {
            return Ok(await _drivenHtmlPagesService.Create(dto));
        }
        [HttpPut("Update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(DrivenHtmlPagesDto dto)
        {
            return Ok(await _drivenHtmlPagesService.Update(dto));
        }
        [HttpPut("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(DrivenHtmlPagesDto dto)
        {
            return Ok(await _drivenHtmlPagesService.Delete(dto));
        }
        [HttpGet("GetDetail")]
        public async Task<IActionResult> GetDetail(int id)
        {
            return Ok(await _drivenHtmlPagesService.GetDetail(id));
        }
        [HttpGet("GetListData")]
        public async Task<IActionResult> GetListData()
        {
            return Ok(await _drivenHtmlPagesService.GetListData());
        }
        [HttpGet()]
        public async Task<IActionResult> GetData([FromQuery] PaginationParam pagination, string searchText)
        {
            return Ok(await _drivenHtmlPagesService.GetData(pagination, searchText));
        }
    }
}
