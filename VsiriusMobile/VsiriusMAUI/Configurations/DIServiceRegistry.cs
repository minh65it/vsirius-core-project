using VsiriusMAUI.Common._Service.Interface;
using VsiriusMAUI.Common._Service.Service;

namespace VsiriusMAUI.Configurations
{
	public static class DIServiceRegistry
	{
		public static void RegisterDIService(IServiceCollection services)
		{
			services.AddSingleton<IAlertService, AlertService>();
		}
	}
}
