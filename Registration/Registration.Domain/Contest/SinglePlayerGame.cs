using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.EventSourcing;

namespace Registration.Domain.Contest
{
	public class SinglePlayerGame : EventSourced
	{
		private bool _isRunning;
		private Dictionary<int, int> _shots = new Dictionary<int, int>();

		protected SinglePlayerGame(Guid id)
			: base(id)
		{
			this.Handles<SinglePlayerGamePlaced>(When);
			this.Handles<SinglePlayerGameStarted>(When);
			this.Handles<PlayerScored>(When);
			this.Handles<PlayerScoreUpdated>(When);
		}

		public SinglePlayerGame(Guid id, IEnumerable<IVersionedEvent> history)
			: this(id)
		{
			this.LoadFrom(history);
		}

		public SinglePlayerGame(Guid id, Guid contestId, string playerName) : this(id)
		{
			this.Apply(new SinglePlayerGamePlaced
			{
				ContestId = contestId,
				RegisteredAt = DateTime.Now,
				PlayerName = playerName
			});
		}

		public void Start()
		{
			if (_isRunning) return; // Game already started before

			this.Apply(new SinglePlayerGameStarted());
		}

		public void MakeShot(string playerName, int shotNumber, int score)
		{
			//if (!_isRunning) throw new InvalidOperationException("Game is not running, player mustn't make shots!");

			if (!_shots.ContainsKey(shotNumber))
			{
				this.Apply(new PlayerScored
				{
					PlayerName = playerName,
					ShotNumber = shotNumber,
					Points = score
				});
			}
			else
			{
				this.Apply(new PlayerScoreUpdated
				{
					ShotNumber = shotNumber,
					Points = score
				});
			}
		}

		private void When(SinglePlayerGamePlaced e)
		{
		}

		private void When(SinglePlayerGameStarted e)
		{
			_isRunning = true;
		}

		private void When(PlayerScored e)
		{
			_shots.Add(e.ShotNumber, e.Points);
		}

		private void When(PlayerScoreUpdated e)
		{
			_shots[e.ShotNumber] = e.Points;
		}
	}
}
