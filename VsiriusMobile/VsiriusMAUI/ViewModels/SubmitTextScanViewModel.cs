using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json;

namespace VsiriusMAUI.ViewModels
{
	[QueryProperty(nameof(JsonList), "JsonList")]
	public partial class SubmitTextScanViewModel : ObservableObject
	{
		private string? jsonList;

		public string JsonList
		{
			get => jsonList ?? string.Empty;
			set => SetProperty(ref jsonList, value);
		}


		private List<string>? _listDescription;

		public List<string> ListDescription
		{
			get => _listDescription ?? new List<string> { };
			set
			{
				_listDescription = JsonSerializer.Deserialize<List<string>>(JsonList);
			}
		}
	}
}
