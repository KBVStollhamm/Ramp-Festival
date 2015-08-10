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
			this.EditShotCommand = new DelegateCommand(EditShot);

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
				  this.LoadGameResult(value.GameId));                
				this.OnPropertyChanged("CurrentGameResult");
			}
		}

		private bool _startGameOnLoading = false;

		private void CurrentGameResult_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "IsSuccessfullyCompleted")
			{

			}
		}

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

		private static Dictionary<int, int> GenerateShots(GameResult gameResult)
		{
			var shots = new Dictionary<int, int>();

			if (gameResult != null && gameResult.Scores != null)
			{
				foreach (var shot in gameResult.Scores)
					shots.Add(shot.ShotNumber, shot.Points);
			}

			return shots;
		}

		private int _currentShotNumber;
		public int CurrentShotNumber
		{
			get
			{
				return _currentShotNumber;
			}
			set
			{
				SetProperty(ref _currentShotNumber, value);

				this.OnPropertyChanged("CanMakeShots");
			}
		}

		public async Task<GameResult> LoadGameResult(Guid gameId)
		{
			var gameResult = await _contestDao.FindGameResult(gameId);
			this.Shots = GenerateShots(gameResult);
			this.CurrentShotNumber = gameResult == null ? 1 : gameResult.Scores.Count() + 1;

			if (_startGameOnLoading)
			{
				await StartGame(this.CurrentGame);
				_startGameOnLoading = false;
			}

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

			var @event = new GameStarted
			{
				GameId = game.GameId,
				PlayerName = game.PlayerName,
				TeamName = game.TeamName           
			};

			_liveBus.Publish(@event);

			await Task.FromResult<object>(null);
		}

		private async Task MakeShot(string scores)
		{
			try
			{
				this.BusyText = "Wurf wird gespeichert...";
				this.IsBusy = true;

				int points = int.Parse(scores);

				await _controlService.MakePlayerShot(_currentGame.GameId, this.CurrentShotNumber, points);

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
						found = gameResult.Scores.FirstOrDefault(x => x.ShotNumber.Equals(waitForShotNumber)) != null;

					if (!found)
						await Task.Delay(500);
				}

				this.CurrentGameResult = new NotifyTaskCompletion<GameResult>(
				  this.LoadGameResult(this.CurrentGame.GameId));
				this.OnPropertyChanged("CurrentGameResult");     
			}
			finally
			{
				this.IsBusy = false;
			}
		}

		private void EditShot()
		{
			System.Windows.MessageBox.Show("Edit Shot");
		}        
	}
}
