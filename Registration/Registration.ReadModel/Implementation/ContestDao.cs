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
		private readonly Func<ContestDbContext> _contextFactory;

		public ContestDao(Func<ContestDbContext> contextFactory)
		{
			_contextFactory = contextFactory;
		}

		public async Task<IList<ContestAlias>> GetPublishedContests()
		{
			using (var context = _contextFactory.Invoke())
			{
				return await context
					.Query<Contest>()
					.Where(dto => dto.IsPublished)
					.Select(x => new ContestAlias { Id = x.Id, Code = x.Code, Name = x.Name })
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
	}
}
