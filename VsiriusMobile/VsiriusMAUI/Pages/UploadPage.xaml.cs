using VsiriusMAUI.ViewModels;

namespace VsiriusMAUI.Pages;

public partial class UploadPage : ContentPage
{
	public UploadPage(UploadViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

	//private async void GetImageData(object sender, EventArgs e)
	//{
	//	var frames = new List<Frame> { Frame1, Frame2, Frame3, Frame4 };
	//	var fileStreams = new List<FileStream>();
	//	loadingPageClicked(true);
	//	using (var httpClient = new HttpClient())
	//	{
	//		try
	//		{
	//			var requestContent = new MultipartFormDataContent();

	//			foreach (var frame in frames)
	//			{
	//				if (frame.Content is Image image && image.Source is FileImageSource path)
	//				{
	//					if (!string.IsNullOrEmpty(path))
	//					{
	//						if (File.Exists(path))
	//						{
	//							var actualPath = path.File;
	//							Console.WriteLine($"File not found: {actualPath}");
	//							if (Path.GetExtension(path) != "addicon.png")
	//							{
	//								var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
	//								fileStreams.Add(fileStream);
	//								var fileContent = new StreamContent(fileStream);
	//								requestContent.Add(fileContent, "Images", Path.GetFileName(path));
	//							}
	//						}
	//					}
	//				}
	//			}

	//			var documentType = DocumentType.SelectedItem as string;
	//			if (string.IsNullOrEmpty(documentType))
	//			{
	//				await DisplayAlert("Lỗi", "Loại tài liệu là bắt buộc.", "OK");
	//				return;
	//			}

	//			if (string.IsNullOrEmpty(Notes.Text))
	//			{
	//				await DisplayAlert("Lỗi", "Ghi chú không được để trống.", "OK");
	//				return;
	//			}

	//			string test = transEnumtoNumber(documentType);


	//			requestContent.Add(new StringContent(test), "DocumentType");
	//			requestContent.Add(new StringContent(Notes.Text), "Notes");

	//			var url = "http://118.69.37.20:5200/api/Claim";
	//			var response = await httpClient.PostAsync(url, requestContent);
	//			var responseContent = await response.Content.ReadAsStringAsync();

	//			if (response.IsSuccessStatusCode)
	//			{
	//				var jsonResponse = JObject.Parse(responseContent);
	//				Notes.Text = "";
	//				foreach (var frame in frames)
	//				{
	//					if (frame.Content is Image image)
	//					{
	//						image.Source = "addicon.png"; 
	//					}
	//				}
	//				loadingPageClicked(false);
	//				ShowCustomAlert("Notification", "Success");
	//			}
	//			else
	//			{
	//				loadingPageClicked(false);
	//				ShowCustomAlert("Notification", "Unable to upload the image");
	//			}
	//		}
	//		catch (Exception ex)
	//		{
	//			loadingPageClicked(false);
	//			ShowCustomAlert($"Notification", $"Something:{ex.Message}");
	//		}
	//		finally
	//		{
	//			foreach (var stream in fileStreams)
	//			{
	//				stream.Dispose();
	//			}
	//		}
	//	}
	//}

	//private string transEnumtoNumber(string s)
	//{
	//	switch (s)
	//	{
	//		case "Return Package": return "0";
	//		case "Order Documents": return "1";
	//		case "Shipped Package": return "2";
	//			default: return "";
	//	}
	//}

}