using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registration.ReadModel
{
	public interface IContestDao
	{
		Task<IList<ContestAlias>> GetPublishedContests();
		Task<Sequencing> FindSequencing(Guid contestId);
	}
}
