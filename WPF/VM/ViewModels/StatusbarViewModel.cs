using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.Models;

namespace VM.ViewModels
{
    public class StatusbarViewModel : BaseNotifyModel
    {
		private string message = string.Empty;

		public string Message
		{
			get { return message; }
			set { message = value; OnPropertyChanged("Message"); }
		}

	}
}
