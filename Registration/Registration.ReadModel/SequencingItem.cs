using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registration.ReadModel
{
	public class SequencingItem
	{
		public SequencingItem()
		{
		}

		public Guid ContestId { get; set; }
		public Guid GameId { get; set; }
		public GameType GameType { get; set; }
		public DateTime RegisteredAt { get; set; }
		public int Position {get ;set;}
		public string PlayerName { get; set; }
		public string TeamName { get; set; }
		public GameState Status { get; set; }
	}

	public enum GameType
	{
		SinglePlayerGame = 1,
		TeamGame = 2,
		ChildGame = 3,
		WomenGame = 4
	}

	public enum GameState
	{
		New = 1,
		Running = 2,
		Finished = 3,
		Aborted = 4
	}
}
