using CSRApi.Dtos;
using CSRApi.Helpers.Utilities;

namespace CSRApi._Services.Interfaces
{
    public interface IDrivenHtmlPagesService
    {
        Task<OperationResult> Create(DrivenHtmlPagesDto dto);
        Task<OperationResult> Update(DrivenHtmlPagesDto dto);
        Task<DrivenHtmlPagesDto> GetDetail(int id);
        Task<OperationResult> Delete(DrivenHtmlPagesDto dto);
        Task<PaginationUtility<DrivenHtmlPagesDto>> GetData(PaginationParam pagination, string searchText);

        Task<List<DrivenHtmlPagesDto>> GetListData();
    }
}
