using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.EventSourcing;

namespace Registration.Domain.Contest
{
	public class TeamGame : EventSourced
	{
		private bool _isRunning;
		private Dictionary<string, Scores> _scores = new Dictionary<string, Scores>();

		protected TeamGame(Guid id)
			: base(id)
		{
			this.Handles<TeamGamePlaced>(When);
		}

		public TeamGame(Guid id, IEnumerable<IVersionedEvent> history)
			: this(id)
		{
			this.LoadFrom(history);
		}

		public TeamGame(Guid id, Guid contestId, string teamName, string player1Name, string player2Name, string player3Name, string player4Name, string player5Name):this(id)
		{
			this.Apply(new TeamGamePlaced
			{
				ContestId = contestId,
				RegisteredAt = DateTime.Now,
				TeamName = teamName,
				Player1Name = player1Name,
				Player2Name = player2Name,
				Player3Name = player3Name,
				Player4Name = player4Name,
				Player5Name = player5Name
			});
		}

		public void Start()
		{
			if (_isRunning) return; // Game already started before

			this.Apply(new TeamGameStarted());
		}

		public void MakeShot(string playerName, int shotNumber, int score)
		{
			//if (!_isRunning) throw new InvalidOperationException("Game is not running, player mustn't make shots!");

			if (!_scores.ContainsKey(playerName))
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
					PlayerName = playerName,
					ShotNumber = shotNumber,
					Points = score
				});
			}
		}

		private void When(TeamGamePlaced e)
		{
		}
		
		private void When(TeamGameStarted e)
		{
			_isRunning = true;
		}

		private void When(PlayerScored e)
		{
			Scores existing;
			if (!_scores.TryGetValue(e.PlayerName, out existing))
			{
				existing = new Scores(e.PlayerName, new Dictionary<int, int>());
			}

			existing.AddOrUpdate(e.ShotNumber, e.Points);
		}

		private void When(PlayerScoreUpdated e)
		{
			Scores existing;
			if (!_scores.TryGetValue(e.PlayerName, out existing))
			{
				existing = new Scores(e.PlayerName, new Dictionary<int, int>());
			}

			existing.AddOrUpdate(e.ShotNumber, e.Points);
		}
	}
}
