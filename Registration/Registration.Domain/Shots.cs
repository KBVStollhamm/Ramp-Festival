using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registration.Domain
{
	/// <summary>
	/// Value object representing all shots of a player.
	/// </summary>
	public class Scores
	{
		public Scores(string playerName, Dictionary<int, int> shots)
		{
			this.PlayerName = playerName;
			this.Shots = new ReadOnlyDictionary<int, int>(shots);
		}

		public string PlayerName { get; private set; }
		public ReadOnlyDictionary<int, int> Shots { get; private set; }

		public Scores AddOrUpdate(int shotNumber, int points)
		{
			Dictionary<int, int> shots = new Dictionary<int,int>();
			foreach (var shot in this.Shots)
				shots.Add(shotNumber, points);

			shots[shotNumber] = points;

			return new Scores(this.PlayerName, shots);
		}
	}
}
