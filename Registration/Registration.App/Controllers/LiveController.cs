using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using Registration.Views;

namespace Registration.Controllers
{
    public class LiveController
    {
        private readonly IRegionManager _regionManager;
        private readonly Func<ILiveView> _viewFactory;

        public LiveController(IRegionManager regionManager, Func<ILiveView> viewFactory)
        {
            _regionManager = regionManager;
            _viewFactory = viewFactory;

            this.LiveShowCommand = new DelegateCommand(ShowLiveView);
        }

        private void ShowLiveView()
        {
            IRegion region = _regionManager.Regions["MainRegion"];

            object liveView = region.GetView("LiveView");
            if (liveView == null)
            {
                liveView = _viewFactory.Invoke();
                region.Add(liveView, "LiveView");
            }

            region.Activate(liveView);

            ShowLeaderboard();
        }

        private void ShowLeaderboard()
        {
            IRegion region = _regionManager.Regions["DetailsRegion"];

            object view = region.GetView("LeaderboardView");
            if (view == null)
            {
                view = ServiceLocator.Current.GetInstance<ILeaderboardView>();
                region.Add(view, "LeaderboardView");
            }

            region.Activate(view
                );
        }

        public ICommand LiveShowCommand { get; private set; }
    }
}
