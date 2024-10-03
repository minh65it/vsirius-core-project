using PhillyLuxEcommerce.Dtos.Params;
using PhillyLuxEcommerce.Helpers.Utilities;

namespace PhillyLuxEcommerce.Services.Interfaces
{
    public interface IHomeService
    {
        Task<OperationResult> GetFilterByProductType();
        Task<OperationResult> GetFilterByProductTypeMoq();
        Task<OperationResult> GetAllHomeProduct(string filter, int takeProduct);
        Task<OperationResult> GetProductMoq(string filter, int takeProduct);
        Task<OperationResult> LoginResult(LoginParam loginDto);
        Task<OperationResult> RegisterResult(RegisterParam registerParam);
        Task<OperationResult> GetAllPages();
    }
}
