using OrdersAPI.Dtos;

namespace OrdersAPI._Services.Interfaces
{
    public interface ISendMailService
    {
        Task<Result> SendToMultipleEmail(string from, List<string> to, string subject, string body);
        Task<Result> SendToMultipleEmailCC(string from, List<string> to, List<string> cc, string subject, string body);
        Task<Result> SendToSingleEmail(string from, string to, string subject, string body);
        Task<Result> SendToSingleEmailCC(string from, string to, List<string> cc, string subject, string body);
    }
}
