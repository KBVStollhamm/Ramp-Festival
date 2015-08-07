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
using Registration.ViewModels;

namespace Registration.Views
{
    /// <summary>
    /// Interaction logic for ContestControllingView.xaml
    /// </summary>
    public partial class GameSelectionView : UserControl
    {
        public GameSelectionView(GameSelectionViewModel viewModel)
        {
            InitializeComponent();

            this.DataContext = viewModel;
        }
    }
}
