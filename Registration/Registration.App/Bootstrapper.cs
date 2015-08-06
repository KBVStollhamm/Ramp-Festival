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
			return this.Container.Resolve<Shell>();
		}

		protected override void ConfigureContainer()
		{
			this.Container.AddFacility<TypedFactoryFacility>();

			this.Container.Register(Component.For<RegistrationModule>()
				.LifestyleSingleton());

			this.Container.Register(Component.For<ShellViewModel>()
				.LifestyleSingleton());
			this.Container.Register(Component.For<Shell>()
				.LifestyleSingleton());

			this.RegisterCommandBus();

			base.ConfigureContainer();
		}

		protected override void InitializeShell()
		{
			base.InitializeShell();

			Application.Current.MainWindow = (Shell)this.Shell;
			Application.Current.MainWindow.Show();
		}

		protected override void InitializeModules()
		{

			base.InitializeModules();

			//EventAggregator.GetEvent<MessageUpdateEvent>().Publish(new MessageUpdateEvent { Message = "Module1" });

			//IModule splashModule = Container.Resolve<RegistrationModule>();
			//splashModule.Initialize();

		}

		private void RegisterCommandBus()
		{
			var commandBus = ServiceBusFactory.New(sbc =>
			{
				sbc.UseMsmq(msmq =>
				{
					msmq.UseMulticastSubscriptionClient();
					msmq.VerifyMsmqConfiguration();
				});
				sbc.ReceiveFrom("msmq://pc-mad/ramp-festival");
				sbc.SetNetwork("WORKGROUP");
			});

			this.Container.Register(Component.For<IServiceBus>()
				.Instance(commandBus)
				.Named("CommandBus"));
		}
	}
}
