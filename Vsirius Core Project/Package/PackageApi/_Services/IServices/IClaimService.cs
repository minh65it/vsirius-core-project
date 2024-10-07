using PackageApi.Dtos;
using PackageApi.Dtos.Claim;
using PackageApi.Utilities;

namespace PackageApi._Services.IServices
{
    public interface IClaimService
    {
        Task<PaginationResult<ClaimDTO>> GetClaim(PaginationParam paginationParam, ClaimSearchParam search);
        Task<OperationResult> Submit(ClaimParam model);
        List<KeyValuePair<int, string>> GetDocTypes();
        Task<List<ClaimDTO>> GetAllByRMA(List<string> rma);
        Task<ClaimDTO?> GetDetail(string id);
    }
}
