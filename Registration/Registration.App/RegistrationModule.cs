﻿using System;
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

			_container.Register(Component.For<RegistrationsController>());

			_container.Register(Component.For<HomeViewModel>());
			_container.Register(Component.For<HomeView>());
			
			_container.Register(Component.For<RegistrationViewModel>());
			_container.Register(Component.For<IRegistrationView>()
				.ImplementedBy<RegistrationView>());

			_container.Register(Component.For<IRegisterPlayerViewModel>()
				.ImplementedBy<RegisterPlayerViewModel>()
				.LifestyleTransient());

			_container.Register(Component.For<IRegisterTeamViewModel>()
				.ImplementedBy<RegisterTeamViewModel>()
				.LifestyleTransient());

			_container.Register(Component.For<SequencingViewModel>());
			_container.Register(Component.For<SequencingView>());

			_container.Register(Component.For<ContestDbContext>()
				.DependsOn(Dependency.OnValue("nameOrConnectionString", "Registration"))
				.LifestyleTransient());
			_container.Register(Component.For<IContestDao>()
				.ImplementedBy<ContestDao>()
				.LifestyleTransient());

			//_container.Resolve<RegistrationsController>().ShowRegistrationView();
			_regionManager.RegisterViewWithRegion("MainRegion", typeof(HomeView));

			Task.Factory.StartNew(() =>
			{
				var ctx = _container.Resolve<ContestDbContext>();
				ctx.Database.Initialize(true);
				_container.Release(ctx);
			})
			.ContinueWith((r) =>
			{
				_regionManager.RegisterViewWithRegion("DetailsRegion", typeof(SequencingView));
			}, TaskScheduler.FromCurrentSynchronizationContext())
			.ContinueWith(async r =>
			{
				var eventAggragator = _container.Resolve<IEventAggregator>();
				eventAggragator.GetEvent<ModuleInitialized>().Publish(new ModuleInitialized());
				_container.Release(eventAggragator);

				await Task.Delay(100);
			});

		}

		private void InitializeDatabase(Database db)
		{
			db.Initialize(true);
		}
	}
}
