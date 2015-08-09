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
    /// Interaction logic for ShotsControl.xaml
    /// </summary>
    public partial class ShotsControl : UserControl
    {
        public ShotsControl()
        {
            InitializeComponent();
        }



        public int? Shot1
        {
            get { return (int?)GetValue(Shot1Property); }
            set { SetValue(Shot1Property, value); }
        }

        // Using a DependencyProperty as the backing store for Shot1.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Shot1Property =
            DependencyProperty.Register("Shot1", typeof(int?), typeof(ShotsControl), new PropertyMetadata(null));



        public ICommand EditShotCommand
        {
            get { return (ICommand)GetValue(EditShotCommandProperty); }
            set { SetValue(EditShotCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EditShotCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EditShotCommandProperty =
            DependencyProperty.Register("EditShotCommand", typeof(ICommand), typeof(ShotsControl), new PropertyMetadata(null));


    }
}
