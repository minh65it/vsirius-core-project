using ProductAiApi.Common;
using ProductAiApi.DTOs;

namespace ProductAiApi._Services.Interfaces
{
    public interface IProductAIService
    {
        Task<PaginationUtil<ProductAiDto>> GetAll(PaginationParam pagination, string? search, bool? isReview);
        Task<OperationResult<ProductAiDto?>> CreateProduct(CreateUpdateProductAiDto dto);
        Task<OperationResult<object?>> Delete(string sku);
        Task<ProductAiDto?> GetDetail(string sku);
        Task<OperationResult<object?>> ConfirmProducts(List<string> skus);
        Task<OperationResult<ProductAiDto?>> UpdateFromAI(CreateUpdateProductAiDto dto);
        Task<OperationResult<ProductAiDto?>> UpdateFromUser(ProductAiDto dto);
        Task<PaginationUtil<string>?> GetAllSku(PaginationParam pagination);
        //Task<OperationResult<ProductAiDto?>> Update(CreateUpdateProductAiDto dto);
    }
}
