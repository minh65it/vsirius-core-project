using System.Net;

namespace SkuMappingAPI.Common
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
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
                await context.Response.WriteAsync(error.Message);
            }
        }
    }
}
