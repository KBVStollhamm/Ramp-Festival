using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Infrastructure;
using Infrastructure.Messaging.Azure;
using Infrastructure.Messaging.Handling;
using Infrastructure.Messaging.Metadata;
using Infrastructure.Serialization;
using Registration.Application.Handlers;
using MassTransit;
using Infrastructure.EventSourcing;
using Infrastructure.EventSourcing.Sql;
using Registration.ReadModel.Implementation;
using Autofac.Core;

namespace Registration.Server
{
	public sealed class RegistrationProcessor : IDisposable
	{
		private IContainer _container;
		private bool _instrumentationEnabled;
		private CancellationTokenSource _cancellationTokenSource;
		private List<IProcessor> _processors;

		public RegistrationProcessor(bool instrumentationEnabled)
		{
			_instrumentationEnabled = instrumentationEnabled;

			this.OnCreating();
			_cancellationTokenSource = new CancellationTokenSource();
			_container = this.CreateContainer();

			_processors = _container.Resolve<IEnumerable<IProcessor>>().ToList();
		}

		#region IDisposable

		public void Dispose()
		{
			_container.Dispose();
			_cancellationTokenSource.Dispose();
		}

		#endregion

		private IContainer CreateContainer()
		{
			ContainerBuilder builder = new ContainerBuilder();

			this.BuildInfrastructure(builder);
			this.BuildHandlers(builder);

			var container = builder.Build();
			OnCreateContainer(container);

			return container;
		}

		private void BuildInfrastructure(ContainerBuilder builder)
		{
			builder.RegisterInstance<ITextSerializer>(new JsonTextSerializer());
			builder.RegisterInstance<IMetadataProvider>(new StandardMetadataProvider());

			builder.RegisterType<EventStoreDbContext>().WithParameter("nameOrConnectionString", "EventStore");
			builder.RegisterGeneric(typeof(SqlEventSourcedRepository<>))
				.As(typeof(IEventSourcedRepository<>))
				.InstancePerLifetimeScope();
				//.WithParameter(new ResolvedParameter(
				//	(pi, ctx) => pi.ParameterType == typeof(IServiceBus) && pi.Name == "configSectionName",
				//	(pi, ctx) => ctx.ResolveNamed("CommandBus", typeof(IServiceBus))));

			builder.RegisterType<ContestDbContext>().WithParameter("nameOrConnectionString", "Registration");
		}

		private void BuildHandlers(ContainerBuilder builder)
		{
			builder.RegisterType<SinglePlayerGameCommandHandler>().AsSelf();
            builder.RegisterType<TeamGameCommandHandler>().AsSelf();
            builder.RegisterType<SequencingReadModelGenerator>().AsSelf();
		}


		public void Start()
		{
			_processors.ForEach(p => p.Start());
		}

		public void Stop()
		{
			_cancellationTokenSource.Cancel();

			_processors.ForEach(p => p.Stop());
		}

		private void OnCreating()
		{
		}

		private void OnCreateContainer(IContainer container)
		{
			ContainerBuilder builder = new ContainerBuilder();

			// Create the command bus
			var commandBus = ServiceBusFactory.New(sbc =>
			{
				sbc.UseJsonSerializer();

				sbc.UseMsmq(msmq =>
				{
					msmq.UseMulticastSubscriptionClient();
					msmq.VerifyMsmqConfiguration();
				});
				sbc.ReceiveFrom("msmq://pc-mad/ramp-festival_registration");
				sbc.SetNetwork("WORKGROUP");

				// This will find all of the consumers in the container and
				// register them with the bus.
				sbc.Subscribe(s =>
				{
					s.LoadFrom(container);
				});
			});
			builder.Register(c => commandBus).As<IServiceBus>().SingleInstance();

			//// Create the event publisher
			//var eventPublisher = ServiceBusFactory.New(sbc =>
			//{
			//    sbc.UseJsonSerializer();

			//    sbc.UseMsmq(msmq =>
			//   {
			//         msmq.UseMulticastSubscriptionClient();
			//         msmq.VerifyMsmqConfiguration();
			//     });
			//    sbc.ReceiveFrom("msmq://localhost/ramp-festival_events");

			//    sbc.Subscribe(s =>
			//    {
			//        s.Handler<PlayerRegistered>((e) => Console.WriteLine("jo"));
			//    });
			//});
			//builder.Register(c => eventPublisher).As<IServiceBus>().Named<IServiceBus>("EventPublisher").SingleInstance();

			//// Create the event bus
			//var eventBus = ServiceBusFactory.New(sbc =>
			//{
			//    sbc.UseJsonSerializer();

			//    sbc.UseMsmq(msmq =>
			//    {
			//        msmq.UseMulticastSubscriptionClient();
			//        msmq.VerifyMsmqConfiguration();
			//    });
			//    sbc.ReceiveFrom("msmq://localhost/ramp-festival_events_registration");

			//    //this will find all of the consumers in the container and
			//    //register them with the bus.
			//    sbc.Subscribe(s =>
			//                {
			//                s.Handler<PlayerRegistered>((e) => Console.WriteLine("jo"));
			//                });
			//});
			//builder.Register(c => eventBus).As<IServiceBus>().Named<IServiceBus>("EventBus").SingleInstance();

			builder.Update(container);
		}
	}
}
