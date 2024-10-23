using IdentityApi._Repositoties;
using IdentityApi._Repositoties.Interfaces;
using IdentityApi._Repositoties.Repositories;
using IdentityApi._Services.Interfaces;
using IdentityApi._Services.Services;
using IdentityApi.Helpers.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace IdentityApi.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static void AddDependencyInjectionConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            //User
            services.AddScoped<IFunctionUtilities, FunctionUtilities>();
            services.AddScoped<IValidUser, ValidUser>();
            //Mail
            services.AddScoped<IEmailSender, EmailSender>();
            //Http
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(x => {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });

            services.AddScoped<IRepositoryAccessor, RepositoryAccessor>();
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAzureStorage, AzureStorage>();

            //Employee
            services.AddScoped<IEmployeeService, EmployeeService>();
        }
    }
}
