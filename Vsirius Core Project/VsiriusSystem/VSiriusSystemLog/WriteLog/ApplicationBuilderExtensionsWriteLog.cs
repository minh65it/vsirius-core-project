using Microsoft.Extensions.DependencyInjection;

namespace VSiriusSystemLog.WriteLog
{
    public static class ApplicationBuilderExtensionsWriteLog
    {
        public static IServiceCollection AddWriteLog(this IServiceCollection services)
        {
            services.AddScoped<ILoggerCustom, LoggerCustom>();

            return services;
        }
    }
}
