﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using Registration.Events;
using Registration.Views;

namespace Registration.ViewModels
{
	public class ShellViewModel : BindableBase
	{
		private readonly IEventAggregator _eventAggregator;

		public ShellViewModel(IEventAggregator eventAggregator)
		{
			this.IsBusy = true;

			_eventAggregator = eventAggregator;
			_eventAggregator.GetEvent<ModuleInitialized>().Subscribe((e) => this.IsBusy = false);
		}

		private bool _isBusy;
		public bool IsBusy
		{
			get
			{
				return _isBusy;
			}
			set
			{
				SetProperty(ref _isBusy, value);
				this.OnPropertyChanged(() => this.SplashVisibility);
				this.OnPropertyChanged(() => this.ShellVisibility);
			}
		}

		public Visibility SplashVisibility
		{
			get { return _isBusy ? Visibility.Visible : Visibility.Hidden; }
		}

		public Visibility ShellVisibility
		{
			get { return _isBusy ? Visibility.Hidden : Visibility.Visible; }
		}
	}
}
