namespace VsiriusMAUI.Pages.common;

public partial class LoadingPage : ContentView
{
	
	public LoadingPage()
	{
		InitializeComponent();
	}

	public void turnOnLoadingPage()
	{
		CustomAlert.IsVisible = true;
	}

	public void turnOffLoadingPage()
	{
		CustomAlert.IsVisible = false;
	}
	
}