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

		public Sequencing FindSequencing(Guid contestId)
		{
			using (var context = _contextFactory.Invoke())
			{
				return context.Sequencing
					.Include(seq => seq.Sequence)
					.FirstOrDefault(seq => seq.ContestId.Equals(contestId));
			}
		}
	}
}
