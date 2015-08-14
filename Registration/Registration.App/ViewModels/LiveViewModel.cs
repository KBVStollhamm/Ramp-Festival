using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Infrastructure.ViewModel;
using MassTransit;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Registration.Events.Live;
using Registration.Models;
using Registration.ReadModel;

namespace Registration.ViewModels
{
	public class LiveViewModel : BindableBase
	{
		private readonly IContestDao _contestDao;

		public LiveViewModel(IContestDao contestDao)
		{
			_contestDao = contestDao;

			_currentPlayerName = "KEIN SPIEL GESTARTET";
			_scores = new Dictionary<int, int>();
			this.Scores = new ReadOnlyDictionary<int, int>(_scores);

			this.RefreshCommand = DelegateCommand.FromAsyncHandler(Refresh);

			InitializeEventHandling();
		}

		private Guid _currentGameId;
		public Guid CurrentGameId
		{
			get
			{
				return _currentGameId;
			}
			private set
			{
				SetProperty(ref _currentGameId, value);
			}
		}

		private string _currentPlayerName;
		public string CurrentPlayerName
		{
			get
			{
				return _currentPlayerName;
			}
			private set
			{
				SetProperty(ref _currentPlayerName, value);
			}
		}

		private string _currentTeamName;
		public string CurrentTeamName
		{
			get
			{
				return _currentTeamName;
			}
			private set
			{
				SetProperty(ref _currentTeamName, value);
			}
		}

		private Dictionary<int, int> _scores;
		public ReadOnlyDictionary<int, int> Scores { get; private set; }

		public int PlayerTotalScore
		{
			get
			{
				return this.Scores.Values.Sum();
			}
		}

		private int _totalTeamScoreWithoutPlayer;
		public int TeamTotalScore
		{
			get
			{
				return _totalTeamScoreWithoutPlayer + this.PlayerTotalScore;
			}
		}

		public bool IsTeamGame
		{
			get
			{
				return !string.IsNullOrEmpty(_currentTeamName);
			}
		}

		public ICommand RefreshCommand { get; private set; }

		private async Task Refresh()
		{
			this.OnPropertyChanged(() => this.Scores);

			await Task.FromResult<object>(null);
		}

		private void InitializeEventHandling()
		{
			var bus = ServiceBusFactory.New(sbc =>
			{
				sbc.UseMsmq(msmq =>
				{
					msmq.UseMulticastSubscriptionClient();
					msmq.VerifyMsmqConfiguration();
				});
				sbc.ReceiveFrom("msmq://localhost/ramp-festival_live_receiver");
				sbc.SetNetwork("WORKGROUP");

				sbc.Subscribe(s => s.Handler<GameStarted>(OnGameStarted));
				sbc.Subscribe(s => s.Handler<PlayerScored>(OnPlayerScored));
			});
		}


		private void OnGameStarted(GameStarted e)
		{
			this.CurrentGameId = e.GameId;
			this.CurrentPlayerName = e.PlayerName;
			this.CurrentTeamName = e.TeamName;
			_scores.Clear();
			if (e.Shots != null)
			{
				foreach (var shot in e.Shots.Where(x => e.PlayerName.Equals(x.PlayerName)))
				{
					_scores.Add(shot.ShotNumber, shot.Score);
				}
			}
			this.OnPropertyChanged("Scores");
			this.OnPropertyChanged("PlayerTotalScore");
			if (e.Shots != null)
				_totalTeamScoreWithoutPlayer = e.Shots.Where(x => !e.PlayerName.Equals(x.PlayerName)).Sum(x => x.Score);
			else
				_totalTeamScoreWithoutPlayer = 0;
			this.OnPropertyChanged("TeamTotalScore");
			this.OnPropertyChanged("IsTeamGame");

			this.Items = new ObservableCollection<LeaderboardItem>();

			this.Leaderboard = new NotifyTaskCompletion<Leaderboard>(this.GetLeaderboard());
			this.OnPropertyChanged("Leaderboard");

			for (int i = 1; i <= 9; i++)
				this.RaiseScoreChanged(i);

		}

		private void OnPlayerScored(PlayerScored e)
		{
			_scores[e.ShotNumber] = e.Points;
			//this.OnPropertyChanged("Scores[]");
			this.OnPropertyChanged("Scores");
			this.OnPropertyChanged("PlayerTotalScore");
			this.OnPropertyChanged("TeamTotalScore");

			RaiseScoreChanged(e.ShotNumber);
		}

		private void RaiseScoreChanged(int shotNumber)
		{
			string propertyName = "Score" + shotNumber.ToString();
			this.OnPropertyChanged(propertyName);
		}

		public int Score1
		{
			get
			{
				int v = 0;
				_scores.TryGetValue(1, out v);
				return v;
			}
			set
			{
				_scores[1] = value;
				this.OnPropertyChanged("Score1");
			}
		}
		public int? Score2
		{
			get
			{
				if (_scores.ContainsKey(2))
					return _scores[2];
				return null;
			}
			set
			{
				if (!value.HasValue)
					_scores.Remove(2);
				else
					_scores[2] = value.Value;
			}
		}
		public int? Score3
		{
			get
			{
				if (_scores.ContainsKey(3))
					return _scores[3];
				return null;
			}
			set
			{
				if (!value.HasValue)
					_scores.Remove(3);
				else
					_scores[3] = value.Value;
			}
		}
		public int? Score4
		{
			get
			{
				if (_scores.ContainsKey(4))
					return _scores[4];
				return null;
			}
			set
			{
				if (!value.HasValue)
					_scores.Remove(4);
				else
					_scores[4] = value.Value;
			}
		}
		public int? Score5
		{
			get
			{
				if (_scores.ContainsKey(5))
					return _scores[5];
				return null;
			}
			set
			{
				if (!value.HasValue)
					_scores.Remove(5);
				else
					_scores[5] = value.Value;
			}
		}
		public int? Score6
		{
			get
			{
				if (_scores.ContainsKey(6))
					return _scores[6];
				return null;
			}
			set
			{
				if (!value.HasValue)
					_scores.Remove(6);
				else
					_scores[6] = value.Value;
			}
		}
		public int? Score7
		{
			get
			{
				if (_scores.ContainsKey(7))
					return _scores[7];
				return null;
			}
			set
			{
				if (!value.HasValue)
					_scores.Remove(7);
				else
					_scores[7] = value.Value;
			}
		}
		public int? Score8
		{
			get
			{
				if (_scores.ContainsKey(8))
					return _scores[8];
				return null;
			}
			set
			{
				if (!value.HasValue)
					_scores.Remove(8);
				else
					_scores[8] = value.Value;
			}
		}
		public int? Score9
		{
			get
			{
				if (_scores.ContainsKey(9))
					return _scores[9];
				return null;
			}
			set
			{
				if (!value.HasValue)
					_scores.Remove(9);
				else
					_scores[9] = value.Value;
			}
		}

		public ObservableCollection<LeaderboardItem> Items
		{
			get;
			private set;
		}

		public NotifyTaskCompletion<Leaderboard> Leaderboard { get; private set; }

		private async Task<Leaderboard> GetLeaderboard()
		{
			var board =  await _contestDao.GetLeaderboard();

			//this.Items.Clear();
			//if (board.SinglePlayerContestLeader != null)
			//	this.Items.Add(board.SinglePlayerContestLeader);

			return board;
		}
	}
}
