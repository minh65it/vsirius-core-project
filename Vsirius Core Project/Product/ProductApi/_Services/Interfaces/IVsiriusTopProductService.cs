using ProductApi.Dtos;
using ProductApi.Helpers.Utilities;

namespace ProductApi._Services.Interfaces
{
    public interface IVsiriusTopProductService
    {
        Task<PaginationUtility<VsiriusProductDto>> GetTopProduct(PaginationParam pagination, VsiriusTopParams param);
    }
}
