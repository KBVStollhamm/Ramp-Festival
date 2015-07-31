using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace NinepinKiosk.App.Model
{
    public class Score : BindableBase
    {
        public Score(int points)
        {
            this.Points = points;
        }

        public int Points { get; private set; }
    }

    public class Scores : ObservableCollection<Score>
    {
    }
}
