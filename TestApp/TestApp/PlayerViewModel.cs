using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Threading;

namespace TestApp
{
	public class PlayerViewModel : INotifyPropertyChanged, INotifyPropertyChanging 
	{
		Random _rand = new Random(Environment.TickCount);
		List<string> _playerNames;
		Timer _timer;

		public PlayerViewModel()
		{
			// alle 10 sekunden 
			_timer = new Timer(5000);
			_timer.Elapsed += Timer_Elapsed;
			_timer.Start();

			_playerNames = Enumerable.Range(0, 50).Select(i => "Player " + i).ToList();
		}

		private void Timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			PlayerName = _playerNames[_rand.Next(0, _playerNames.Count)];
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (!ReferenceEquals(handler, null))
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public event PropertyChangingEventHandler PropertyChanging;

		protected virtual void OnPropertyChanging(string propertyName)
		{
			PropertyChangingEventHandler handler = PropertyChanging;
			if (!ReferenceEquals(handler, null))
			{
				handler(this, new PropertyChangingEventArgs(propertyName));
			}
		}

		private string _playerName;
		public string PlayerName
		{
			get { return _playerName; }
			set
			{
				if (_playerName == value)
					return;

				OnPropertyChanging("PlayerName");
				_playerName = value;
				OnPropertyChanged("PlayerName");
			}
		}
	}
}