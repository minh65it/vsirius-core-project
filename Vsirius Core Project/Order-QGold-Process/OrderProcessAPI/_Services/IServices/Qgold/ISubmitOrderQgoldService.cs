using OrderProcessDomain.AmazonSeller;

namespace OrderProcessApi._Services.IServices.Qgold
{
    public interface ISubmitOrderQgoldService
    {
        Task<bool> CreateFile(string textContent, string fileName);
        Task<bool> SubmitQgold(List<QgoldFtpOrderObject> orders, bool? isResubmit);
    }
}
