using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace VSiriusSystemLog.CorrelationId
{
    public static class ApplicationBuilderExtensionsCorrelationId
    {
        public static IServiceCollection AddCorrelationIdGeneratorService(this IServiceCollection services)
        {
            services.AddScoped<ICorrelationIdGenerator, CorrelationIdGenerator>();

            return services;
        }
        public static IApplicationBuilder AddCorrelationIdMiddleware(this IApplicationBuilder applicationBuilder)
        => applicationBuilder.UseMiddleware<CorrelationIdMiddleware>();
    }
}
