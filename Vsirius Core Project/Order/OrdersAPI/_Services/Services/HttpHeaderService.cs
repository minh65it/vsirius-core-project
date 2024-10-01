using OrdersAPI._Services.Interfaces;
using OrdersAPI.Dtos;

namespace OrdersAPI._Services.Services
{
    public class HttpHeaderService : IHttpHeaderService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpHeaderService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public HttpHeaderDto GetHeaders()
        {
            HttpContext context = _httpContextAccessor.HttpContext;

            DateTime dateHeaders = new DateTime();
            if (context.Response.Headers.TryGetValue("Date", out var dateValues))
            {
                string dateValue = dateValues.ToString();

                if (DateTime.TryParse(dateValue, out DateTime date))
                {
                    dateHeaders = date;
                }
            }

            var result = new HttpHeaderDto
            {
                UserAgent = context.Request.Headers["User-Agent"],
                SecChUa = context.Request.Headers["Sec-Ch-Ua"],
                SecChUaMobile = context.Request.Headers["Sec-Ch-Ua-Mobile"],
                SecChUaPlatform = context.Request.Headers["Sec-Ch-Ua-Platform"],
                Date = dateHeaders == DateTime.MinValue ? DateTime.UtcNow : dateHeaders
            };
            
            return result;
        }
    }
}
