using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PimApi._Services.Interfaces;

namespace PimApi.Controllers
{
    [Route("api/pim/[controller]")]
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
