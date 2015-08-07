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
            await Task.Delay(5000);

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

        public async Task<IList<SequencingItem>> GetAllPendingGames()
        {
            await Task.Delay(1000);
            using (var context = _contextFactory.Invoke())
            {
                return await context.Query<SequencingItem>()
                    .OrderBy(x => x.RegisteredAt)
                    .ThenBy(x => x.Position)
                    .ToListAsync();
            }
        }
    }
}
