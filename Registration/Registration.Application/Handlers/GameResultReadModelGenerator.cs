using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Registration.Domain.Contest;
using Registration.ReadModel;
using Registration.ReadModel.Implementation;

namespace Registration.Application.Handlers
{
	public class GameResultReadModelGenerator
		: Consumes<SinglePlayerGamePlaced>.All
		, Consumes<TeamGamePlaced>.All
		, Consumes<PlayerScored>.All
		, Consumes<PlayerScoreUpdated>.All
	{
		private readonly Func<RegistrationDbContext> _contextFactory;
		private readonly IServiceBus _eventBus;

		public GameResultReadModelGenerator(Func<RegistrationDbContext> contextFactory, IServiceBus eventBus)
		{
			_contextFactory = contextFactory;
			_eventBus = eventBus;
		}

		public void Consume(SinglePlayerGamePlaced message)
		{
			if (message == null) throw new ArgumentNullException("message");

			using (var context = _contextFactory.Invoke())
			{
				GameType gameType = GameType.SinglePlayerGame;
				var contest = context.Find<Contest>(message.ContestId);
				if (contest != null)
				{
					switch (contest.ContestType)
					{
						case ContestType.SinglePlayerContest:
							gameType = GameType.SinglePlayerGame;
							break;
						case ContestType.TeamContest:
							gameType = GameType.TeamGame;
							break;
						case ContestType.ChildrenContest:
							gameType = GameType.ChildGame;
							break;
						case ContestType.WomenContest:
							gameType = GameType.WomenGame;
							break;
					}
				}

				var dto = context.Find<GameResult>(message.SourceId);
				if (dto == null)
				{
					dto = new GameResult
					{
						GameId = message.SourceId,
						GameType = gameType,
						Name = message.PlayerName
					};
				}

				context.Save(dto);
			}
		}



		public void Consume(TeamGamePlaced message)
		{
			if (message == null) throw new ArgumentNullException("message");

			using (var context = _contextFactory.Invoke())
			{
				var dto = context.Find<GameResult>(message.SourceId);
				if (dto == null)
				{
					dto = new GameResult
					{
						GameId = message.SourceId,
						GameType = GameType.TeamGame,
						Name = message.TeamName
					};
				}


				context.Save(dto);
			}
		}

		public void Consume(PlayerScored message)
		{
			if (message == null) throw new ArgumentNullException("message");

			using (var context = _contextFactory.Invoke())
			{
				var dto = context.Query<GameResult>().Include("Scores").FirstOrDefault(x => x.GameId == message.SourceId);

				if (dto == null)
				{
					dto = new GameResult
					{
						GameId = message.SourceId
					};
				}

				var score = dto.Scores.FirstOrDefault(x => x.ShotNumber == message.ShotNumber && x.PlayerName.Equals(message.PlayerName));
				if (score != null)
				{
					score.Points = message.Points;
				}
				else
				{
					dto.Scores.Add(new Shot
					{
						PlayerName = message.PlayerName,
						ShotNumber = message.ShotNumber,
						Points = message.Points
					});
				}

				dto.TotalScore = dto.Scores.Sum(x => x.Points);

				context.Save(dto);
			}
		}

		public void Consume(PlayerScoreUpdated message)
		{
			if (message == null) throw new ArgumentNullException("message");

			using (var context = _contextFactory.Invoke())
			{
				var dto = context.Query<GameResult>().Include("Scores").FirstOrDefault(x => x.GameId == message.SourceId);

				if (dto == null)
				{
					dto = new GameResult
					{
						GameId = message.SourceId
					};
				}

				var score = dto.Scores.FirstOrDefault(x => x.ShotNumber == message.ShotNumber && x.PlayerName.Equals(message.PlayerName));
				if (score != null)
				{
					score.Points = message.Points;
				}
				else
				{
					dto.Scores.Add(new Shot
					{
						PlayerName = message.PlayerName,
						ShotNumber = message.ShotNumber,
						Points = message.Points
					});
				}

				dto.TotalScore = dto.Scores.Sum(x => x.Points);

				context.Save(dto);
			}
		}

	}
}
