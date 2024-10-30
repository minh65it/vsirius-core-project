using Microsoft.AspNetCore.Mvc;
using StullerApiClient.Models;
using StullerDataApi.Utilities;
using StullerDataApi.Params;
using StullerDataApi.Services.IServices;

namespace VsiriusApi.Controllers.Stuller
{
    [ApiExplorerSettings(GroupName = "Stuller")]
    [Route("api/vs/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _product;
        private readonly IKeyService _keyService;

        public ProductController(IProductService product, IKeyService keyService)
        {
            _product = product;
            _keyService = keyService;
        }

        /// <summary>
        /// clientId: StullerApi; clientSecret: 90b34498-b58e-4f88-ab18-0c1da8a139d8-StullerApi
        /// </summary>
        /// <remarks>
        /// clientId: StullerApi; clientSecret: 90b34498-b58e-4f88-ab18-0c1da8a139d8-StullerApi
        /// </remarks>
        /// <param name="param"></param>
        /// <param name="page"></param>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Filter([FromQuery] ProductParams param, [FromQuery] PaginationParam page, [FromHeader] string clientId, [FromHeader] string clientSecret)
        {
            if (_keyService.ValidateCredentials(clientId, clientSecret))
            {
                return Ok(await _product.Filter(param, page));
            }
            else
            {
                return Unauthorized();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// clientId: StullerApi; clientSecret: 90b34498-b58e-4f88-ab18-0c1da8a139d8-StullerApi
        /// </remarks>
        /// <param name="SKU"></param>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <returns></returns>
        [HttpGet("SKU")]
        public async Task<IActionResult> DetailProduct(string SKU, [FromHeader] string clientId, [FromHeader] string clientSecret)
        {
            if (_keyService.ValidateCredentials(clientId, clientSecret))
            {
                return Ok(await _product.DetailProduct(SKU));
            }
            else
            {
                return Unauthorized();
            }
            
        }
        [HttpPut]
        public async Task<IActionResult> UpdateProduct(string SKU, Product model, [FromHeader] string clientId, [FromHeader] string clientSecret)
        {
            if (_keyService.ValidateCredentials(clientId, clientSecret))
            {
                return Ok(await _product.UpdateProduct(SKU, model));
            }
            else
            {
                return Unauthorized();
            }
            
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(string SKU, [FromHeader] string clientId, [FromHeader] string clientSecret)
        {
            if (_keyService.ValidateCredentials(clientId, clientSecret))
            {
                return Ok(await _product.DeleteProduct(SKU));
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
