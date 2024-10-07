using OrderProcessDomain.Stuller;

namespace OrderProcessApi._Services.IServices.Stuller
{
    public interface ISubmitOrderStullerService
    {
        Task<bool> SubmitStuller(SubmitOrderParam order);
    }
}
