using Microsoft.Maui.Controls;
using Newtonsoft.Json.Linq;
using VsiriusMAUI.ViewModels;

namespace VsiriusMAUI.Pages;

public partial class UploadPage : ContentPage
{
	public UploadPage()
	{
		InitializeComponent();
		BindingContext = new UploadViewModel();
	}

	private async void uploadImage(object sender, EventArgs e)
	{
		try
		{
			string action = await DisplayActionSheet("Choose an option", "Cancel", null, "Camera", "Gallery");
			FileResult? photo = null;

			if (action == "Camera")
			{
				var capturedPhoto = await MediaPicker.CapturePhotoAsync();
				if (capturedPhoto != null)
				{
					photo = capturedPhoto;
					photo.FileName = DateTime.UtcNow.ToString("ddMMyyyyHHmmss");
				}
				else
				{
					photo = null;
				}
			}
			else if (action == "Gallery")
			{
				var pickedPhoto = await MediaPicker.PickPhotoAsync();
				if (pickedPhoto != null)
				{
					photo = pickedPhoto;
					photo.FileName = DateTime.UtcNow.ToString("ddMMyyyyHHmmss");
				}
				else
				{
					photo = null;
				}
			}
			else if (action == "Cancel")
			{
				photo = null;
				return;
			}

			if (photo == null)
			{
				return;
			}


			var localFilePath = photo!.FullPath;

			if (sender is Image tappedImage)
			{
				tappedImage.Source = ImageSource.FromFile(localFilePath);
			}

		}
		catch (Exception ex)
		{
			loadingPageClicked(false);
			await DisplayAlert("Error", ex.ToString(), "Cancel");
			Console.WriteLine(ex.ToString());
		}
	}

	private void loadingPageClicked(bool b)
	{

		if (b)
		{
			LoadingPage.turnOnLoadingPage();
		}
		else
		{
			LoadingPage.turnOffLoadingPage();
		}
	}

	private async void GetImageData(object sender, EventArgs e)
	{
		var frames = new List<Frame> { Frame1, Frame2, Frame3, Frame4 };
		var fileStreams = new List<FileStream>();
		loadingPageClicked(true);
		using (var httpClient = new HttpClient())
		{
			try
			{
				var requestContent = new MultipartFormDataContent();

				foreach (var frame in frames)
				{
					if (frame.Content is Image image && image.Source is FileImageSource path)
					{
						if (!string.IsNullOrEmpty(path))
						{
							if (File.Exists(path))
							{
								var actualPath = path.File;
								Console.WriteLine($"File not found: {actualPath}");
								if (Path.GetExtension(path) != "addicon.png")
								{
									var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
									fileStreams.Add(fileStream);
									var fileContent = new StreamContent(fileStream);
									requestContent.Add(fileContent, "Images", Path.GetFileName(path));
								}
							}
						}
					}
				}

				var documentType = DocumentType.SelectedItem as string;
				if (string.IsNullOrEmpty(documentType))
				{
					await DisplayAlert("Lỗi", "Loại tài liệu là bắt buộc.", "OK");
					return;
				}

				if (string.IsNullOrEmpty(Notes.Text))
				{
					await DisplayAlert("Lỗi", "Ghi chú không được để trống.", "OK");
					return;
				}

				string test = transEnumtoNumber(documentType);


				requestContent.Add(new StringContent(test), "DocumentType");
				requestContent.Add(new StringContent(Notes.Text), "Notes");

				var url = "http://118.69.37.20:5200/api/Claim";
				var response = await httpClient.PostAsync(url, requestContent);
				var responseContent = await response.Content.ReadAsStringAsync();

				if (response.IsSuccessStatusCode)
				{
					var jsonResponse = JObject.Parse(responseContent);
					Notes.Text = "";
					foreach (var frame in frames)
					{
						if (frame.Content is Image image)
						{
							image.Source = "addicon.png"; 
						}
					}
					loadingPageClicked(false);
					ShowCustomAlert("Notification", "Success");
				}
				else
				{
					loadingPageClicked(false);
					ShowCustomAlert("Notification", "Unable to upload the image");
				}
			}
			catch (Exception ex)
			{
				loadingPageClicked(false);
				ShowCustomAlert($"Notification", $"Something:{ex.Message}");
			}
			finally
			{
				foreach (var stream in fileStreams)
				{
					stream.Dispose();
				}
			}
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

	private string transEnumtoNumber(string s)
	{
		switch (s)
		{
			case "Return Package": return "0";
			case "Order Documents": return "1";
			case "Shipped Package": return "2";
				default: return "";
		}
	}

}