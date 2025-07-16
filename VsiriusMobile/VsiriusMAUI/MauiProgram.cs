using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using VsiriusMAUI.Common._Service.Interface;
using VsiriusMAUI.Common._Service.Service;
using VsiriusMAUI.Configurations;

namespace VsiriusMAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            var getAssemebly = Assembly.GetExecutingAssembly();
            using var stream = getAssemebly.GetManifestResourceStream("VsiriusMAUI.appsetting.json");

            var config = new ConfigurationBuilder()
                .AddJsonStream(stream!)
                .Build();

            builder.Configuration.AddConfiguration(config);

            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkitCamera()
				.ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("FluentSystemIcons-Filled.ttf", "FluentIcons");
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            DIServiceRegistry.RegisterDIService(builder.Services);
            PageRegistry.RegisterPage(builder.Services);

            builder.Services.AddSingleton<LoadingService>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

			return builder.Build();
        }
    }
}
