
using Microsoft.AspNetCore.Mvc;
using PimApi._Services.Interfaces;

namespace VsiriusApi.Controllers.PimApi
{
    [ApiExplorerSettings(GroupName = "Pim")]
    [Route("api/[controller]")]
    [ApiController]
    public class ETLOrderController : ControllerBase
    {
        private readonly IETLOrderService _service;

        public ETLOrderController(IETLOrderService service)
        {
            _service = service;
        }

        [HttpPost()]
        public async Task<IActionResult> ConvertOrder()
        {
            return Ok(await _service.ConvertOrder());
        }
    }
}
