using MassTransit;
using Microsoft.Practices.Prism.Mvvm;
using Registration.Events;
using Registration.ReadModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.Commands;
using System.Windows.Input;

namespace Registration.ViewModels
{
	public class SequencingViewModel : BindableBase
	{
		private readonly IContestDao _contestDao;

		public SequencingViewModel(IContestDao contestDao, IServiceBus eventBus)
		{
			_contestDao = contestDao;

			_sequence = new ObservableCollection<SequencingItem>();
			this.Sequence = new ReadOnlyObservableCollection<SequencingItem>(_sequence);
			eventBus.SubscribeHandler<SequencingChanged>(async (e) =>
			{
				await LoadData().ConfigureAwait(false);
			});

			this.RefreshDataCommand = DelegateCommand.FromAsyncHandler(LoadData);
		}

		private ObservableCollection<SequencingItem> _sequence;
		public ReadOnlyObservableCollection<SequencingItem> Sequence { get; private set; }

		private async Task LoadData()
		{
			try
			{
				IList<SequencingItem> model = await _contestDao.GetAllNewGames();
				if (model == null) return;

				//app.current.dispatcher.invoke(() =>
				//{
					_sequence.Clear();
					foreach (var item in model)
						_sequence.Add(item);
				//});
			}
			catch (Exception ex)
			{
				  //TODO: Implement exception handling
			}
		}

		public ICommand RefreshDataCommand { get; private set; }
	}
}
