using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Registration.Controllers;
using Registration.ViewModels;
using Registration.Views;
using Registration.Services;
using MassTransit;
using Registration.ReadModel;
using Registration.ReadModel.Implementation;
using System.Data.Entity;
using System.Threading;
using Microsoft.Practices.Prism.PubSubEvents;
using Registration.Events;

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
            _container.Register(Component.For<IRegistrationService>()
                .ImplementedBy<RegistrationService>()
                .DependsOn(Dependency.OnComponent(typeof(IServiceBus), "CommandBus")));
            _container.Register(Component.For<IGameControlService>()
                .ImplementedBy<GameControlService>()
                .DependsOn(Dependency.OnComponent(typeof(IServiceBus), "CommandBus")));

            _container.Register(Component.For<RegistrationsController>());
            _container.Register(Component.For<GameController>());
            _container.Register(Component.For<LiveController>());

            _container.Register(Component.For<HomeViewModel>());
            _container.Register(Component.For<HomeView>());

            //_container.Register(Component.For<RegistrationViewModel>());
            //_container.Register(Component.For<IRegistrationView>()
            //	.ImplementedBy<RegistrationView>());
            _container.Register(Component.For<ContestRegistrationViewModel>());
            _container.Register(Component.For<IRegistrationView>()
                .ImplementedBy<ContestRegistrationView>());

            _container.Register(Component.For<IRegisterPlayerViewModel>()
                .ImplementedBy<RegisterPlayerViewModel>()
                .LifestyleTransient());

            _container.Register(Component.For<IRegisterTeamViewModel>()
                .ImplementedBy<RegisterTeamViewModel>()
                .LifestyleTransient());

            _container.Register(Component.For<GameControlFrameView>()
                .LifestyleSingleton());
            _container.Register(Component.For<GamingSummaryViewModel>()
                .LifestyleSingleton());
            _container.Register(Component.For<IGamingSummaryView>()
                .ImplementedBy<GamingSummaryView>()
                .LifestyleSingleton());
            _container.Register(Component.For<GameSelectionViewModel>()
                .LifestyleTransient());
            _container.Register(Component.For<GameSelectionView>()
                .LifestyleTransient());

            _container.Register(Component.For<ILeaderboardView>()
                .ImplementedBy<LeaderboardView>()
                .LifestyleSingleton());
            _container.Register(Component.For<LiveViewModel>()
                .LifestyleSingleton());
            _container.Register(Component.For<ILiveView>()
                .ImplementedBy<LiveView>()
                .LifestyleSingleton());

            _container.Register(Component.For<SequencingViewModel>());
            _container.Register(Component.For<SequencingView>());

            _container.Register(Component.For<RegistrationDbContext>()
                .DependsOn(Dependency.OnValue("nameOrConnectionString", "Registration"))
                .LifestyleTransient());
            _container.Register(Component.For<IContestDao>()
                .ImplementedBy<ContestDao>()
                .LifestyleTransient());

            //_container.Resolve<RegistrationsController>().ShowRegistrationView();
            _regionManager.RegisterViewWithRegion("MainRegion", typeof(HomeView));
            _regionManager.RegisterViewWithRegion("MainRegion", typeof(GameControlFrameView));
            _regionManager.RegisterViewWithRegion("DetailsRegion", typeof(SequencingView));

            //var eventAggragator = _container.Resolve<IEventAggregator>();
            //eventAggragator.GetEvent<ModuleInitialized>().Publish(new ModuleInitialized());
            //_container.Release(eventAggragator);

        }

		private void InitializeDatabase(Database db)
		{
			db.Initialize(true);
		}
	}
}
