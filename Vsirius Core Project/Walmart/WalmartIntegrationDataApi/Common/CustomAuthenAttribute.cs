using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WalmartIntegrationDataApi.Common
{
    public class CustomAuthenAttribute:ActionFilterAttribute
    {
        private IConfigurationRoot _configuration;
        public CustomAuthenAttribute() {

            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            var requestSecret = context.HttpContext.Request.Headers["Secret"].ToString();
            var configSecret = _configuration.GetSection("AppSecretToken").Value;

            if(configSecret != requestSecret)
            {              
                context.Result = new BadRequestResult();
            }
        }
    }
}
