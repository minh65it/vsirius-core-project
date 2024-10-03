using PhillyLuxEcommerce.Dtos;
using PhillyLuxEcommerce.Helpers.Utilities;
using PhillyLuxEcommerce.ViewModels;

namespace PhillyLuxEcommerce.Services.Interfaces
{
    public interface IPageService
    {
        Task<OperationResult> AddContact(ContactVM contactVM);
        Task<OperationResult> Page(int code);
        Task<List<PageInfoDto>>? GetListPage();
    }
}
