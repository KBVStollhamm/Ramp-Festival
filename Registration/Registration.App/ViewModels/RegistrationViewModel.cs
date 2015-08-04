using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Registration.Controllers;

namespace Registration.ViewModels
{
	public class RegistrationViewModel
	{
		public RegistrationViewModel(RegistrationsController controller)
		{
			this.RegisterPlayerCommand = controller.RegisterPlayerCommand;
			this.RegisterTeamCommand = controller.RegisterTeamCommand;
		}

		public ICommand RegisterPlayerCommand { get; private set; }
		public ICommand RegisterTeamCommand { get; private set; }
	}
}
