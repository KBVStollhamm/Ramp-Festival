using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
using Registration.ViewModels;
using Registration.Views;

namespace Registration.Controllers
{
    public class GameController
    {
        private readonly IRegionManager _regionManager;
        private readonly Func<Guid, GameSelectionViewModel> _viewModelFactory;
        private readonly Func<GameSelectionViewModel, GameSelectionView> _viewFactory;

        public GameController(IRegionManager regionManager, Func<Guid, GameSelectionViewModel> viewModelFactory, Func<GameSelectionViewModel, GameSelectionView> viewFactory)
        {
            _regionManager = regionManager;
            _viewModelFactory = viewModelFactory;
            _viewFactory = viewFactory;

            this.GoToGameSelectionCommand = DelegateCommand.FromAsyncHandler(ShowGameSelection);
        }

        public async Task ShowGameSelection()
        {
            await Task.Delay(100);

            IRegion mainRegion = _regionManager.Regions["MainRegion"];

            var viewModel = _viewModelFactory.Invoke(new Guid("C4DB0204-5A4F-47D8-9DBB-014F09AC78E4"));
            var view = _viewFactory.Invoke(viewModel);
            mainRegion.Add(view);

            mainRegion.Activate(view);
        }

        public ICommand GoToGameSelectionCommand {get;private set;}
    }
}
