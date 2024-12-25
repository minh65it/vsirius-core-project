using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsiriusMAUI.Pages;
using VsiriusMAUI.Pages.common;
using VsiriusMAUI.ViewModels;


namespace VsiriusMAUI.Configurations
{
	public static class PageRegistry
	{
		public static void RegisterPage(IServiceCollection services)
		{
			services.AddSingleton<MainPage>();
			services.AddSingleton<MainViewModel>();

			services.AddTransient<UploadPage>();
			services.AddTransient<UploadViewModel>();

			services.AddTransient<SubmitTextScanViewModel>();
			services.AddTransient<SubmitSTextScanPage>();

			services.AddSingleton<LoadingPage>();
		}
	}
}
