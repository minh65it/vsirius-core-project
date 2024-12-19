using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json;

namespace VsiriusMAUI.ViewModels
{
	[QueryProperty(nameof(JsonList), "JsonList")]
	public partial class SubmitTextScanViewModel : ObservableObject
	{
		// private List<string>? jsonList;
		private string? jsonList;

		public string JsonList
		{
			get => jsonList ?? string.Empty;
			set => SetProperty(ref jsonList, value);
			//set
			//{
			//	jsonList = JsonSerializer.Deserialize<List<string>>(value);
			//}
		}
	}
}
