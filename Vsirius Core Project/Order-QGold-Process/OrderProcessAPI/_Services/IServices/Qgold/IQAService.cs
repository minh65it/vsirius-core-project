using OrderProcessApi.DTOs;
using OrderProcessApi.Utilities.Mapping;
using OrderProcessDomain.Qgold;

namespace OrderProcessApi._Services.IServices.Qgold
{
    public interface IQAService
    {
        Task<OperationResult> SendMailQuestion(QgoldQADTO question);
        Task<PaginationResult<QgoldQADTO>> GetQuestion(PaginationParam paginationParam, SearchQgoldQAParam seachParam);
        Task<QgoldQADTO> DetailQuestion(string id);
    }
}
