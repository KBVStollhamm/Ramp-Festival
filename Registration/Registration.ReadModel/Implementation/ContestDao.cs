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
    }
}
