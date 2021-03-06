﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Registration.Controllers;

namespace Registration.ViewModels
{
	public class HomeViewModel : BindableBase
	{
		private readonly RegistrationsController _registrationsController;

		public HomeViewModel(RegistrationsController registrationsController, GameController gameController, LiveController liveController)
		{
			_registrationsController = registrationsController;

			this.GoToRegistrationCommand = new DelegateCommand(GoToRegistration);
            this.OpenGameSelectionCommand = gameController.OpenGameSelectionCommand;
            this.LiveShowCommand = liveController.LiveShowCommand;
		}

		public ICommand GoToRegistrationCommand { get; private set; }
		public void GoToRegistration()
		{
			_registrationsController.ShowRegistrationView();
		}

        public ICommand OpenGameSelectionCommand { get; private set; }
        public ICommand LiveShowCommand { get; private set; }
	}
}

