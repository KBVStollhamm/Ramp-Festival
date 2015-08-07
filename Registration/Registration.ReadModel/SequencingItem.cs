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

        public Guid GameId { get; set; }
        public GameType GameType { get; set; }
        public DateTime RegisteredAt { get; set; }
		public int Position {get ;set;}
		public string PlayerName { get; set; }
		public string TeamName { get; set; }

	}

    public enum GameType
    {
        SinglePlayerGame = 1,
        TeamGame = 2,
        ChildGame = 3
    }
}
