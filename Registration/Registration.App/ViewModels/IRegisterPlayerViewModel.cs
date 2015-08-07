using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Registration.ViewModels
{
	public interface IRegisterPlayerViewModel
	{
		event EventHandler CloseViewRequested;

        Guid ContestId { get; set; }
	}
}
