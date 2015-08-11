using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registration.ReadModel
{
	public class Leaderboard
	{
		public Guid Id { get; set; }

		public DateTime Date { get; set; }

		public LeaderboardItem SinglePlayerContestLeader { get; set; }
		public LeaderboardItem TeamContestLeader { get; set; }
		public LeaderboardItem ChildrenContestLeader { get; set; }
		public LeaderboardItem WomenContestLeader { get; set; }
	}

	public class LeaderboardItem
	{
		public Guid Id { get; set; }
		public Guid GameId { get; set; }
		public string LeaderName { get; set; }
		public int TotalScore { get; set; }
	}
}
