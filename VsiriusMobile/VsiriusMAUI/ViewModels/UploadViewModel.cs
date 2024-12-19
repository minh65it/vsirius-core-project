using Amazon;
using Amazon.Runtime;
using Amazon.Textract;
using Amazon.Textract.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using VsiriusMAUI.Common._Service.Interface;

namespace VsiriusMAUI.ViewModels
{
	public partial class UploadViewModel: ObservableObject
	{
		private readonly IAlertService _alertService;

		public ObservableCollection<string> Images { get; set; } = new ObservableCollection<string>();

		public ObservableCollection<string> Options { get; set; }

		private string _keyorPath = "REMOVED_AWS_KEY";
		
		public string KeyorPath
		{
			get => _keyorPath;
		}

		private string _secretKey = "eMiQIRN8j4srfwbpdOBTGeUr81j2TUUu894fvOiD";
		public string SecretKey
		{
			get => _secretKey;
		}

		private string _note = string.Empty;

		public string Note
		{
			get => _note;
			set => SetProperty(ref _note, value);
		}

		private string _selectedOption = "Select an option";

		public string SelectedOption
		{
			get => _selectedOption;
			set => SetProperty(ref _selectedOption, value);
		}

		private readonly string[] _paths = new string[4];

		public string GetPath(int index) => _paths[index];

		public void SetPath(int index, string value)
		{
			if (index >= 0 && index < _paths.Length)
			{
				_paths[index] = value;
				OnPropertyChanged($"Path{index + 1}");
				OnPropertyChanged($"IsImageNull{index + 1}");
			}
		}

		public bool GetIsImageNull(int index) => string.IsNullOrEmpty(GetPath(index));

		public string Path1 => GetPath(0);
		public string Path2 => GetPath(1);
		public string Path3 => GetPath(2);
		public string Path4 => GetPath(3);

		public bool IsImageNull1 => GetIsImageNull(0);
		public bool IsImageNull2 => GetIsImageNull(1);
		public bool IsImageNull3 => GetIsImageNull(2);
		public bool IsImageNull4 => GetIsImageNull(3);

		public ICommand AddImageCommand { get; }
		public ICommand Image2Text { get; }
		public ICommand ScanImage { get; }

		public UploadViewModel(IAlertService alertService)
		{
			_alertService = alertService;
			Options = new ObservableCollection<string>
			{
				"Select an option",
				"Return Package",
				"Order Documents",
				"Shipped Package"
			};

			AddImageCommand = new Command(async () => await OnAddImage());
			Image2Text = new Command<string>(async (path) => await _image2Text(path));
			ScanImage = new Command(async () => await _scanImage());
		}

		private async Task<List<string>> _scanImage()
		{
			var paths = new List<string> { Path1, Path2, Path3, Path4 };

			var results = new List<string>();

			foreach(var path in paths)
			{
				var result = await _image2Text(path);
				results.Add(result);
			}

			return results;

		}

		//private async void GetImageData(object sender, EventArgs e)
		//{
		//	var frames = new List<string> { Path1, Path2, Path3, Path4 };
		//	var fileStreams = new List<FileStream>();
		//	// loadingPageClicked(true);
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

		//			var documentType = SelectedOption;
		//			if (string.IsNullOrEmpty(documentType))
		//			{
		//				await _alertService.DisplayAlertAsync("Notification", "Document type is required", "Close");
		//				return;
		//			}

		//			if (string.IsNullOrEmpty(Note))
		//			{
		//				await _alertService.DisplayAlertAsync("Notification", "Note type is required", "Close");
		//				return;
		//			}

		//			string test = transEnumtoNumber(documentType);


		//			requestContent.Add(new StringContent(test), "DocumentType");
		//			requestContent.Add(new StringContent(Note), "Notes");

		//			var url = "http://118.69.37.20:5200/api/Claim";
		//			var response = await httpClient.PostAsync(url, requestContent);
		//			var responseContent = await response.Content.ReadAsStringAsync();

		//			if (response.IsSuccessStatusCode)
		//			{
		//				var jsonResponse = JObject.Parse(responseContent);
		//				Note = string.Empty;
		//				foreach (var frame in frames)
		//				{
		//					if (frame.Content is Image image)
		//					{
		//						image.Source = "addicon.png";
		//					}
		//				}
		//				// loadingPageClicked(false);
		//				// ShowCustomAlert("Notification", "Success");
		//				await _alertService.DisplayAlertAsync("Notification", "Success", "Close");
		//			}
		//			else
		//			{
		//				// loadingPageClicked(false);
		//				// ShowCustomAlert("Notification", "Unable to upload the image");
		//			}
		//		}
		//		catch (Exception ex)
		//		{
		//			// loadingPageClicked(false);
		//			await _alertService.DisplayAlertAsync("Error", $"{ex.ToString()}", "Close");
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

		private async Task<string> _image2Text(string path)
		{
			try
			{
				var credentials = new BasicAWSCredentials(KeyorPath, SecretKey);
				StringBuilder sb = new();

				MemoryStream stream = await Common.Utilities.Common.ReadFile(path);

				byte[] byteArray = stream.ToArray();

				AmazonTextractClient textract = new AmazonTextractClient(credentials, RegionEndpoint.USEast1);
				DetectDocumentTextRequest request = new DetectDocumentTextRequest
				{
					Document = new Document
					{
						Bytes = new MemoryStream(byteArray)
					}
				};
				DetectDocumentTextResponse response = await textract.DetectDocumentTextAsync(request);

				foreach (var block in response.Blocks)
				{
					if (block.BlockType == BlockType.LINE)
					{
						sb.AppendLine(block.Text);
					}
				}

				return sb.ToString();
			}
			catch { return string.Empty; }

		}

		private async Task OnAddImage()
		{
			var result = await MediaPicker.PickPhotoAsync();
			if (result != null)
			{
				Images.Add(result.FullPath);
			}
		}
	}
}
