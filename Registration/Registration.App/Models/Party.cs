using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registration.Models
{
	public class Party
	{
		public Party(PlayerName playerName, TeamName teamName)
		{
			this.PlayerName = playerName;
			this.TeamName = teamName;
		}

		public PlayerName PlayerName { get; private set; }
		public TeamName TeamName { get; private set; }
	}
}
