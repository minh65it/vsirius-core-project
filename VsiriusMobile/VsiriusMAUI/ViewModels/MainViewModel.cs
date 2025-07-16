using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;
using System.Windows.Input;
using VsiriusMAUI.Common._Service.Interface;
using VsiriusMAUI.Pages;

namespace VsiriusMAUI.ViewModels
{
	public partial class MainViewModel: ObservableObject
	{

		private readonly IAlertService _alertService;
		private readonly IConfiguration _configuration;

		private string _email = string.Empty;

		public string Email
		{
			get => _email;
			set => SetProperty(ref _email, value);
		}

		private string _password = string.Empty;
		public string Password
		{
			get => _password;
			set => SetProperty(ref _password, value);
		}


		private bool _isVisibleLoadingPage = false;
		public bool IsVisibleLoadingPage
		{
			get => _isVisibleLoadingPage;
			set => SetProperty(ref _isVisibleLoadingPage, value);	
		}

		public ICommand Loggin { get; }

		public MainViewModel(IAlertService alertService, IConfiguration configuration)
		{
			Loggin = new Command(() => _loggin());

			_alertService = alertService;
			_configuration = configuration;
		}

		private async void _loggin()
		{
			try
			{
				IsVisibleLoadingPage = true;
				string url = _configuration["LoginApi"]!;

				var rBody = new
				{
					email = Email,
					password = Password.Replace(" ", ""),
					rememberMe = true
				};

				var handler = new HttpClientHandler
				{

					ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
				};

				using (var httpClient = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(30) })
				{

					var jsonContent = new StringContent(JsonConvert.SerializeObject(rBody), Encoding.UTF8, "application/json");
					var response = await httpClient.PostAsync(url, jsonContent);

					if (response.IsSuccessStatusCode)
					{
						IsVisibleLoadingPage = false;

						await Shell.Current.GoToAsync(nameof(UploadPage));
						
					}
					else
					{
						await _alertService.DisplayAlertAsync("Notification", "Email or password is incorrect", "Close");
						IsVisibleLoadingPage = false;
					}
				}
			}
			catch (Exception ex)
			{
				IsVisibleLoadingPage = false;
				await _alertService.DisplayAlertAsync("Error", ex.ToString(), "Close");
			}
			finally
			{
				IsVisibleLoadingPage = false;
			}
		}

	}
}
