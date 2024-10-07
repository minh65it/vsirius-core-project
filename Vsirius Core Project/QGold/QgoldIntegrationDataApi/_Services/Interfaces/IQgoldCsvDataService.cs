using QgoldIntegrationDataApi.Dtos.Qgold;
using QgoldIntegrationDataApi.Helpers.Params;
using QgoldIntegrationDataApi.Helpers.Ultilities;

namespace QgoldIntegrationDataApi._Services.Interfaces
{
    public interface IQgoldCsvDataService
    {
        Task<PaginationUtility<QgoldCsvData_DTO>> GetData(PaginationParam pagination,QgoldCsvDataFillter dataFillter,bool isPaging = true);
        Task<OperationResult> Create(QgoldCsvData_DTO dto);
        Task<OperationResult> Update(QgoldCsvData_DTO dto);
        Task<OperationResult> Delete(QgoldCsvData_DTO dto);
        Task<List<KeyValuePair<string,string>>> GetBookList();
        Task<OperationResult> GenDataByExcel(IFormFile fileExcel);
        Task<List<string>> GetDataQgoldItem();
        Task<OperationResult> CreateAndUpdateDataQgold(List<QgoldData> dto);
        Task<OperationResult> DeleteDataQgold(List<string> dto);
    }
}
