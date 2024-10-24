using CSRApi._Services.Interfaces;
using CSRApi.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VsiriusApi.Controllers.CSRApi
{
    [ApiExplorerSettings(GroupName = "CSR")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class PageViewController : ControllerBase
    {
        private readonly IPageViewService _service;

        public PageViewController(IPageViewService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<IActionResult> Create(CountViewDto dto)
        {
            await _service.IncreaseView(dto);
            return Ok();
        }
        [HttpPost("IndentityUser")]
        public async Task<IActionResult> IndentityUser(IdentityUserIdDto dto)
        {
            return Ok(await _service.SaveInfoUser(dto));
        }
    }
}
