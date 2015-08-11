using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Infrastructure.ViewModel;
using MassTransit;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Registration.Controllers;
using Registration.Events;
using Registration.Events.Live;
using Registration.ReadModel;
using Registration.Services;

namespace Registration.ViewModels
{
	public class GamingSummaryViewModel : BindableBase
	{
		private readonly GameController _controller;
		private readonly IGameControlService _controlService;
		private readonly IContestDao _contestDao;
		private readonly IServiceBus _liveBus;

		public GamingSummaryViewModel(GameController controller, IGameControlService controlService, IContestDao contestDao, IEventAggregator eventAggregator)
		{
			_controller = controller;
			_controlService = controlService;
			_contestDao = contestDao;

			this.OpenGameSelectionCommand = _controller.OpenGameSelectionCommand;
			this.StartGameCommand = DelegateCommand<SequencingItem>.FromAsyncHandler(StartGame);
			this.MakeShotCommand = DelegateCommand<string>.FromAsyncHandler(MakeShot);
			this.EditShotCommand = new DelegateCommand<string>(EditShot);

			eventAggregator.GetEvent<GameSelected>().Subscribe((payload) => this.CurrentGame = payload);

			_liveBus = ServiceBusFactory.New(sbc =>
			{
				sbc.UseMsmq(msmq =>
				{
					msmq.UseMulticastSubscriptionClient();
					msmq.VerifyMsmqConfiguration();
				});
				sbc.ReceiveFrom("msmq://localhost/ramp-festival_live_sender");
				sbc.SetNetwork("WORKGROUP");
			});
		}

		private SequencingItem _currentGame;
		public SequencingItem CurrentGame
		{
			get
			{
				return _currentGame;
			}
			set
			{
				this.SetProperty(ref _currentGame, value);

				this.Shots = null;
				_startGameOnLoading = true;

				if (value == null) return;

				this.CurrentGameResult = new NotifyTaskCompletion<GameResult>(
				  this.LoadGameResult(value.GameId, value.PlayerName));                
				this.OnPropertyChanged("CurrentGameResult");
			}
		}

		private bool _startGameOnLoading = false;

		public NotifyTaskCompletion<GameResult> CurrentGameResult { get; private set; }

		private Dictionary<int, int> _shots;
		public Dictionary<int, int> Shots
		{
			get
			{
				return _shots;
			}
			set
			{
				this.SetProperty(ref _shots, value);
			}
		}

		private static Dictionary<int, int> GenerateShots(GameResult gameResult, string playerName)
		{
			var shots = new Dictionary<int, int>();

			if (gameResult != null && gameResult.Scores != null)
			{
				foreach (var shot in gameResult.Scores.Where(x => playerName.Equals(x.PlayerName)))
					shots.Add(shot.ShotNumber, shot.Points);
			}

			return shots;
		}

		private int _currentShotNumber;
		public int CurrentShotNumber
		{
			get
			{
				return _shotNumberToEdit > 0 ? _shotNumberToEdit : _currentShotNumber;
			}
			set
			{
				SetProperty(ref _currentShotNumber, value);

				this.OnPropertyChanged("CanMakeShots");
			}
		}

		public async Task<GameResult> LoadGameResult(Guid gameId, string playerName)
		{
			var gameResult = await _contestDao.FindGameResult(gameId);
			this.Shots = GenerateShots(gameResult, playerName);
			this.CurrentShotNumber = gameResult == null ? 1 : gameResult.Scores.Where(x => x.PlayerName.Equals(playerName)).Count() + 1;
			this.PlayerTotalScore = gameResult == null ? 0 : gameResult.TotalScore;

			if (_startGameOnLoading)
			{
				await StartGame(this.CurrentGame);

				List<Events.Live.Shot> eventShots = new List<Events.Live.Shot>();
				foreach (var shot in gameResult.Scores)
					eventShots.Add(new Events.Live.Shot() { PlayerName = shot.PlayerName, ShotNumber = shot.ShotNumber, Score = shot.Points });
				var @event = new GameStarted
				{
					GameId = gameId,
					PlayerName = this.CurrentGame.PlayerName,
					TeamName = this.CurrentGame.TeamName,
					Shots = eventShots
				};

				_liveBus.Publish(@event);

				_startGameOnLoading = false;
			}

			_shotNumberToEdit = 0; // Reset edit mode

			return gameResult;
		}

		private bool _isBusy;
		public bool IsBusy
		{
			get
			{
				return _isBusy;
			}
			set
			{
				this.SetProperty(ref _isBusy, value);

				this.OnPropertyChanged("CanMakeShots");
			}
		}

		private string _busyText;
		public string BusyText
		{
			get
			{
				return _busyText;
			}
			set
			{
				this.SetProperty(ref _busyText, value);
			}
		}

		public bool CanMakeShots
		{
			get
			{
				return !_isBusy && this.CurrentShotNumber <= 9;
			}
		}

		public ICommand OpenGameSelectionCommand { get; private set; }
		public ICommand StartGameCommand { get; private set; }
		public ICommand MakeShotCommand { get; private set; }
		public ICommand EditShotCommand { get; private set; }

		private async Task StartGame(SequencingItem game)
		{
			if (game.GameType == GameType.TeamGame)
			{
				await _controlService.StartTeamGame(game.GameId, game.PlayerName);
			}
			else
			{
				await _controlService.StartSinglePlayerGame(game.GameId);
			}

			await Task.FromResult<object>(null);
		}

		private async Task MakeShot(string scores)
		{
			try
			{
				this.BusyText = "Wurf wird gespeichert...";
				this.IsBusy = true;

				int points = int.Parse(scores);

				if (_currentGame.GameType == GameType.TeamGame)
					await _controlService.MakeTeamPlayerShot(_currentGame.GameId, _currentGame.PlayerName, this.CurrentShotNumber, points);
				else
					await _controlService.MakePlayerShot(_currentGame.GameId, _currentGame.PlayerName, this.CurrentShotNumber, points);

				var @event = new PlayerScored
				{
					GameId = this.CurrentGame.GameId,
					PlayerName = this.CurrentGame.PlayerName,
					Points = points,
					ShotNumber = this.CurrentShotNumber
				};

				int waitForShotNumber = this.CurrentShotNumber;
				_liveBus.Publish(@event);

				bool found = false;
				while (!found)
				{
					var gameResult = await _contestDao.FindGameResult(this.CurrentGame.GameId);

					if (gameResult != null)
						found = gameResult.Scores.FirstOrDefault(x => x.ShotNumber.Equals(waitForShotNumber) && x.PlayerName.Equals(this.CurrentGame.PlayerName)) != null;

					if (!found)
						await Task.Delay(500);
				}

				if (_shotNumberToEdit > 0) await Task.Delay(5000);

				this.CurrentGameResult = new NotifyTaskCompletion<GameResult>(
				  this.LoadGameResult(this.CurrentGame.GameId, this.CurrentGame.PlayerName));
				this.OnPropertyChanged("CurrentGameResult");
			}
			finally
			{
				this.IsBusy = false;
			}
		}

		private int _playerTotalScore;
		public int PlayerTotalScore
		{
			get
			{
				return _playerTotalScore;
			}
			set
			{
				SetProperty(ref _playerTotalScore, value);
			}
		}

		private void EditShot(string param)
		{
			_shotNumberToEdit = int.Parse(param);
			//System.Windows.MessageBox.Show("Edit Shot");
		}

		private int _shotNumberToEdit = 0;
	}
}
