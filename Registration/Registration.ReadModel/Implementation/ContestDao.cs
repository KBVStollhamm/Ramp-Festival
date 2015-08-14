using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registration.ReadModel.Implementation
{
	public class ContestDao : IContestDao
	{
		private readonly Func<RegistrationDbContext> _contextFactory;

		public ContestDao(Func<RegistrationDbContext> contextFactory)
		{
			_contextFactory = contextFactory;
		}

		public async Task<Contest> FindContest(Guid contestId)
		{
			using (var context = _contextFactory.Invoke())
			{
				return await context.FindAsync<Contest>(contestId);
			}
		}

		public async Task<IList<Contest>> GetPublishedContests()
		{
			using (var context = _contextFactory.Invoke())
			{
				return await context
					.Query<Contest>()
					.Where(dto => dto.IsPublished)
					.ToListAsync();
			}
		}

		public async Task<Sequencing> FindSequencing(Guid contestId)
		{
			using (var context = _contextFactory.Invoke())
			{
				return await context.Sequencing
					.Include(seq => seq.Sequence)
					.FirstOrDefaultAsync(seq => seq.ContestId.Equals(contestId));
			}
		}

		public async Task<SequencingItem> FindSequencingItem(Guid gameId)
		{
			using (var context = _contextFactory.Invoke())
			{
				return await context.Query<SequencingItem>()
					.FirstOrDefaultAsync(seq => seq.GameId.Equals(gameId));
			}
		}

		public async Task<SequencingItem> FindNextGame(Guid stationId)
		{
			using (var context = _contextFactory.Invoke())
			{
				return await context.Query<SequencingItem>().OrderBy(x => x.RegisteredAt).ThenBy(x => x.Position).FirstOrDefaultAsync();
			}
		}
		
		public async Task<IList<SequencingItem>> GetAllPendingGames()
		{
			using (var context = _contextFactory.Invoke())
			{
				return await context.Query<SequencingItem>()
					.Where(x => x.Status == GameState.New || x.Status == GameState.Running)
					.OrderBy(x => x.RegisteredAt)
					.ThenBy(x => x.Position)
					.ToListAsync();
			}
		}

		public async Task<IList<SequencingItem>> GetAllNewGames()
		{
			using (var context = _contextFactory.Invoke())
			{
				return await context.Query<SequencingItem>()
					.Where(x => x.Status == GameState.New)
					.OrderBy(x => x.RegisteredAt)
					.ThenBy(x => x.Position)
					.ToListAsync();
			}
		}

		public async Task<GamingSummary> FindGamingSummary(Guid stationId)
		{
			using (var context = _contextFactory.Invoke())
			{
				return await context.FindAsync<GamingSummary>(stationId);
			}
		}

		public async Task<GameResult> FindGameResult(Guid gameId)
		{
			using (var context = _contextFactory.Invoke())
			{
				return await context.Query<GameResult>().Include(x => x.Scores).Where(x => x.GameId.Equals(gameId)).FirstOrDefaultAsync();
			}
		}

		public async Task<Leaderboard> GetLeaderboard()
		{
			Leaderboard result = new Leaderboard();
			result.Id = Guid.Empty;
			result.Date = DateTime.Today;

			using (var context = _contextFactory.Invoke())
			{
				var singleLeader = await context.Query<GameResult>().Where(x => x.GameType == GameType.SinglePlayerGame).OrderByDescending(x => x.TotalScore).FirstOrDefaultAsync();
				var teamLeader = await context.Query<GameResult>().Where(x => x.GameType == GameType.TeamGame).OrderByDescending(x => x.TotalScore).FirstOrDefaultAsync();
				var childLeader = await context.Query<GameResult>().Where(x => x.GameType == GameType.ChildGame).OrderByDescending(x => x.TotalScore).FirstOrDefaultAsync();
				var womenLeader = await context.Query<GameResult>().Where(x => x.GameType == GameType.WomenGame).OrderByDescending(x => x.TotalScore).FirstOrDefaultAsync();

				result.SinglePlayerContestLeader = FromGameResult(singleLeader);
				result.TeamContestLeader = FromGameResult(teamLeader);
				result.ChildrenContestLeader = FromGameResult(childLeader);
				result.WomenContestLeader = FromGameResult(womenLeader);
			}

			return result;			
		}

		private LeaderboardItem FromGameResult(GameResult gameResult)
		{
			if (gameResult == null) return null;

			return new LeaderboardItem() { GameId = gameResult.GameId, TotalScore = gameResult.TotalScore, LeaderName = gameResult.Name, GameType = gameResult.GameType };

		}
	}
}
