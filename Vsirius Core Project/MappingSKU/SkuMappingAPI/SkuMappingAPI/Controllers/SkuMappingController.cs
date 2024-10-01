using Microsoft.AspNetCore.Mvc;
using SkuMappingAPI._Repositories.Interfaces;
using SkuMappingAPI.Common;
using SkuMappingAPI.DTOs;
using SkuMappingAPI.Services.Interfaces;

namespace SkuMappingAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SkuMappingController : ControllerBase
    {
        private readonly IMappingSkuService _service;
        private Result _result;

        public SkuMappingController(IMappingSkuService service, Result result)
        {
            _service = service;
            _result = result;
        }

        [HttpGet("GetSkuWalmartFromQgold")]
        public async Task<IActionResult> GetSkuWalmartFromQgold([FromQuery] PaginationParam pagination)
        {
            return Ok(await _service.GetSkuWalmartFromQgold(pagination));
        }


        [HttpPost("BulkSkuWalmartQgold")]
        public async Task<IActionResult> BulkSkuWalmartQgold([FromBody] List<QgoldToWalmartDTO> dto)
        {
            _result = await _service.BulkSkuWalmartQgold(dto);
            return Ok(_result);
        }
    }
}
