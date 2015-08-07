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
		private readonly Func<ContestDbContext> _contextFactory;
		private readonly IServiceBus _eventBus;

		public ContestReadModelGenerator(Func<ContestDbContext> contextFactory, IServiceBus eventBus)
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
}
