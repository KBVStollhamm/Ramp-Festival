using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.ViewModel;
using Registration.ReadModel;

namespace Registration.ViewModels
{
    public class GameSelectionViewModel
    {
        private readonly IContestDao _contestDao;

        public GameSelectionViewModel(IContestDao contestDao, Guid contestId)
        {
            _contestDao = contestDao;

            this.PendingGames = new NotifyTaskCompletion<IList<SequencingItem>>(
                _contestDao.GetAllPendingGames());
        }

        public NotifyTaskCompletion<IList<SequencingItem>> PendingGames { get; private set; }
    }
 }
