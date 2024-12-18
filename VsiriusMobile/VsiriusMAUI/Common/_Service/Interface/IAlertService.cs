using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsiriusMAUI.Common._Service.Interface
{
	public interface IAlertService
	{
		Task DisplayAlertAsync(string title, string message, string cancel);
	}
}
