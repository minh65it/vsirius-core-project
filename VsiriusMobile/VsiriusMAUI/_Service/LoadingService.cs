using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsiriusMAUI.Common._Service.Service
{
	public class LoadingService 
	{
		public event Action? LoadingStarted;
		public event Action? LoadingStopped;

		public void TurnOnLoadingPage()
		{
			LoadingStarted?.Invoke();
		}

		public void TurnOffLoadingPage()
		{
			LoadingStopped?.Invoke();
		}
	}
}
