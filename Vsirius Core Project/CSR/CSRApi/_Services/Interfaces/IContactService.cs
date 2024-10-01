using CSRApi.Dtos;
using CSRApi.Helpers.Utilities;

namespace CSRApi._Services.Interfaces
{
    public interface IContactService
    {
        Task<OperationResult> Create(ContactDto dto);
        Task<OperationResult> Update(ContactDto dto);
        Task<OperationResult> Delete(string id);
        Task<PaginationUtility<ContactDto>> GetData(PaginationParam pagination, string keyword);

        Task<ContactDto> GetDetail(string id);
    }
}
