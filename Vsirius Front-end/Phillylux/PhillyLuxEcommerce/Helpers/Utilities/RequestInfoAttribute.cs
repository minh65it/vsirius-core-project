using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using PhillyLuxEcommerce.Dtos.Params;
using System.Text;

namespace PhillyLuxEcommerce.Helpers.Utilities
{
    public class RequestInfoAttribute : ActionFilterAttribute
    {
        private string _title = string.Empty;
        private IConfigurationRoot _configuration;

        public RequestInfoAttribute(string title)
        {
            _title = title;
            _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            RequestInfoParam requestInfo = new RequestInfoParam();
            requestInfo.PageURL = filterContext.HttpContext.Request.Path;

            try
            {
                if (filterContext.HttpContext.Request.Method == "GET")
                {
                    requestInfo.PageParam = filterContext.HttpContext.Request.QueryString.ToString();
                }
                else
                {
                    requestInfo.PageParam = JsonConvert.SerializeObject(filterContext.HttpContext.Request.Form);
                }

                requestInfo.PageTitle = _title;
                var json = JsonConvert.SerializeObject(requestInfo);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var apiUrl = _configuration.GetSection("ViewAnalytic").Value;
                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(apiUrl!),
                    Content = content,
                    Method = new HttpMethod("POST")
                };

                var task = Task.Run(() =>
                {
                    using (var client = new HttpClient())
                    {
                        client.Send(request);
                    }
                });

            }
            catch (Exception)
            {
                // Handle exception
            }
        }
    }
}
