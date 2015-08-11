using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Registration.Controls
{
    /// <summary>
    /// Interaction logic for TeamHeaderControl.xaml
    /// </summary>
    public partial class TeamHeaderControl : UserControl
    {
        public TeamHeaderControl()
        {
            InitializeComponent();
        }

        public string TeamName
        {
            get { return (string)GetValue(TeamNameProperty); }
            set { SetValue(TeamNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlayerName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TeamNameProperty =
            DependencyProperty.Register("TeamName", typeof(string), typeof(TeamHeaderControl), new PropertyMetadata(default(string)));



        public int TotalScore
        {
            get { return (int)GetValue(TotalScoreProperty); }
            set { SetValue(TotalScoreProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TotalScore.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TotalScoreProperty =
            DependencyProperty.Register("TotalScore", typeof(int), typeof(TeamHeaderControl), new PropertyMetadata(0));
    }
}
