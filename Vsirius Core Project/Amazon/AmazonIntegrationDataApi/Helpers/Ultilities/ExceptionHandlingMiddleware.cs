using System.Net;
using VSiriusSystemLog.WriteLog;

namespace AmazonIntegrationDataApi.Helpers.Ultilities
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
                switch (error)
                {
                    case ApplicationException e:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case KeyNotFoundException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case UnauthorizedAccessException e:
                        // Unauthorized error
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }
                await _logService.CreateLogError(error.Message, "");
                await context.Response.WriteAsync(error.Message);
            }
        }
    }
}
