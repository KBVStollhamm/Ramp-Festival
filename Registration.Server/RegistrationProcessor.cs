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
using Registration.Domain.Contest;
using Registration.ReadModel.Implementation;

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

			var handler = container.Resolve<SequencingReadModelGenerator>();
			Guid sourceId = Guid.NewGuid();
			handler.Consume(new ContestPlaced { SourceId = sourceId });
			handler.Consume(new PlayerRegistered { SourceId = sourceId, PlayerName = "MAD", Version=1 });
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

			builder.RegisterType<ContestDbContext>().WithParameter("nameOrConnectionString", "Registration");
		}

		private void BuildHandlers(ContainerBuilder builder)
		{
			builder.RegisterType<ContestCommandHandler>().AsSelf();
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

			//now we add the bus
			var commandBus = ServiceBusFactory.New(sbc =>
			{
				sbc.UseMsmq(msmq =>
				{
					msmq.UseMulticastSubscriptionClient();
					msmq.VerifyMsmqConfiguration();
				});
				sbc.ReceiveFrom("msmq://pc-mad/ramp-festival_commands_registration");

				//this will find all of the consumers in the container and
				//register them with the bus.
				sbc.Subscribe(s => {
					s.LoadFrom(container);
					//s.Consumer<ContestCommandHandler>()
					//.Permanent();
				});
			});
			builder.Register(c => commandBus).As<IServiceBus>().SingleInstance();

			//builder.RegisterType<TestProcessor>().As<IProcessor>().SingleInstance();

			//var serializer = container.Resolve<ITextSerializer>();

			//var sessionlessCommandProcessor = new CommandProcessor(new SubscriptionReceiver(), serializer);
			//builder.RegisterInstance(sessionlessCommandProcessor).Named<IProcessor>("SessionlessCommandProcessor");

			builder.Update(container);

			//this.RegisterCommandHandlers(container, sessionlessCommandProcessor);
		}

		//private void RegisterCommandHandlers(IContainer container, ICommandHandlerRegistry registry)
		//{
		//	foreach (var commandHandler in container.Resolve<IEnumerable<ICommandHandler>>())
		//	{
		//		registry.Register(commandHandler);
		//	}
		//}
	}
}
