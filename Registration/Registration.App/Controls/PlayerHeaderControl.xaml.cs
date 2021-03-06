﻿using System;
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
    /// Interaction logic for PlayerHeaderControl.xaml
    /// </summary>
    public partial class PlayerHeaderControl : UserControl
    {
        public PlayerHeaderControl()
        {
            InitializeComponent();            
        }

        public string PlayerName
        {
            get { return (string)GetValue(PlayerNameProperty); }
            set { SetValue(PlayerNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlayerName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlayerNameProperty =
            DependencyProperty.Register("PlayerName", typeof(string), typeof(PlayerHeaderControl), new PropertyMetadata(default(string)));

        public int TotalScore
        {
            get { return (int)GetValue(TotalScoreProperty); }
            set { SetValue(TotalScoreProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TotalScore.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TotalScoreProperty =
            DependencyProperty.Register("TotalScore", typeof(int), typeof(PlayerHeaderControl), new PropertyMetadata(0));
    }
}
