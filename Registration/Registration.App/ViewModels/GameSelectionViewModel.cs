using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Infrastructure.ViewModel;
using Registration.Controllers;
using Registration.ReadModel;

namespace Registration.ViewModels
{
    public class GameSelectionViewModel
    {
        private readonly IContestDao _contestDao;

        public GameSelectionViewModel(GameController controller, IContestDao contestDao, Guid contestId)
        {
            _contestDao = contestDao;

            this.SelectGameCommand = controller.SelectGameCommand;

            this.PendingGames = new NotifyTaskCompletion<IList<SequencingItem>>(
                _contestDao.GetAllPendingGames());
        }

        public NotifyTaskCompletion<IList<SequencingItem>> PendingGames { get; private set; }

        public ICommand SelectGameCommand { get; private set; }
    }
 }
