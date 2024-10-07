using OrderProcessApi.DTOs;
using OrderProcessApi.Utilities.Mapping;
using OrderProcessDomain.AmazonSeller;
using OrderProcessDomain.Qgold;

namespace OrderProcessApi._Services.IServices.Qgold
{
    public interface IPackageClaimService
    {
        Task<OperationResult> SendMailLostPackage (QgoldOrderDetailDTO order);
    }
}
