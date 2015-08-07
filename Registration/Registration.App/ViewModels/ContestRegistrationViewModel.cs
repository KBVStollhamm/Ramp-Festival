using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Registration.Controllers;
using Registration.ReadModel;

namespace Registration.ViewModels
{
	public class ContestRegistrationViewModel
	{
		private readonly IContestDao _contestDao;

		public ContestRegistrationViewModel(RegistrationsController controller, IContestDao contestDao)
		{
			this._contestDao = contestDao;

			_contests = new ObservableCollection<Contest>();
			this.Contests = new ReadOnlyObservableCollection<Contest>(_contests);

			this.RefreshDataCommand = DelegateCommand.FromAsyncHandler(LoadData);
			this.RegisterPlayerCommand = controller.RegisterPlayerCommand;
			this.RegisterTeamCommand = controller.RegisterTeamCommand;
		}

		private ObservableCollection<Contest> _contests;
		public ReadOnlyObservableCollection<Contest> Contests { get; private set; }

		private async Task LoadData()
		{
			try
			{
				IList<Contest> model = await _contestDao.GetPublishedContests();
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

		private void StartRegistration(Contest contest)
		{
		}

		public ICommand RefreshDataCommand { get; private set; }
		public ICommand RegisterPlayerCommand { get; private set; }
		public ICommand RegisterTeamCommand { get; private set; }
	}
}
