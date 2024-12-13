using CommunityToolkit.Maui.Views;

namespace VsiriusMAUI.Pages;

public partial class ScanPage : ContentPage
{
	public ScanPage()
	{
		InitializeComponent();
	}

	private void streamCaptured(object sender, MediaCapturedEventArgs e)
	{
		//if (Dispatcher.IsDispatchRequired)
		//{
		//	Dispatcher.Dispatch(() => CapturedImage.Source = ImageSource.FromStream(() => e.Media));
		//	return;
		//}

		//CapturedImage.Source = ImageSource.FromStream(() => e.Media);
	}
	 
	private async void funcScan(object sender, EventArgs e)
	{
		await MyCamera.CaptureImage(CancellationToken.None);
	}
}