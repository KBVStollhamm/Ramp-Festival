using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using Registration.ViewModels;
using Registration.Views;

namespace Registration.Controllers
{
	public class RegistrationsController
	{
		private readonly IRegionManager _regionManager;

		public RegistrationsController(IRegionManager regionManager)
		{
			_regionManager = regionManager;

			this.RegisterPlayerCommand = new DelegateCommand(RegisterPlayer);
			this.RegisterTeamCommand = new DelegateCommand(RegisterTeam);
		}

		public ICommand RegisterPlayerCommand { get; private set; }
		public ICommand RegisterTeamCommand { get; private set; }

		public void RegisterPlayer()
		{
			IRegion mainRegion = _regionManager.Regions["MainRegion"];

			var viewModel = ServiceLocator.Current.GetInstance<IRegisterPlayerViewModel>();
			viewModel.CloseViewRequested += delegate
			{
				this.ShowRegistrationView();
			};
			mainRegion.Add(viewModel);
			
			mainRegion.Activate(viewModel);
		}

		//private void ShowRegisterPlayerView()
		//{
		//	IRegion region = _regionManager.Regions["MainRegion"];

		//	object view = region.GetView("RegisterPlayerView");
		//	if (view == null)
		//	{
		//		view = ServiceLocator.Current.GetInstance<RegisterPlayerView>();
		//		region.Add(view, "RegisterPlayerView");
		//	}

		//	region.Activate(view);
		//}

		//private void RemoveRegisterPlayerView()
		//{
		//	IRegion region = _regionManager.Regions["MainRegion"];

		//	object view = region.GetView("RegisterPlayerView");
		//	if (view != null)
		//	{
		//		region.Remove(view);
		//	}
		//}
	
		public void RegisterTeam()
		{
			System.Windows.MessageBox.Show("Register Team");
		}

		public void ShowRegistrationView()
		{
			IRegion region = _regionManager.Regions["MainRegion"];

			object view = region.GetView("RegistrationView");
			if (view == null)
			{
				view = ServiceLocator.Current.GetInstance<IRegistrationView>();
				region.Add(view, "RegistrationView");
			}

			region.Activate(view);
		}
	}
}
