using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Registration.ViewModels;
using Registration.Views;

namespace Registration
{
	public class RegistrationModule : IModule
	{
		private readonly IWindsorContainer _container;
		private readonly IRegionManager _regionManager;

		public RegistrationModule(IWindsorContainer container, IRegionManager regionManager)
		{
			_container = container;
			_regionManager = regionManager;
		}

		public void Initialize()
		{
			_container.Register(Component.For<RegistrationViewModel>());
			_container.Register(Component.For<RegistrationView>());

			_regionManager.RegisterViewWithRegion("MainRegion", typeof(RegistrationView));
		}
	}
}
