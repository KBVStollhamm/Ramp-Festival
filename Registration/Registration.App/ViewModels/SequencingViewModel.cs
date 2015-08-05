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
            eventBus.SubscribeHandler<SequencingChanged>(e =>
            {
                LoadData();
            }, e => e.ContestId.Equals(Constants.NinepinContestId));
            LoadData();
        }

        private ObservableCollection<SequencingItem> _sequence;
        public ReadOnlyObservableCollection<SequencingItem> Sequence { get; private set; }

        private void LoadData()
        {
            Sequencing model = _contestDao.FindSequencing(Constants.NinepinContestId);
            if (model == null) return;

            App.Current.Dispatcher.Invoke(() =>
            {
                _sequence.Clear();
                foreach (var item in model.Sequence.OrderBy(x => x.Position))
                    _sequence.Add(item);
            });
        }
    }
}
