using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Registration.ReadModel;

namespace Registration.ViewModels
{
	public class ContestRegistrationViewModel
	{
		private readonly IContestDao _contestDao;

		public ContestRegistrationViewModel(IContestDao contestDao)
		{
			this._contestDao = contestDao;

			_contests = new ObservableCollection<ContestAlias>();
			this.Contests = new ReadOnlyObservableCollection<ContestAlias>(_contests);

			this.RefreshDataCommand = DelegateCommand.FromAsyncHandler(LoadData);
		}

		private ObservableCollection<ContestAlias> _contests;
		public ReadOnlyObservableCollection<ContestAlias> Contests { get; private set; }

		private async Task LoadData()
		{
			try
			{
				IList<ContestAlias> model = await _contestDao.GetPublishedContests();
				if (model == null) return;

				App.Current.Dispatcher.Invoke(() =>
				{
					_contests.Clear();
					foreach (var item in model)
						_contests.Add(item);
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
