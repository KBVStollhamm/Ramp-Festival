﻿using MassTransit;
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
				await LoadData();
			}, e => e.ContestId.Equals(Constants.NinepinContestId));

			this.RefreshDataCommand = DelegateCommand.FromAsyncHandler(LoadData);
		}

		private ObservableCollection<SequencingItem> _sequence;
		public ReadOnlyObservableCollection<SequencingItem> Sequence { get; private set; }

		private async Task LoadData()
		{
            try
            {


                Sequencing model = await _contestDao.FindSequencing(Constants.NinepinContestId);
                if (model == null) return;

                App.Current.Dispatcher.Invoke(() =>
                {
                    _sequence.Clear();
                    foreach (var item in model.Sequence.OrderBy(x => x.RegisteredAt).ThenBy(x => x.Position))
                        _sequence.Add(item);
                });
            }
            catch (Exception ex)
            {
                  //TODO: Implement exception handling
            }
		}

		public ICommand RefreshDataCommand { get; private set; }
	}
}
