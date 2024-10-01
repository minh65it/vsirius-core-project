using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductApi._Services.Interfaces;
using ProductApi.Dtos;
using ProductApi.Helpers.Utilities;
using System.Data;

namespace ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VsiriusProductController : ControllerBase
    {
        private readonly IVsiriusProductService _vsiriusProductService;

        public VsiriusProductController(IVsiriusProductService vsiriusProductService)
        {
            _vsiriusProductService = vsiriusProductService;
        }

        [HttpPost("CreateProduct")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProduct([FromBody] VsiriusProductDto dto)
        {
            var result = await _vsiriusProductService.CreateProduct(dto);
            return Ok(result);
        }

        [DisableRequestSizeLimit]
        [HttpPost("ImportExcel")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ImportFile(IFormFile fileExcel)
        {
            var result = await _vsiriusProductService.ImportFile(fileExcel);
            return Ok(result);
        }
        [HttpGet("SKU")]
        public async Task<IActionResult> GetProduct([FromQuery] string item_sku)
        {
            var result = await _vsiriusProductService.GetProduct(item_sku);
            return Ok(result);
        }
        [HttpDelete()]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveProduct([FromQuery] string item_sku)
        {
            var result = await _vsiriusProductService.RemoveProduct(item_sku);
            return Ok(result);
        }
        [HttpPut()]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct([FromBody] VsiriusProductDto dto)
        {
            var result = await _vsiriusProductService.UpdateProduct(dto);
            return Ok(result);
        }

        /// <summary>
        /// Get the filter for products on the frontend page, if it is null, it will get a list of feed_product_type, when there is a feed_product_type param, it will filter by feed_product_type
        /// </summary>
        /// <param name="feed_product_type"></param>
        /// <returns></returns>
        [HttpGet("Filter")]
        public async Task<IActionResult> GetFilter([FromQuery] string? feed_product_type)
        {
            var result = await _vsiriusProductService.GetFilter(feed_product_type);
            return Ok(result);
        }

        /// <summary>
        /// Get the required fields in the excel file to create constraints when adding new products. Used for backoffice
        /// </summary>
        /// <param name="feed_product_type"></param>
        /// <returns></returns>
        [HttpGet("GetValueForAdd")]
        public async Task<IActionResult> GetValueForAdd([FromQuery] string? feed_product_type)
        {
            var result = await _vsiriusProductService.GetValueForAdd(feed_product_type);
            return Ok(result);
        }

        /// <summary>
        /// FilterParam is based on /api/VsiriusProduct/Filter, if filterParam is empty send null array
        /// </summary>
        /// <param name="filterParam"></param>
        /// <param name="pagination"></param>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        [HttpPost()]
        public async Task<IActionResult> GetAll([FromBody] List<VsiriusFilterParamDto> filterParam, [FromQuery] PaginationParam pagination, [FromQuery] string? keyWord)
        {
            var result = await _vsiriusProductService.GetAll(filterParam, pagination, keyWord);
            return Ok(result);
        }

        /// <summary>
        /// Find products by product type, based on layer1. eg watches, rings, apparel..., page size 20
        /// </summary>
        /// <param name="layer1"></param>
        /// <returns></returns>
        [HttpGet("Layer1")]
        public async Task<IActionResult> Layer1([FromQuery] string layer1)
        {
            var result = await _vsiriusProductService.GetAllLayer1(layer1);
            return Ok(result);
        }

        /// <summary>
        /// Get the list of sku items that are in the database. endpoint for app console mapper
        /// </summary>
        /// <param name="item_sku_category"></param>
        /// <returns></returns>
        [HttpGet("GetAllItemSKU")]
        public async Task<IActionResult> GetAllItemSKU(string item_sku_category)
        {
            var result = await _vsiriusProductService.GetAllItemSKU(item_sku_category);
            return Ok(result);
        }
        [HttpPut("RemoveMultipleProduct")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveMultipleProduct([FromBody] List<string> listSKU)
        {
            var result = await _vsiriusProductService.RemoveMultipleProduct(listSKU);
            return Ok(result);
        }
    }
}
