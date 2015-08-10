using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContestManagement.Events;
using MassTransit;
using Registration.ReadModel;
using Registration.ReadModel.Implementation;

namespace Registration.Application.Handlers
{
	public class ContestReadModelGenerator
		: Consumes<ContestCreated>.All
	{
		private readonly Func<RegistrationDbContext> _contextFactory;
		private readonly IServiceBus _eventBus;

		public ContestReadModelGenerator(Func<RegistrationDbContext> contextFactory, IServiceBus eventBus)
		{
			_contextFactory = contextFactory;
			_eventBus = eventBus;
		}

		public void Consume(ContestCreated message)
		{
			if (message == null) throw new ArgumentNullException("message");

			Console.WriteLine(message.Name);

			using (var repository = _contextFactory.Invoke())
			{
				var dto = repository.Find<Contest>(message.SourceId);
				if (dto != null)
				{
					Trace.TraceWarning(
						"Ignoring ConferenceCreated event for conference with ID {0} as it was already created.",
						message.SourceId);
				}
				else
				{
					repository.Set<Contest>().Add(
						new Contest(
							message.SourceId,
							message.ContestType.ToReadModelType(),
							message.Slug,
							message.Name,
							message.Description,
							message.Location,
							message.StartDate,
							message.EndDate));

					repository.SaveChanges();
				}
			}
		}
	}

	public static class MappingExtensions
	{
		internal static Registration.ReadModel.ContestType ToReadModelType(this ContestManagement.Events.ContestType eventType)
		{
			switch (eventType)
			{
				case ContestManagement.Events.ContestType.SinglePlayerContest:
					return ReadModel.ContestType.SinglePlayerContest;
				case ContestManagement.Events.ContestType.TeamContest:
					return ReadModel.ContestType.TeamContest;
				case ContestManagement.Events.ContestType.ChildrenContest:
					return ReadModel.ContestType.ChildrenContest;
				case ContestManagement.Events.ContestType.WomenContest:
					return ReadModel.ContestType.WomenContest;
				default:
					return ReadModel.ContestType.SinglePlayerContest;
					//throw new InvalidCastException();
			}
		}
	}
}
