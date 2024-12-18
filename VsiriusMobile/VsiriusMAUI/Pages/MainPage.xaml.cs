using Newtonsoft.Json;
using System.Text;
using VsiriusMAUI.Pages;
using VsiriusMAUI.Pages.common;
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

		private void CheckedCheckBox(object sender, EventArgs e)
		{
			password.IsPassword = !password.IsPassword;
		}

		//private void loadingPageClicked(bool b)
		//{

		//	if (b)
		//	{
		//		LoadingPage.turnOnLoadingPage();
		//	} else
		//	{
		//		LoadingPage.turnOffLoadingPage();
		//	}
		//}

		//private void ShowCustomAlert(string title, string message)
		//{
		//	if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(message))
		//		return;

		//	CustomAlert.IsVisible = true;
		//	NotiText.Text = message;
		//	NotiTitle.Text = title;
		//}

		//private void OnAlertOkClicked(object sender, EventArgs e)
		//{
		//	CustomAlert.IsVisible = false;
		//}
	}

}
