using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Registration.Domain.Contest;
using Registration.ReadModel;
using Registration.ReadModel.Implementation;

namespace Registration.Application.Handlers
{
	public class SequencingReadModelGenerator
		: Consumes<ContestPlaced>.All
		, Consumes<PlayerRegistered>.All
	{
		private readonly Func<ContestDbContext> _contextFactory;

		public SequencingReadModelGenerator(Func<ContestDbContext> contextFactory)
		{
			_contextFactory = contextFactory;
		}

		public void Consume(ContestPlaced message)
		{
			if (message == null) throw new ArgumentNullException("message");

			using (var context = _contextFactory.Invoke())
			{
				var dto = new Sequencing(message.SourceId)
				{					
				};

				context.Save(dto);
			}
		}

		public void Consume(PlayerRegistered message)
		{
			if (message == null) throw new ArgumentNullException("message");

			using (var context = _contextFactory.Invoke())
			{
				var dto = context.Find<Sequencing>(message.SourceId);
				if (WasNotAlreadyHandled(dto, message.Version))
				{
					dto.Version = message.Version;
					dto.Sequence.Add(new SequencingItem(1)
						{
							PlayerName = message.PlayerName,
							TeamName = ""
						});

					context.Save(dto);
				}
			}
		}

		private static bool WasNotAlreadyHandled(Sequencing sequencing, int eventVersion)
		{
			// This assumes that events will be handled in order, but we might get the same message more than once.
			if (eventVersion > sequencing.Version)
			{
				return true;
			}
			else if (eventVersion == sequencing.Version)
			{
				Trace.TraceWarning(
					"Ignoring duplicate sequencing update message with version {1} for contest id {0}",
					sequencing.ContestId,
					eventVersion);
				return false;
			}
			else
			{
				Trace.TraceWarning(
					@"An older sequencing update message was received with version {1} for contest id {0}, last known version {2}.
This read model generator has an expectation that the EventBus will deliver messages for the same source in order.",
					sequencing.ContestId,
					eventVersion,
					sequencing.Version);
				return false;
			}
		}
	}
}
