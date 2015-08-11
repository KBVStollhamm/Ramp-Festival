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
using Infrastructure.EventSourcing;
using Registration.Events;

namespace Registration.Application.Handlers
{
	public class SequencingReadModelGenerator
		: Consumes<SinglePlayerGamePlaced>.All
		, Consumes<TeamGamePlaced>.All
		, Consumes<SinglePlayerGameStarted>.All
		, Consumes<TeamGameStarted>.All
		, Consumes<GameFinished>.All
	{
		private readonly Func<RegistrationDbContext> _contextFactory;
		private readonly IServiceBus _eventBus;

		public SequencingReadModelGenerator(Func<RegistrationDbContext> contextFactory, IServiceBus eventBus)
		{
			_contextFactory = contextFactory;
			_eventBus = eventBus;
		}

		public void Consume(SinglePlayerGamePlaced message)
		{
			if (message == null) throw new ArgumentNullException("message");

			using (var context = _contextFactory.Invoke())
			{
				var dto = context.Find<Sequencing>(message.ContestId);
				if (dto == null)
				{
					dto = new Sequencing(message.ContestId);
				}

				var dtoItem = dto.Sequence.SingleOrDefault(x => x.GameId.Equals(message.SourceId));
				if (dtoItem == null)
				{
					dtoItem = new SequencingItem();
					dto.Sequence.Add(dtoItem);
				}
				dtoItem.GameId = message.SourceId;
				dtoItem.RegisteredAt = message.RegisteredAt;
				dtoItem.Position = 1;
				dtoItem.PlayerName = message.PlayerName;
				dtoItem.TeamName = string.Empty;
				dtoItem.Status = GameState.New;

				try
				{
					var contestDto = context.Find<Contest>(message.ContestId);
					switch (contestDto.ContestType)
					{
						case ContestType.SinglePlayerContest:
							dtoItem.GameType = Registration.ReadModel.GameType.SinglePlayerGame;
							break;
						case ContestType.TeamContest:
							throw new InvalidOperationException();
						case ContestType.ChildrenContest:
							dtoItem.GameType = Registration.ReadModel.GameType.ChildGame;
							break;
						case ContestType.WomenContest:
							dtoItem.GameType = Registration.ReadModel.GameType.WomenGame;
							break;
						default:
							break;
					}
				}
				catch (Exception)
				{ 
					Trace.TraceWarning("Ignoring GameType for game with ID {0} as contest with ID {1} wasn't found.",
						message.SourceId, message.ContestId);
				}

				context.Save(dto);
			}

			_eventBus.Publish(new SequencingChanged() { ContestId = message.ContestId });
		}

		public void Consume(TeamGamePlaced message)
		{
			if (message == null) throw new ArgumentNullException("message");

			using (var context = _contextFactory.Invoke())
			{
				var dto = context.Find<Sequencing>(message.ContestId);
				if (dto == null)
				{
					dto = new Sequencing(message.ContestId);
				}

				AddOrUpdate(dto, message.SourceId, message.RegisteredAt, 1, message.Player1Name, message.TeamName);
				AddOrUpdate(dto, message.SourceId, message.RegisteredAt, 2, message.Player2Name, message.TeamName);
				AddOrUpdate(dto, message.SourceId, message.RegisteredAt, 3, message.Player3Name, message.TeamName);
				AddOrUpdate(dto, message.SourceId, message.RegisteredAt, 4, message.Player4Name, message.TeamName);
				AddOrUpdate(dto, message.SourceId, message.RegisteredAt, 5, message.Player5Name, message.TeamName);

				context.Save(dto);
			}

			_eventBus.Publish(new SequencingChanged() { ContestId = message.ContestId });
		}

		private static void AddOrUpdate(Sequencing sequence, Guid gameId, DateTime registeredAt, int position, string playerName, string teamName)
		{
			var dtoItem = sequence.Sequence.SingleOrDefault(x => x.GameId.Equals(gameId) && x.Position == position);
			if (dtoItem == null)
			{
				dtoItem = new SequencingItem();
				sequence.Sequence.Add(dtoItem);
			}
			dtoItem.GameId = gameId;
			dtoItem.GameType = Registration.ReadModel.GameType.TeamGame;
			dtoItem.RegisteredAt = registeredAt;
			dtoItem.Position = position;
			dtoItem.PlayerName = playerName;
			dtoItem.TeamName = teamName;
			dtoItem.Status = GameState.New;
		}

		//		private static bool WasNotAlreadyHandled(Sequencing sequencing, int eventVersion)
		//		{
		//			// This assumes that events will be handled in order, but we might get the same message more than once.
		//			if (eventVersion > sequencing.Version)
		//			{
		//				return true;
		//			}
		//			else if (eventVersion == sequencing.Version)
		//			{
		//				Trace.TraceWarning(
		//					"Ignoring duplicate sequencing update message with version {1} for contest id {0}",
		//					sequencing.ContestId,
		//					eventVersion);
		//				return false;
		//			}
		//			else
		//			{
		//				Trace.TraceWarning(
		//					@"An older sequencing update message was received with version {1} for contest id {0}, last known version {2}.
		//This read model generator has an expectation that the EventBus will deliver messages for the same source in order.",
		//					sequencing.ContestId,
		//					eventVersion,
		//					sequencing.Version);
		//				return false;
		//			}
		//		}

		public void Consume(SinglePlayerGameStarted message)
		{
			using (var context = _contextFactory.Invoke())
			{
				var dto = context.Query<SequencingItem>().Where(e => e.GameId.Equals(message.SourceId)).SingleOrDefault();
				if (dto != null)
				{
					dto.Status = GameState.Running;
				}

				context.Save(dto);
			}

			_eventBus.Publish(new SequencingChanged() { ContestId = Guid.Empty });
		}

		public void Consume(TeamGameStarted message)
		{
			using (var context = _contextFactory.Invoke())
			{
				var dto = context.Query<SequencingItem>().Where(e => e.GameId.Equals(message.SourceId) && e.PlayerName.Equals(message.PlayerName)).SingleOrDefault();
				if (dto != null)
				{
					dto.Status = GameState.Running;
				}

				context.Save(dto);
			}

			_eventBus.Publish(new SequencingChanged() { ContestId = Guid.Empty });
		}

		public void Consume(GameFinished message)
		{
			using (var context = _contextFactory.Invoke())
			{
				var dto = context.Query<SequencingItem>().Where(e => e.GameId.Equals(message.SourceId) && e.PlayerName.Equals(message.PlayerName)).SingleOrDefault();
				if (dto != null)
				{
					dto.Status = GameState.Finished;
				}

				context.Save(dto);
			}
		}
	}
}
