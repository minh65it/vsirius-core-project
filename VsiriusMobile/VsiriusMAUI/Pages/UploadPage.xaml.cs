using VsiriusMAUI.ViewModels;

namespace VsiriusMAUI.Pages;

public partial class UploadPage : ContentPage
{
	public UploadPage(UploadViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}