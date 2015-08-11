using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using MassTransit;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using PrismContrib.WindsorExtensions;
using Registration.ViewModels;
using Registration.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
			return this.Container.Resolve<Shell3>();
		}

		protected override void ConfigureContainer()
		{
			this.Container.AddFacility<TypedFactoryFacility>();

			this.Container.Register(Component.For<RegistrationModule>()
				.LifestyleSingleton());

			this.Container.Register(Component.For<ShellViewModel>()
				.LifestyleSingleton());
			this.Container.Register(Component.For<Shell3>()
				.LifestyleSingleton());

			this.RegisterCommandBus("Registration", "RegistrationCommandBus");
            this.RegisterCommandBus("GameControl", "GameControlCommandBus");

			base.ConfigureContainer();
		}

		protected override void InitializeShell()
		{
			base.InitializeShell();

			Application.Current.MainWindow = (Shell3)this.Shell;
			Application.Current.MainWindow.Show();
		}

		protected override void InitializeModules()
		{

			base.InitializeModules();

			//EventAggregator.GetEvent<MessageUpdateEvent>().Publish(new MessageUpdateEvent { Message = "Module1" });

			//IModule splashModule = Container.Resolve<RegistrationModule>();
			//splashModule.Initialize();

		}

		private void RegisterCommandBus(string suffix, string name)
		{
			var commandBus = ServiceBusFactory.New(sbc =>
			{
				sbc.UseMsmq(msmq =>
				{
					msmq.UseMulticastSubscriptionClient();
					msmq.VerifyMsmqConfiguration();
				});
				sbc.ReceiveFrom("msmq://localhost/ramp-festival_commands" + suffix);
				sbc.SetNetwork("WORKGROUP");
			});

			this.Container.Register(Component.For<IServiceBus>()
				.Instance(commandBus)
				.Named(name));
		}
	}
}
