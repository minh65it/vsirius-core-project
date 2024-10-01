using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace QuartzInfrastructure
{
    public static class DependencyInjection
    {
        public static void AddJob<T>(this IServiceCollection services) where T : IJob
        {
            _ = services.AddQuartz();

            services.AddQuartzHostedService(opts =>
            {
                opts.WaitForJobsToComplete = true;
            });

            services.ConfigureOptions<BackgroundJobSetup<T>>();
        }
        public static IServiceCollection AddUpdateJobs(this IServiceCollection services)
        {
            services.AddScoped<IUpdateJobsService, UpdateJobsService>();

            return services;
        }
    }
}
