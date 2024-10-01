
using ExcludedProductAPI.DTOs;
using ExcludedProductAPI.Helpers;

namespace ExcludedProductAPI._Services.Interfaces
{
    public interface IExcludedProductService
    {
        Task<PaginationUtil<ExcludedProductDTO>> GetAll(PaginationParam pagination, ExcludedProductParam search);
        Task<OperationResult<object?>> Delete(string id);
        Task<ExcludedProductDTO?> GetDetail(string id);
        Task<OperationResult<ExcludedProductDTO?>> Update(ExcludedProductDTO dto);
        Task<OperationResult<ExcludedProductDTO?>> Create(ExcludedProductDTO dto);

    }
}
