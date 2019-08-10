using MeditSmile2D.Common;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MeditSmile2D.View
{
    /// <summary>
    /// Tooth.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UpperTooth : UserControl
    {
        public UpperTooth()
        {
            InitializeComponent();
        }

        #region ShowLengths
        public static readonly DependencyProperty ShowLengthsProperty =
            DependencyProperty.Register("ShowLengths", typeof(bool), typeof(UpperTooth));

        public bool ShowLengths
        {
            get { return (bool)GetValue(ShowLengthsProperty); }
            set { SetValue(ShowLengthsProperty, value); }
        }
        #endregion

        #region Fill
        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register("Fill", typeof(bool), typeof(UpperTooth));

        public bool Fill
        {
            get { return (bool)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }
        #endregion

    }
}
