using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Windows.Input;

namespace VsiriusMAUI.ViewModels
{
	public partial class MainViewModel: ObservableObject
	{
		[ObservableProperty]
		public bool isPassword = true;

		[ObservableProperty]
		public string iconPassword = "Hide";

		public ICommand TogglePasswordCommand { get; }


		public MainViewModel()
		{
			TogglePasswordCommand = new Command(OnCick);
		}

		private void OnCick()
		{
			IsPassword = !IsPassword;
			IconPassword = IsPassword ? "Hide" : "Show";
		}

	}
}
