using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Messaging;
using Microsoft.Practices.Prism.Mvvm;
using Registration.Controllers;

namespace Registration.ViewModels
{
	public class HomeViewModel : BindableBase
	{
		public HomeViewModel(RegistrationsController registrationsController)
		{
		}

		public ICommand GoToRegistrationCommand { get; private set; }
	}
}

