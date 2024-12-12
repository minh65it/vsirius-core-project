using Newtonsoft.Json;
using System.Text;
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

		private async void funcLogin(object sender, EventArgs e)
		{
			try
			{
				//string action = await DisplayActionSheet("Choose an option", "Cancel", null, "Camera", "Gallery");
				//FileResult? photo = null;
				//if (action == "Camera")
				//{
				//	photo = await MediaPicker.CapturePhotoAsync() ?? new FileResult("dotnet_bot.png");
				//	photo.FileName = DateTime.UtcNow.ToString("ddMMyyyyHHmmss");

				//}
				//else if (action == "Gallery")
				//{
				//	photo = await MediaPicker.PickPhotoAsync() ?? new FileResult("dotnet_bot.png");
				//	photo.FileName = DateTime.UtcNow.ToString("ddMMyyyyHHmmss");
				//}

				//var localFilePath = photo!.FullPath;


				//var platform = DeviceInfo.Platform;
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
						await Navigation.PushAsync(new UploadPage());
					}
					else
					{
						await DisplayAlert("Error", "Fail", "Cancel");
					}
				}
			}
			catch (Exception ex)
			{
				await DisplayAlert("Error", ex.ToString(), "Cancel");
				Console.WriteLine(ex.ToString());
			}
		}
	}

}
