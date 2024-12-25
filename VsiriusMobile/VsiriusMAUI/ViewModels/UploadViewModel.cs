using Amazon;
using Amazon.Runtime;
using Amazon.Textract;
using Amazon.Textract.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows.Input;
using VsiriusMAUI.Common._Service.Interface;
using VsiriusMAUI.Dtos.ScanImage;

namespace VsiriusMAUI.ViewModels
{
	public partial class UploadViewModel : ObservableObject
	{
		private readonly IAlertService _alertService;
		private readonly IConfiguration _configuration;

		#region Property

		public ObservableCollection<string> Images { get; set; } = new ObservableCollection<string>();

		public ObservableCollection<string> Options { get; set; }

		private string _note = string.Empty;

		public string Note
		{
			get => _note;
			set => SetProperty(ref _note, value);
		}

		private string _selectedOption = "-- Select an option --";

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

		private bool _isVisibleLoadingPage = false;
		public bool IsVisibleLoadingPage
		{
			get => _isVisibleLoadingPage;
			set => SetProperty(ref _isVisibleLoadingPage, value);
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
		public ICommand ScanImage { get; }
		public ICommand UploadImage { get; }
		public ICommand UploadData { get; }

		#endregion

		public UploadViewModel(IAlertService alertService, IConfiguration configuration)
		{
			_alertService = alertService;
			_configuration = configuration;
			Options = new ObservableCollection<string>
			{
				"-- Select an option --",
				"Return Package",
				"Order Documents",
				"Shipped Package"
			};

			UploadData = new Command(async () => await _UploadData());
			ScanImage = new Command(async () => await _scanImage());
			UploadImage = new Command<string>(async (index) => await _UploadImage(index));
		}

		private async Task _UploadImage(string index)
		{
			try
			{
				var action = await _alertService.DisplayActionSheet(new List<string> { "Camera", "Gallery" });
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

				SetPath(int.Parse(index), localFilePath);

			}
			catch (Exception ex)
			{
				await _alertService.DisplayAlertAsync("Error", $"{ex.ToString()}", "Close");
			}
		}

		private async Task _scanImage()
		{
			try
			{
				IsVisibleLoadingPage = true;
				var paths = new List<string> { Path1, Path2, Path3, Path4 };

				var results = new List<string>();

				foreach (var path in paths)
				{
					if (string.IsNullOrEmpty(path)) continue;
					var result = await Image2Text(path);
					results.Add(result);
				}

				var jsonList = JsonSerializer.Serialize(results);

				IsVisibleLoadingPage = false;

				// await Shell.Current.GoToAsync($"{nameof(SubmitSTextScanPage)}?JsonList={jsonList}");

			}
			catch (Exception ex)
			{
				IsVisibleLoadingPage = false;
				await _alertService.DisplayAlertAsync("Error", $"{ex.ToString}", "Close");
			}


		}

		private async Task _UploadData()
		{
			IsVisibleLoadingPage = true;
			var paths = new List<string> { Path1, Path2, Path3, Path4 };
			// List<StreamContent> lstClaimTextParam = new List<StreamContent>();
			var fileStreams = new List<FileStream>();
			using (var httpClient = new HttpClient())
			{
				try
				{
					var requestContent = new MultipartFormDataContent();

					//foreach (var path in paths)
					//{
					//	if (!string.IsNullOrEmpty(path))
					//	{

					//		using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
					//		{
					//			lstClaimTextParam.Add(new StreamContent(fileStream));
					//		}
					//	}
					//}


					//if (!string.IsNullOrEmpty(paths[0]))
					//{

					//	using (var fileStream = new FileStream(paths[0], FileMode.Open, FileAccess.Read))
					//	{
					//		lstClaimTextParam.Add(new StreamContent(fileStream));
					//	}
					//}



					var documentType = SelectedOption;
					if (string.IsNullOrEmpty(documentType) || documentType == "-- Select an option --")
					{
						await _alertService.DisplayAlertAsync("Notification", "Document type is required", "Close");
						return;
					}

					if (string.IsNullOrEmpty(Note))
					{
						await _alertService.DisplayAlertAsync("Notification", "Note type is required", "Close");
						return;
					}


					requestContent.Add(new StringContent(transEnumtoNumber(documentType).ToString()), "DocumentType");
					requestContent.Add(new StringContent(Note), "Notes");

					//foreach (var imageInfo in lstClaimTextParam)
					//{
					//	if (imageInfo!= null)
					//	{
					//		var stream = await imageInfo.ReadAsStreamAsync();
					//		var memoryStream = new MemoryStream();
					//		await stream.CopyToAsync(memoryStream);
					//		memoryStream.Position = 0;

					//		var streamContent = new StreamContent(memoryStream);
					//		streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

					//		requestContent.Add(streamContent, "Images", DateTime.Now.ToString()); 
					//	}
					//}

					var fileStream = new FileStream(paths[0], FileMode.Open, FileAccess.Read);
					
						var fileName = Path.GetFileName(paths[0]);
						var fileContent = new StreamContent(fileStream);
						fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
						requestContent.Add(fileContent, "Images", fileName);
					



					var url = _configuration["UploadImageApi"];

					var response = await httpClient.PostAsync(url, requestContent);
					var responseContent = await response.Content.ReadAsStringAsync();

					if (response.IsSuccessStatusCode)
					{
						var jsonResponse = JObject.Parse(responseContent);
						Note = string.Empty;
						SelectedOption = "-- Select an option --";
						//for (int i = 0; i < paths.Count; i++)
						//{
						//	SetPath(i, string.Empty);
						//}
						SetPath(0, string.Empty);
						IsVisibleLoadingPage = false;
						await _alertService.DisplayAlertAsync("Notification", "Success", "Close");
					}
					else
					{
						IsVisibleLoadingPage = false;
						await _alertService.DisplayAlertAsync("Notification", $"{response.RequestMessage}", "Close");
					}
				}
				catch (Exception ex)
				{
					IsVisibleLoadingPage = false;
					await _alertService.DisplayAlertAsync("Error", $"{ex.ToString()}", "Close");
				}
				finally
				{
					IsVisibleLoadingPage = false;
					foreach (var stream in fileStreams)
					{
						stream.Dispose();
					}
				}
			}
		}

		private DocumentTypeDto transEnumtoNumber(string s)
		{
			switch (s)
			{
				case "Return Package": return DocumentTypeDto.ReturnPackage;
				case "Order Documents": return DocumentTypeDto.OrderDocuments;
				case "Shipped Package": return DocumentTypeDto.ShippedPackage;
				default: return DocumentTypeDto.None;
			}
		}

		private async Task<string> Image2Text(string path)
		{
			try
			{
				var accessKey = _configuration["AmazonKey:AccessKey"]!;
				var secretKey = _configuration["AmazonKey:SecretKey"]!;
				var credentials = new BasicAWSCredentials(accessKey, secretKey);
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
	}
}
