using System.Net;
using VSiriusSystemLog.WriteLog;

namespace WalmartIntegrationDataApi.Common
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        private ILoggerCustom _logService;

        public ExceptionHandlingMiddleware(ILoggerCustom logService)
        {
            _logService = logService;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = error switch
                {
                    ApplicationException => (int)HttpStatusCode.BadRequest,// custom application error
                    KeyNotFoundException => (int)HttpStatusCode.NotFound,// not found error
                    UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,// Unauthorized error
                    _ => (int)HttpStatusCode.InternalServerError,// unhandled error
                };
                await _logService.CreateLogError(error.Message, "");
                await context.Response.WriteAsync(error.Message);
            }
        }
    }
}
