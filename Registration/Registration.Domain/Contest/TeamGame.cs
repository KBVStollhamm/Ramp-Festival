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
		private Dictionary<string, bool> _isRunning = new Dictionary<string, bool>();
		private List<Shot> _shots = new List<Shot>();
		private string _teamName;

		protected TeamGame(Guid id)
			: base(id)
		{
			this.Handles<TeamGamePlaced>(When);
			this.Handles<TeamGameStarted>(When);
			this.Handles<PlayerScored>(When);
			this.Handles<PlayerScoreUpdated>(When);
			this.Handles<GameFinished>(When);
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

		public void Start(string playerName)
		{
			if (_isRunning.ContainsKey(playerName)) return; // Game already started before

			this.Apply(new TeamGameStarted
			{
				PlayerName = playerName
			});
		}

		public void MakeShot(string playerName, int shotNumber, int score)
		{
			//if (!_isRunning) throw new InvalidOperationException("Game is not running, player mustn't make shots!");

			Shot existing;
			existing = _shots.FirstOrDefault(x => x.PlayerName.Equals(playerName) && x.ShotNumber == shotNumber);

			if (existing == null)
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

			if (_shots.Count == 45)
			{
				this.Apply(new GameFinished
				{
					PlayerName = playerName,
					TotalScore = _shots.Sum(x => x.Score),
					TeamName = _teamName
				});
			}
		}

		private void When(TeamGamePlaced e)
		{
			_teamName = e.TeamName;
		}
		
		private void When(TeamGameStarted e)
		{
			if (string.IsNullOrEmpty(e.PlayerName)) return; // Old data in events 

			_isRunning[e.PlayerName] = true;
		}

		private void When(PlayerScored e)
		{
			_shots.Add(new Shot(e.PlayerName, e.ShotNumber, e.Points));
		}

		private void When(PlayerScoreUpdated e)
		{
			var old = _shots.Single(x => x.PlayerName.Equals(e.PlayerName) && x.ShotNumber == e.ShotNumber);
			_shots.Remove(old);

			_shots.Add(new Shot(e.PlayerName, e.ShotNumber, e.Points));
		}

		private void When(GameFinished e)
		{
			
		}
	}
}
