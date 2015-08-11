using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registration.Domain
{
	/// <summary>
	/// Value object representing a player's shot.
	/// </summary>
	public class Shot
	{
		public Shot(string playerName, int shotNumber, int score)
		{
			this.PlayerName = playerName;
            this.ShotNumber = shotNumber;
            this.Score = score;
		}

		public string PlayerName { get; private set; }
        public int ShotNumber { get; private set; }
        public int Score { get; private set; }
		//public ReadOnlyDictionary<int, int> Shots { get; private set; }

		//public Scores AddOrUpdate(int shotNumber, int points)
		//{
		//	Dictionary<int, int> shots = new Dictionary<int,int>();
		//	foreach (var shot in this.Shots)
		//		shots.Add(shotNumber, points);

		//	shots[shotNumber] = points;

		//	return new Scores(this.PlayerName, shots);
		//}
	}
}
