
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using VsiriusMAUI.Pages;
using VsiriusMAUI.ViewModels;

namespace VsiriusMAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkitCamera()
				.ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("FluentSystemIcons-Filled.ttf", "FluentIcons");
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainViewModel>();

            builder.Services.AddTransient<ScanPage>();
            builder.Services.AddTransient<ScanViewModel>();
            
            builder.Services.AddTransient<UploadPage>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
