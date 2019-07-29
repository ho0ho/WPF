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

namespace MeditSmile2D.View
{
    /// <summary>
    /// Tooth.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Tooth : UserControl
    {
        public Tooth()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ShowLengthsProperty =
            DependencyProperty.Register("ShowLengths", typeof(bool), typeof(Tooth));

        public bool ShowLengths
        {
            get { return (bool)GetValue(ShowLengthsProperty); }
            set { SetValue(ShowLengthsProperty, value); }
        }

    }
}
