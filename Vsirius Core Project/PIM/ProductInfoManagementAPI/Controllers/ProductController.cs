using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PimApi._Services.Interfaces;
using PimApi.Dtos;
using PimApi.Helpers.Utilities;

namespace PimApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Tạo mới 1 sản phẩm
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("CreateProduct")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDto dto)
        {
            var result = await _productService.CreateProduct(dto);
            return Ok(result);
        }

        /// <summary>
        /// Import sản phẩm vào data
        /// </summary>
        /// <param name="fileExcel"></param>
        /// <returns></returns>
        [DisableRequestSizeLimit]
        [HttpPost("ImportExcel")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> ImportFile(IFormFile fileExcel)
        {
            var result = await _productService.ImportFile(fileExcel);
            return Ok(result);
        }

        [HttpGet("SKU")]
        public async Task<IActionResult> GetProduct([FromQuery] string item_sku)
        {
            var result = await _productService.GetProduct(item_sku);
            return Ok(result);
        }
        [HttpDelete()]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveProduct([FromQuery] string item_sku)
        {
            var result = await _productService.RemoveProduct(item_sku);
            return Ok(result);
        }
        [HttpPut()]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductDto dto)
        {
            var result = await _productService.UpdateProduct(dto);
            return Ok(result);
        }

        /// <summary>
        /// FilterParam is based on /api/VsiriusProduct/Filter, if filterParam is empty send null array
        /// </summary>
        /// <remarks>
        /// Search cho frontend. láy ra 1 số trường cần thiết
        /// </remarks>
        /// <param name="filterParam"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        [HttpPost()]
        public async Task<IActionResult> GetAll([FromBody] GetAllParmas filterParam, [FromQuery] PaginationParam pagination)
        {
            var result = await _productService.GetAll(filterParam, pagination);
            return Ok(result);
        }

        /// <summary>
        /// Get the list of sku items that are in the database. endpoint for app console mapper
        /// </summary>
        /// <remarks>
        /// tìm kiếm item_sku_category với 'S' hoặc 'Q'
        /// </remarks>
        /// <param name="item_sku_category"></param>
        /// <returns></returns>
        [HttpGet("GetAllItemSKU")]
        public async Task<IActionResult> GetAllItemSKU(string item_sku_category)
        {
            var result = await _productService.GetAllItemSKU(item_sku_category);
            return Ok(result);
        }
        /// <summary>
        /// Xoá nhiều sản phẩm
        /// </summary>
        /// <param name="listSKU"></param>
        /// <returns></returns>
        [HttpPut("RemoveMultipleProduct")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveMultipleProduct([FromBody] List<string> listSKU)
        {
            var result = await _productService.RemoveMultipleProduct(listSKU);
            return Ok(result);
        }

        /// <summary>
        /// Thay cho endpoint getallbackoffice. Lấy toàn bộ data
        /// </summary>
        /// <remarks>
        /// VSirius 0,
        /// PhillyLux 1,
        /// Amazon 2,
        /// Walmart 3,
        /// Ebay 4,
        /// Etsy 5
        /// </remarks>
        /// <param name="pagination"></param>
        /// <param name="searchParams"></param>
        /// <returns></returns>
        [HttpPost("GetData")]
        public async Task<IActionResult> GetData([FromQuery] PaginationParam pagination, [FromBody] SearchParams searchParams)
        {
            return Ok(await _productService.GetData(pagination, searchParams, true));
        }

        /// <summary>
        /// Lấy ra các sản phẩm nào đang có trên từng martketplace
        /// </summary>
        /// <remarks>
        /// VSirius 0,
        /// PhillyLux 1,
        /// Amazon 2,
        /// Walmart 3,
        /// Ebay 4,
        /// Etsy 5
        /// </remarks>
        /// <param name="martketplace"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        [HttpGet("GetAllItemMartketplace")]
        public async Task<IActionResult> GetAllItemMartketplace([FromQuery] Marketplace martketplace, [FromQuery] PaginationParam pagination)
        {
            return Ok(await _productService.GetAllItemMartketplace(martketplace, pagination));
        }

        /// <summary>
        /// update list SKU này có feild IsVsirius = true, IsPhillyLux = true....
        /// </summary>
        /// <param name="martketplace"></param>
        /// <param name="listSKU"></param>
        /// <returns></returns>
        [HttpPut("UpdateMartketplace")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateMartketplace([FromQuery] Marketplace martketplace, [FromBody] List<string> listSKU, [FromQuery] bool isValue)
        {
            var result = await _productService.UpdateMartketplace(martketplace, listSKU, isValue);
            return Ok(result);
        }

        /// <summary>
        /// data for Walmart
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet("GetData")]
        public async Task<IActionResult> GetData([FromQuery] PaginationParam pagination, [FromQuery] string? keyword)
        {
            return Ok(await _productService.GetData(pagination, keyword, true));
        }
    }
}
