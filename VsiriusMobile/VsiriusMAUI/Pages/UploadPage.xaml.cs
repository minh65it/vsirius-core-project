using System.Collections.ObjectModel;
using VsiriusMAUI.ViewModels;

namespace VsiriusMAUI.Pages;

public partial class UploadPage : ContentPage
{

	//public ObservableCollection<string> Options { get; set; }
	public UploadPage()
	{
		InitializeComponent();
		BindingContext = new UploadViewModel();
	}
}