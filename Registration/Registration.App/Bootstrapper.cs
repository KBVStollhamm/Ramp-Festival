using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Castle.MicroKernel.Registration;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using PrismContrib.WindsorExtensions;
using Registration.ViewModels;
using Registration.Views;

namespace Registration
{
	public class Bootstrapper : WindsorBootstrapper
	{
		protected override void ConfigureModuleCatalog()
		{
			base.ConfigureModuleCatalog();

			ModuleCatalog moduleCatalog = (ModuleCatalog)this.ModuleCatalog;
			moduleCatalog.AddModule(typeof(RegistrationModule));

		}
		protected override DependencyObject CreateShell()
		{
			return this.Container.Resolve<Shell>();
		}

		protected override void ConfigureContainer()
		{
			this.Container.Register(Component.For<RegistrationModule>()
				.LifestyleSingleton());

			this.Container.Register(Component.For<Shell>()
				.LifestyleSingleton());

		   base.ConfigureContainer();
		}

		protected override void InitializeShell()
		{
			base.InitializeShell();

			Application.Current.MainWindow = (Shell)this.Shell;
			Application.Current.MainWindow.Show();
		}
	}
}
