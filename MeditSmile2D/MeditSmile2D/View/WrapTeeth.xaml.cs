using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
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
    public partial class WrapTeeth : UserControl
    {
        public WrapTeeth()
        {
            InitializeComponent();

            lineH.Visibility = Visibility.Hidden;
            lineV.Visibility = Visibility.Hidden;
            lengthH.Visibility = Visibility.Hidden;
            lengthV.Visibility = Visibility.Hidden;
        }

        #region Points

        public IEnumerable Points
        {
            get { return (IEnumerable)GetValue(PointsProperty); }
            set { SetValue(PointsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Points. This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PointsProperty = DependencyProperty.Register(
            "Points", typeof(IEnumerable), typeof(WrapTeeth), new PropertyMetadata(null, PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var wrapPoints = d as WrapTeeth;
            if (wrapPoints == null)
                return;

            if (e.NewValue is INotifyCollectionChanged)
            {
                (e.NewValue as INotifyCollectionChanged).CollectionChanged += wrapPoints.OnPointCollectionChanged;
                wrapPoints.RegisterCollectionItemPropertyChanged(e.NewValue as IEnumerable);
            }

            if (e.OldValue is INotifyCollectionChanged)
            {
                (e.OldValue as INotifyCollectionChanged).CollectionChanged -= wrapPoints.OnPointCollectionChanged;
                wrapPoints.UnRegisterCollectionItemPropertyChanged(e.OldValue as IEnumerable);
            }

            if (e.NewValue != null)
            {
                wrapPoints.SetLineRectData();
            }
        }

        #endregion

        #region ShowLength

        public bool ShowLength
        {
            get { return (bool)GetValue(ShowLengthProperty); }
            set { SetValue(ShowLengthProperty, value); }
        }

        public static readonly DependencyProperty ShowLengthProperty
            = DependencyProperty.Register("ShowLength", typeof(bool), typeof(WrapTeeth),
                                          new PropertyMetadata(false, ShowLengthPropertyChangedCallback));

        private static void ShowLengthPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var wrapPoints = d as WrapTeeth;
            if (wrapPoints == null)
                return;

            if (e.NewValue != null)               
                 wrapPoints.SetLineRectData();                
        }        

        #endregion

        void SetLineRectData()
        {
            if (Points == null) return;
            var points = new List<Point>();

            foreach (var point in Points)
            {
                var pointProperties = point.GetType().GetProperties();
                if (pointProperties.All(p => p.Name != "X") ||
                pointProperties.All(p => p.Name != "Y"))
                    continue;
                var x = (float)point.GetType().GetProperty("X").GetValue(point, new object[] { });
                var y = (float)point.GetType().GetProperty("Y").GetValue(point, new object[] { });
                points.Add(new Point(x, y));
            }

            if (points.Count <= 1)
                return;

            Point minP = GetMin(points);
            Point maxP = GetMax(points);

            DrawRect(minP, maxP);  
            DrawLineXY(minP, maxP);
        }

        private void RegisterCollectionItemPropertyChanged(IEnumerable collection)
        {
            if (collection == null)
                return;
            foreach (INotifyPropertyChanged point in collection)
                point.PropertyChanged += OnPointPropertyChanged;
        }

        private void UnRegisterCollectionItemPropertyChanged(IEnumerable collection)
        {
            if (collection == null)
                return;
            foreach (INotifyPropertyChanged point in collection)
                point.PropertyChanged -= OnPointPropertyChanged;
        }

        private void OnPointCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RegisterCollectionItemPropertyChanged(e.NewItems);
            UnRegisterCollectionItemPropertyChanged(e.OldItems);

            SetLineRectData();
        }

        private void OnPointPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "X" || e.PropertyName == "Y")
                SetLineRectData();
        }

        #region DrawRect & DrawLine 

        private Point GetMin(List<Point> pts)
        {
            double xMin = float.MaxValue;
            double yMin = float.MaxValue;

            foreach (var point in pts)
            {
                if (point.X < xMin)
                    xMin = point.X;
                if (point.Y < yMin)
                    yMin = point.Y;
            }
            return new Point(xMin, yMin);
        }

        private Point GetMax(List<Point> pts)
        {
            double xMax = float.MinValue;
            double yMax = float.MinValue;

            foreach (var point in pts)
            {
                if (point.X > xMax)
                    xMax = point.X;
                if (point.Y > yMax)
                    yMax = point.Y;
            }

            return new Point(xMax, yMax);
        }

        public double Top;
        public double Left;

        readonly double padding = 2;
        private void DrawRect(Point min, Point max)
        {            
            // Grid의 크기 설정
            innerWrap.Height = max.Y - min.Y + padding;
            innerWrap.Width = max.X - min.X + padding;

            Top = min.Y - padding / 2;
            Left = min.X - padding / 2;

            Canvas.SetLeft(this, Left);
            Canvas.SetTop(this, Top);
        }
        
        private void DrawLineXY(Point min, Point max)
        {
            double widthRect = max.X - min.X;
            double heightRect = max.Y - min.Y;
            Point startHorizontal = new Point(min.X, min.Y + heightRect / 2);
            Point endHorizontal = new Point(max.X, min.Y + heightRect / 2);
            Point startVertical = new Point(min.X + widthRect / 2, min.Y);
            Point endVertical = new Point(min.X + widthRect / 2, max.Y);

            // point자체는 전체 canvas에 대한 좌표이므로 각 WrapTeeth를 감싸는 Grid에 대한 좌표로 변환
            lineH.X1 = startHorizontal.X - Left;
            lineH.Y1 = startHorizontal.Y - Top;
            lineH.X2 = endHorizontal.X - Left;
            lineH.Y2 = endHorizontal.Y - Top;

            lineV.X1 = startVertical.X - Left;
            lineV.Y1 = startVertical.Y - Top;
            lineV.X2 = endVertical.X - Left;
            lineV.Y2 = endVertical.Y - Top;

            // infomation of length
            double value;
            double leftH, topH, leftV, topV;
            double padding = 10;

            // for length of Horizontal Line
            lengthH.Content = widthRect.ToString();
            value = lineH.X1 + widthRect / 2 - padding;
            double.TryParse(value.ToString("N2"), out leftH);
            value = lineV.Y1 + padding;
            double.TryParse(value.ToString("N2"), out topH);

            Canvas.SetLeft(lengthH, leftH);
            Canvas.SetTop(lengthH, topH);

            // for length of Vertical Line
            lengthV.Content = heightRect.ToString();
            value = lineV.X1 + padding;
            double.TryParse(value.ToString("N2"), out leftV);
            value = lineV.Y1 + heightRect / 2 - padding;
            double.TryParse(value.ToString("N2"), out topV);

            Canvas.SetLeft(lengthV, leftV);
            Canvas.SetTop(lengthV, topV);

            if (ShowLength == true)
            {
                lineH.Visibility = Visibility.Visible;
                lineV.Visibility = Visibility.Visible;
                lengthH.Visibility = Visibility.Visible;
                lengthV.Visibility = Visibility.Visible;
            } else
            {
                lineH.Visibility = Visibility.Hidden;
                lineV.Visibility = Visibility.Hidden;
                lengthH.Visibility = Visibility.Hidden;
                lengthV.Visibility = Visibility.Hidden;
            }
        }

        #endregion

    }
}
