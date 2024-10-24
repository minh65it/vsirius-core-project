using Microsoft.AspNetCore.Mvc;
using ProductApi._Services.Interfaces;
using ProductApi.Dtos;
using ProductApi.Helpers.Utilities;

namespace VsiriusApi.Controllers.ProductApi
{
    [ApiExplorerSettings(GroupName = "Product")]
    [Route("api/[controller]")]
    [ApiController]
    public class VsiriusTopProductController : ControllerBase
    {
        private readonly IVsiriusTopProductService _vsiriusShopService;

        public VsiriusTopProductController(IVsiriusTopProductService vsiriusShopService)
        {
            _vsiriusShopService = vsiriusShopService;
        }

        /// <summary>
        /// Get all products with pagination in Shop
        /// </summary>
        /// <remarks>
        /// Retrieve products for the shop page, with pagination. param.Filter: popular and latest // Filter nếu khác 2 giá trị trên sẽ trả về hết 
        /// param.feed_product_type là loại sản phẩm
        /// Nhiều feed_product_type sẽ ngăn cách nhau bởi dấu |
        /// </remarks>
        /// <param name="pagination"></param>
        /// <param name="filterCategory"></param>
        /// <returns></returns>
        [HttpGet()]
        public async Task<IActionResult> GetShopProduct([FromQuery] PaginationParam pagination, [FromQuery] VsiriusTopParams param)
        {
            var result = await _vsiriusShopService.GetTopProduct(pagination, param);
            return Ok(result);
        }
    }
}
