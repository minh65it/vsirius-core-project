using CSRApi._Repositories.Interfaces;
using CSRApi._Repositories.Repositories;
using CSRApi._Services.Interfaces;
using CSRApi._Services.Services;
using Microsoft.EntityFrameworkCore;
using VsiriusDataModel.DataContext;

namespace VsiriusApi.DependencyInjection
{
    public static class CSR
    {
        public static void AddCSRServices(this IServiceCollection services, IConfiguration configuration)
        {
             
            // Register Repositories
            services.AddScoped<IFavouritesRepository, FavouritesRepository>();
            services.AddScoped<IVsiriusProductRepository, VsiriusProductRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<INotificationDetailRepository, NotificationDetailRepository>();
            services.AddScoped<INotificationMainRepository, NotificationMainRepository>();
            services.AddScoped<IDrivenHtmlPagesRepository, DrivenHtmlPagesRepository>();
            services.AddScoped<IPageViewRepository, PageViewRepository>();
            services.AddScoped<ISecretUserRepository, SecretUserRepository>();

            // Register Services
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IFavouritesService, FavouritesService>();
            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<IPageViewService, PageViewService>();
            services.AddScoped<IDrivenHtmlPagesService, DrivenHtmlPagesService>();

            // Config SignalR
            services.AddSignalR(options =>
            {
                options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
                options.KeepAliveInterval = TimeSpan.FromSeconds(30);
            });                     
        }
        
    }
}
