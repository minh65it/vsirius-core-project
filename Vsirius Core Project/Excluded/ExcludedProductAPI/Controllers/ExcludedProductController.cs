using ExcludedProductAPI._Services.Interfaces;
using ExcludedProductAPI.DTOs;
using ExcludedProductAPI.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace ExcludedProductAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize(Roles = "Admin")]
    public class ExcludedProductController : ControllerBase
    {
        private readonly IExcludedProductService _excludedProductService;
        public ExcludedProductController(
            IExcludedProductService excludedProductService)
        {
            _excludedProductService = excludedProductService;
        }
        [HttpGet("GetAll")]
        public async Task<PaginationUtil<ExcludedProductDTO>> GetAll(
            [FromQuery] PaginationParam pagination,
            [FromQuery] ExcludedProductParam search)
        {
            return await _excludedProductService.GetAll(pagination, search);
        }

        [HttpGet("GetDetail")]
        public async Task<ExcludedProductDTO?> GetDetail(string MarketplaceSKU)
        {
            return await _excludedProductService.GetDetail(MarketplaceSKU);
        }

        [HttpDelete]
        public async Task<OperationResult<object?>> Delete(string MarketplaceSKU)
        {
            return await _excludedProductService.Delete(MarketplaceSKU);
        }

        [HttpPost]
        public async Task<OperationResult<ExcludedProductDTO?>> Create(ExcludedProductDTO dto)
        {
            return await _excludedProductService.Create(dto);
        }

        [HttpPut]
        public async Task<OperationResult<ExcludedProductDTO?>> Update(ExcludedProductDTO dto)
        {
            return await _excludedProductService.Update(dto);
        }

    }
}
