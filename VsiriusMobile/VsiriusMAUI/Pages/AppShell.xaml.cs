using VsiriusMAUI.Pages;

namespace VsiriusMAUI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(UploadPage), typeof(UploadPage));
			Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
			Routing.RegisterRoute(nameof(SubmitSTextScanPage), typeof(SubmitSTextScanPage));
		}
    }
}
