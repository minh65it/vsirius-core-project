using VsiriusMAUI.ViewModels;

namespace VsiriusMAUI.Pages;

public partial class SubmitSTextScanPage : ContentPage
{
	public SubmitSTextScanPage(SubmitTextScanViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}