using ProductApi.Dtos;
using ProductApi.Helpers.Utilities;

namespace ProductApi._Services.Interfaces
{
    public interface IVsiriusFilterService
    {
        Task<PaginationUtility<VsiriusFilterDto>> GetListFilter(PaginationParam pagination, string? fullTextSearch, bool isPaging = true);
        Task<VsiriusFilterDto> GetFilter(int filterId);
        Task<OperationResult> GetFilterByProductType();
        Task<OperationResult> AddFilterByExcel(IFormFile fileExcel);
        Task<OperationResult> AddFilter(VsiriusFilterDto vsiriusFilterDto);
        Task<OperationResult> UpdateFilter(VsiriusFilterDto vsiriusFilterDto);
        Task<OperationResult> DeleteFilter(int filterId);

    }
}
