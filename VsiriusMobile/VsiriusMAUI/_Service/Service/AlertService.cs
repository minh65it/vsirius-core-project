using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsiriusMAUI.Common._Service.Interface;

namespace VsiriusMAUI.Common._Service.Service
{
	public class AlertService : IAlertService
	{
		public async Task DisplayAlertAsync(string title, string message, string cancel)
		{
			try
			{
				if (Application.Current?.MainPage is Shell shell)
				{
					var currentPage = shell.CurrentPage;
					if (currentPage != null)
					{
						await currentPage.DisplayAlert(title, message, cancel);
					}
				}
				else if (Application.Current?.MainPage != null)
				{
					await Application.Current.MainPage.DisplayAlert(title, message, cancel);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error displaying alert: {ex.Message}");
			}
		}
		public async Task<string> DisplayActionSheet(List<string> action)
		{
			try
			{
				if (Application.Current?.MainPage is Shell shell)
				{
					var currentPage = shell.CurrentPage;
					if (currentPage != null)
					{
						var result = await currentPage.DisplayActionSheet("Choose an option", "Cancel", null, action.ToArray());
						return result;
					}
				}
				return string.Empty;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error displaying alert: {ex.Message}");
				return string.Empty;
			}
		}

	}
}
