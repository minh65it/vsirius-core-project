using VsiriusSystem._Repositories.Interfaces;
using VsiriusSystem._Repositories.Repositories;
using VsiriusSystem._Services.Interfaces;
using VsiriusSystem._Services.Services;

namespace VsiriusApi.DependencyInjection
{
    public static class System
    {
        public static void AddSystemServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ILogRepository, LogRepository>();
            services.AddTransient<IJobSchedulerRepository, JobSchedulerRepository>();
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<IJobSchedulerService, JobSchedulerService>();
            services.AddScoped<IApiKeysService, ApiKeysService>();

            services.AddScoped<ITextractAwsService, TextractAWSService>();
            services.AddSingleton<IKeyService, KeyService>();

            services.AddScoped<IChatGPTService, ChatGPTService>();
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.Cookie.IsEssential = true;
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });

            /*var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Production.json").Build();*/
            
        }
    }
}
