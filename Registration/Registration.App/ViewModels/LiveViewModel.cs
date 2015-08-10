using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MassTransit;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Registration.Events.Live;
using Registration.Models;

namespace Registration.ViewModels
{
    public class LiveViewModel : BindableBase
    {
        public LiveViewModel()
        {
            _currentPlayerName = "MAaddi";
            _scores = new Dictionary<int, int>();
            this.Scores = new ReadOnlyDictionary<int, int>(_scores);

            _scores[1] = 55;

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

        private Dictionary<int, int> _scores;
        public ReadOnlyDictionary<int, int> Scores { get; private set; }

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
        }

        private void OnPlayerScored(PlayerScored e)
        {
            _scores[e.ShotNumber] = e.Points;
            this.OnPropertyChanged("Scores");
        }
    }
}
