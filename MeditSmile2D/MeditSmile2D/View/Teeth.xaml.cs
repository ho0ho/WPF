using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace MeditSmile2D.View
{
    public partial class Teeth : UserControl
    {
        public Teeth()
        {
            InitializeComponent();
        }

        #region Points

        public static readonly DependencyProperty PointsProperty
            = DependencyProperty.Register("Points", typeof(IEnumerable), typeof(Teeth));

        public IEnumerable Points
        {
            get { return (IEnumerable)GetValue(PointsProperty); }
            set { SetValue(PointsProperty, value); }
        }

        #endregion

        #region ShowLengths
        public static readonly DependencyProperty ShowLengthXYProperty
            = DependencyProperty.Register("ShowLengthXY", typeof(bool), typeof(Teeth));

        public bool ShowLengthXY
        {
            get { return (bool)GetValue(ShowLengthXYProperty); }
            set { SetValue(ShowLengthXYProperty, value); }
        }
        #endregion

        #region Fill
        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register("Fill", typeof(bool), typeof(Teeth));

        public bool Fill
        {
            get { return (bool)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }
        #endregion
    }
}
