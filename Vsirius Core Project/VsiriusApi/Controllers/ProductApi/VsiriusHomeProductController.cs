using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductApi._Services.Interfaces;
using ProductApi.Dtos;

namespace VsiriusApi.Controllers.ProductApi
{
    [ApiExplorerSettings(GroupName = "Product")]
    [Route("product/api/[controller]")]
    [ApiController]
    public class VsiriusHomeProductController : ControllerBase
    {
        private readonly IVsiriusHomeProductService _vsiriusHomeProductService;

        public VsiriusHomeProductController(IVsiriusHomeProductService vsiriusHomeProductService)
        {
            _vsiriusHomeProductService = vsiriusHomeProductService;
        }


        /// <summary>
        /// Show products off the frontend homepage, Filter by 2 cases popular or latest
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("GetAllHomeProduct")]
        public async Task<IActionResult> GetAllHomeProduct([FromQuery] VsiriusHomeProductParams param)
        {
            var result = await _vsiriusHomeProductService.GetAllHomeProduct(param);
            return Ok(result);
        }

        /// <summary>
        /// Show products off the backoffice homepage
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllHomeProductBackend")]
        public async Task<IActionResult> GetAllHomeProductBackend()
        {
            var result = await _vsiriusHomeProductService.GetAllHomeProductBackend();
            return Ok(result);
        }

        /// <summary>
        /// Get related products with 1 displayed product in detail page
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("GetRelatedProduct")]
        public async Task<IActionResult> GetRelatedProduct([FromQuery] VsiriusHomeProductParams param)
        {
            var result = await _vsiriusHomeProductService.GetRelatedProduct(param);
            return Ok(result);
        }

        /// <summary>
        /// Create or update this product outside the home page
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost()]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUpdateHomeProduct([FromBody] VsiriusHomeProductDto dto)
        {
            var result = await _vsiriusHomeProductService.CreateUpdateHomeProduct(dto);
            return Ok(result);
        }

        /// <summary>
        /// Do not show this product outside the home page
        /// </summary>
        /// <param name="item_sku"></param>
        /// <returns></returns>
        [HttpDelete()]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveHomeProduct([FromQuery] string item_sku)
        {
            var result = await _vsiriusHomeProductService.RemoveHomeProduct(item_sku);
            return Ok(result);
        }
    }
}
