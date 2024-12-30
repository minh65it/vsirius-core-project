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
		private void CheckedCheckBox(object sender, EventArgs e)
		{
			password.IsPassword = !password.IsPassword;
		}
	}
}
