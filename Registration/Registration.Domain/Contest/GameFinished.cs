using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.EventSourcing;

namespace Registration.Domain.Contest
{
	public class GameFinished : VersionedEvent
	{
		public string PlayerName { get; set; }
		public int TotalScore { get; set; }
		public string TeamName { get; set; }
	}
}
