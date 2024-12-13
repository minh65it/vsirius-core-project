using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VsiriusMAUI.ViewModels
{
	public partial class UploadViewModel: INotifyPropertyChanged
	{
		public ObservableCollection<string> Images { get; set; } = new ObservableCollection<string>();
		public ObservableCollection<string> Options { get; set; }
		public double Width => DeviceDisplay.MainDisplayInfo.Width;

		private string _selectedOption;

		public string SelectedOption
		{
			get => _selectedOption;
			set
			{
				if (_selectedOption != value)
				{
					_selectedOption = value;
					OnPropertyChanged(nameof(SelectedOption));
				}
			}
		}

		public ICommand AddImageCommand { get; }

		public UploadViewModel()
		{
			// Initialize Options
			Options = new ObservableCollection<string>
			{
				"Return Package",
				"Order Documents",
				"Shipped Package"
			};

			AddImageCommand = new Command(async () => await OnAddImage());
		}

		private async Task OnAddImage()
		{
			var result = await MediaPicker.PickPhotoAsync();
			if (result != null)
			{
				Images.Add(result.FullPath);
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
