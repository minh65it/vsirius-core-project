using EtsyIntegrationDataAPI._Services.Interfaces;
using EtsyIntegrationDataAPI.Dtos.ProductEtsyDto;
using EtsyIntegrationDataAPI.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace EtsyIntegrationDataAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductEtsyController : ControllerBase
    {
        private readonly IProductEtsyService _productEtsyService;

        public ProductEtsyController(IProductEtsyService productEtsyService)
        {
            _productEtsyService = productEtsyService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetData([FromQuery] PaginationParam pagination, [FromQuery] string? keyword, [FromQuery] bool isPaging)
        {
            var res = await _productEtsyService.GetData(pagination, keyword, isPaging);
            return Ok(res);
        }
        [HttpGet("Detail")]
        public async Task<IActionResult> GetDataDetail([FromQuery] string item_sku, [FromQuery] string? keyword)
        {
            var res = await _productEtsyService.GetDataDetail(item_sku, keyword);
            return Ok(res);
        }

        [HttpPost("UpdateListingIdProduct")]
        public async Task<IActionResult> UpdateListingIdProduct([FromBody] ProductEtsyDto dto)
        {
            var res = await _productEtsyService.UpdateListingIdProduct(dto);
            return Ok(res);
        }
    }
}
