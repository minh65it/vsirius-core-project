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

		//private void OnTogglePasswordVisibilityClicked(object sender, EventArgs e)
		//{
		//	password.IsPassword = !password.IsPassword;
		//	togglePasswordVisibility.Text = password.IsPassword ? "👁️" : "🚫";
		//}

		private void CheckboxCheckedChanged(object sender, EventArgs e)
		{
			password.IsPassword = !password.IsPassword;
		}

		private async void funcLogin(object sender, EventArgs e)
		{
			try
			{
				loadingPageClicked(true);
				string url = "http://api.vsirius.com/identity/api/Auth/Login";

				var rBody = new
				{
					email = email.Text,
					password = password.Text,
					rememberMe = true
				};

				using (var httpClient = new HttpClient())
				{
					var jsonContent = new StringContent(JsonConvert.SerializeObject(rBody), Encoding.UTF8, "application/json");
					var response = await httpClient.PostAsync(url, jsonContent);

					if (response.IsSuccessStatusCode)
					{
						loadingPageClicked(false);
						await Navigation.PushAsync(new UploadPage());
					}
					else
					{
						loadingPageClicked(false);
						ShowCustomAlert("Notification", "Email or password is incorrect");
					}
				}
			}
			catch (Exception ex)
			{
				await DisplayAlert("Error", ex.ToString(), "Cancel");
				Console.WriteLine(ex.ToString());
			}
		}

		private void loadingPageClicked(bool b)
		{

			if (b)
			{
				LoadingPage.turnOnLoadingPage();
			} else
			{
				LoadingPage.turnOffLoadingPage();
			}
		}

		private void ShowCustomAlert(string title, string message)
		{
			CustomAlert.IsVisible = true;

			NotiText.Text = message;
			NotiTitle.Text = title;
		}

		private void OnAlertOkClicked(object sender, EventArgs e)
		{
			CustomAlert.IsVisible = false;
		}
	}

}
