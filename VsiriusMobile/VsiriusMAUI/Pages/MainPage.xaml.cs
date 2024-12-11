using VsiriusMAUI.Pages;
using VsiriusMAUI.ViewModels;

namespace VsiriusMAUI
{
	public partial class MainPage : ContentPage
	{
		public MainPage(MainViewModel vm)
		{
			InitializeComponent();
			BindingContext = vm;
		}

		private void SLogin(object sender, EventArgs e)
		{
			Navigation.PushAsync(new UploadPage());
		}
	}

}
