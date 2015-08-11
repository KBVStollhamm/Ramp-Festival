using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registration.ReadModel
{
	public interface IContestDao
	{
		Task<Contest> FindContest(Guid contestId);
		Task<IList<Contest>> GetPublishedContests();

		Task<Sequencing> FindSequencing(Guid contestId);
		Task<SequencingItem> FindNextGame(Guid stationId);
		Task<IList<SequencingItem>> GetAllPendingGames();
		Task<IList<SequencingItem>> GetAllNewGames();

		Task<GamingSummary> FindGamingSummary(Guid stationId);

		Task<GameResult> FindGameResult(Guid gameId);

		Task<Leaderboard> GetLeaderboard();
	}
}
