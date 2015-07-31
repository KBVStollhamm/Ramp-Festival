using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using NinepinKiosk.App.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace NinepinKiosk.App.ViewModels
{
    public class MainPageViewModel
    {
        public MainPageViewModel(INavigationService navigationService)
        {
            this.Group = new Item("Group1", "ggg", "A group");
            this.Items = new ObservableCollection<Item>();
            this.Items.Add(new Item("Item1", "abc", "This is my first item"));
            this.Items.Add(new Item("Item2", "def", "This is my second item"));

            this.Scores = new Scores();
            for (int i = 1; i <= 9; i++)
                this.Scores.Add(new Score(i));

            this.GoBackCommand = new DelegateCommand(navigationService.GoBack, navigationService.CanGoBack);
        }

        public string Title
        {
            get { return "Luftkegeln"; }
        }

        public Item Group { get; private set; }
        public ObservableCollection<Item> Items { get; private set; }

        public Scores Scores { get; private set; }

        public ICommand GoBackCommand { get; private set; }
    }
}
