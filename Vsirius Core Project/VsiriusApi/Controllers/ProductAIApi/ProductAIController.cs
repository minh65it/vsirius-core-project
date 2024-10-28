using Microsoft.AspNetCore.Mvc;
using ProductAiApi._Services.Interfaces;
using ProductAiApi.Common;
using ProductAiApi.DTOs;

namespace VsiriusAPI.Controllers.ProductAIApi
{
    [ApiExplorerSettings(GroupName = "ProductAI")]
    [ApiController]
    [Route("[controller]")]
    public class ProductAIController : ControllerBase
    {
        private readonly IProductAIService _productAiService;

        public ProductAIController(
            IProductAIService productAiService)
        {
            _productAiService = productAiService;
        }
        /// <summary>
        /// Get all Product create by AI
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="searchKey"></param>
        /// <param name="isReview"></param>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<PaginationUtil<ProductAiDto>> GetAll(
            [FromQuery] PaginationParam pagination,
            [FromQuery] string? searchKey,
            [FromQuery] bool? isReview
            )
        {
            return await _productAiService.GetAll(pagination, searchKey, isReview);
        }

        /// <summary>
        /// Get List Sku create by AI
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllSku")]
        public async Task<PaginationUtil<string>?> GetAllSku([FromQuery] PaginationParam pagination)
        {
            return await _productAiService.GetAllSku(pagination);
        }
        /// <summary>
        /// Detail Product
        /// </summary>
        /// <param name="sku"></param>
        /// <returns></returns>
        [HttpGet("GetDetail")]
        public async Task<ProductAiDto?> GetDetail(string sku)
        {
            return await _productAiService.GetDetail(sku);
        }
        /// <summary>
        /// Delete product
        /// </summary>
        /// <param name="sku"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<OperationResult<object?>> Delete(string sku)
        {
            return await _productAiService.Delete(sku);
        }
        /// <summary>
        /// Create product by AI
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<OperationResult<ProductAiDto?>> Create(CreateUpdateProductAiDto dto)
        {
            return await _productAiService.CreateProduct(dto);
        }

        /// <summary>
        /// confirm review multiple ai product
        /// </summary>
        /// <param name="skus"></param>
        /// <returns></returns>
        [HttpPost("ConfirmProducts")]
        public async Task<OperationResult<object?>> ConfirmProducts([FromBody] List<string> skus)
        {
            return await _productAiService.ConfirmProducts(skus);
        }

        [HttpPut("UpdateFromAI")]
        public async Task<OperationResult<ProductAiDto?>> UpdateFromAI(CreateUpdateProductAiDto dto)
        {
            return await _productAiService.UpdateFromAI(dto);
        }

        [HttpPut("UpdateFromUser")]
        public async Task<OperationResult<ProductAiDto?>> UpdateFromUser(ProductAiDto dto)
        {
            return await _productAiService.UpdateFromUser(dto);
        }

    }
}
