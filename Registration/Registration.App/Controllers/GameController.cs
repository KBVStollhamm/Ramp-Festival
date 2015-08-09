using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using Registration.Events;
using Registration.ReadModel;
using Registration.ViewModels;
using Registration.Views;

namespace Registration.Controllers
{
    public class GameController
    {
        private readonly IRegionManager _regionManager;
        private readonly GameControlFrameView _frameView;
        private readonly Func<Guid, GameSelectionViewModel> _viewModelFactory;
        private readonly Func<GameSelectionViewModel, GameSelectionView> _viewFactory;
        private readonly IEventAggregator _eventAggregator;

        public GameController(IRegionManager regionManager, GameControlFrameView frameView, Func<Guid, GameSelectionViewModel> viewModelFactory, Func<GameSelectionViewModel, GameSelectionView> viewFactory, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _frameView = frameView;
            _viewModelFactory = viewModelFactory;
            _viewFactory = viewFactory;
            _eventAggregator = eventAggregator;

            this.SelectGameCommand = new DelegateCommand<SequencingItem>(SelectGame);
            this.OpenGameSelectionCommand = new DelegateCommand(ShowGameSelection);
        }

        private void SelectGame(SequencingItem game)
        {
            ShowGamingOverview();

            _eventAggregator.GetEvent<GameSelected>().Publish(game);
        }

        private void ShowGamingOverview()
        {
            IRegion region = _regionManager.Regions["MainRegion"];

            object summaryView = region.GetView("GamingSummaryView");
            if (summaryView == null)
            {
                summaryView = ServiceLocator.Current.GetInstance<IGamingSummaryView>();
                region.Add(summaryView, "GamingSummaryView");
            }

            region.Activate(summaryView);
        }
    
        private void ShowGameSelection()
        {
            IRegion controllingRegion = _regionManager.Regions["MainRegion"];

            var viewModel = _viewModelFactory.Invoke(new Guid("C4DB0204-5A4F-47D8-9DBB-014F09AC78E4"));
            var view = _viewFactory.Invoke(viewModel);
            controllingRegion.Add(view);

            controllingRegion.Activate(view);
        }

        public ICommand SelectGameCommand{ get; private set; }
        public ICommand OpenGameSelectionCommand {get;private set;}
    }
}
