using SkuMappingAPI.Common;
using SkuMappingAPI.DTOs;

namespace SkuMappingAPI.Services.Interfaces
{
    public interface IMappingSkuService
    {
        Task<Result> BulkSkuWalmartQgold(List<QgoldToWalmartDTO> dto);
        Task<PaginationUtility<QgoldToWalmartDTO>> GetSkuWalmartFromQgold(PaginationParam pagination);
    }
}
