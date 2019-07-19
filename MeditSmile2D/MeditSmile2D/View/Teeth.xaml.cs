using MeditSmile2D.View.Utils;
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
    /// <summary>
    /// Teeth.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Teeth : UserControl
    {
        public Teeth()
        {
            InitializeComponent();
        }

        #region Points

        public IEnumerable Points
        {
            get { return (IEnumerable)GetValue(PointsProperty); }
            set { SetValue(PointsProperty, value); }
        }

        public static readonly DependencyProperty PointsProperty =
            DependencyProperty.Register("Points", typeof(IEnumerable), typeof(Teeth), new PropertyMetadata(null, PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            /// 이 함수내로 진입했다는 것은, Teeth.Points 프로퍼티가 SetValue() 된 적이 있었음을 의미.
            /// Teeth.Points에 들어오는 데이터 DependencyObject d는 TeethType으로, 
            /// 이 함수는 결국 치아 1개에 대한 데이터(가 변동될 때)를 다루는 함수

            // 그래서 Teeth로 형변환 후 진행
            var teeth = d as Teeth;
            if (teeth == null) return;

            if (e.NewValue is INotifyCollectionChanged)
            {
                /* 이 함수에서 수행하는 전체 프로세스는 아래와 같다. 
                 
                   1. 치아 1개 자체의 값이 변경될 때 수행될 Changed 함수를 등록
                      e.NewValue => ToothType
                                 => INotifyCollectionChanged로 형변환한 후 
                                 => INotifyCollectionChanged.CollectionChanged 에 Changed함수 등록
                                 - 치아 1개(점 10개의 콜렉션)에 대한 Changed 함수 : Teeth.OnPointCollectionChanged()

                   2. 그 후, e.NewValue(=> 치아 1개를 의미)가 갖는 10개의 PointViewModel객체에 대해 각각 Changed함수 등록
                      point      => TeethType
                                 => INotifyPropertyChanged로 형변환한 후,
                                 => INotifyCollectionChanged.PropertyChanged 에 Changed함수 등록
                                 - 점 1개에 대한 Changed 함수 : Teeth.OnPointPropertyChanged() 

                   즉, 모든 각 점(PointViewModel 객체)이 변경될 때 수행될 OnChanged 함수를 직접 정의하고, 
                   => Teeth.OnPointPropertyChanged() 정의
                   이 모든 점들의 Changed 함수를 등록하기 위해, 각 점들을 INotifyPropertyChanged 타입으로 형변환한뒤, 
                   INotifyPropertyChanged.PropertyChanged에 Changed 함수를 모두 등록한다. 

                   치아 1개(TeethType 객체)가 변경될 때 수행될 OnChanged 함수를 직접 정의한 뒤,
                   => Teeth.OnPointCollectionChanged() 정의
                   해당 치아의 Change 함수를 등록하기 위해, 해당 치아를 INotifyCollectionChanged 타입으로 형변환한 뒤,
                   INotifyCollectionChanged.CollectionChanged에 Changed 함수를 등록한다.
                 */

                (e.NewValue as INotifyCollectionChanged).CollectionChanged += teeth.OnPointCollectionChanged;
                teeth.RegisterCollectionItemPropertyChanged(e.NewValue as IEnumerable);
            }

            if (e.OldValue is INotifyCollectionChanged)
            {
                (e.OldValue as INotifyCollectionChanged).CollectionChanged -= teeth.OnPointCollectionChanged;
                teeth.UnRegisterCollectionItemPropertyChanged(e.OldValue as IEnumerable);
            }

            if (e.NewValue != null)
                teeth.SetPathData();
        }

        #endregion

        #region PathColor

        public Brush PathColor
        {
            get { return (Brush)GetValue(PathColorProperty); }
            set { SetValue(PathColorProperty, value); }
        }

        public static readonly DependencyProperty PathColorProperty =
            DependencyProperty.Register("PathColor", typeof(Brush), typeof(Teeth), new PropertyMetadata(Brushes.Black));

        #endregion

        #region IsClosedCurve

        public bool IsClosedCurve
        {
            get { return (bool)GetValue(IsClosedCurveProperty); }
            set { SetValue(IsClosedCurveProperty, value); }
        }

        public static readonly DependencyProperty IsClosedCurveProperty =
            DependencyProperty.Register("IsClosedCurve", typeof(bool), typeof(Teeth), new PropertyMetadata(true));
        // new PropertyMetadata(default(bool), OnIsClosedCurveChanged));

        #endregion

        private void SetPathData()
        {
            // Drawing
            if (Points == null) return;
            var points = new List<Point>();

            foreach (var point in Points)
            {
                var pointProperties = point.GetType().GetProperties();
                if (pointProperties.All(p => p.Name != "X") || pointProperties.All(p => p.Name != "Y"))
                    continue;
                var x = (float)point.GetType().GetProperty("X").GetValue(point, new object[] { });
                var y = (float)point.GetType().GetProperty("Y").GetValue(point, new object[] { });
                points.Add(new Point(x, y));
            }

            if (points.Count <= 1) return;

            var Teeth_PathFigure = new PathFigure { StartPoint = points.FirstOrDefault() };
            var Teeth_SegmentCollection = new PathSegmentCollection();
            var bezierSegments = InterpolationUtils.InterpolatePointWithBezierCurves(points, IsClosedCurve);
            if (bezierSegments == null || bezierSegments.Count < 1)
            {
                foreach (var point in points.GetRange(1, points.Count - 1))
                {
                    var lineSegment = new LineSegment { Point = point };
                    Teeth_SegmentCollection.Add(lineSegment);
                }
            }
            else
            {
                foreach (var curveSeg in bezierSegments)
                {
                    var segment = new BezierSegment
                    {
                        Point1 = curveSeg.FirstControlPoint,
                        Point2 = curveSeg.SecondControlPoint,
                        Point3 = curveSeg.EndPoint
                    };
                    Teeth_SegmentCollection.Add(segment);
                }
            }

            Teeth_PathFigure.Segments = Teeth_SegmentCollection;
            var Teeth_PathFigureCollection = new PathFigureCollection { Teeth_PathFigure };
            var Teeth_PathGeometry = new PathGeometry { Figures = Teeth_PathFigureCollection };

            path.Data = Teeth_PathGeometry;
        }

        private void RegisterCollectionItemPropertyChanged(IEnumerable collection)
        {
            if (collection == null) return;
            foreach (INotifyPropertyChanged point in collection)
                point.PropertyChanged += OnPointPropertyChanged;
        }

        private void UnRegisterCollectionItemPropertyChanged(IEnumerable collection)
        {
            if (collection == null) return;
            foreach (INotifyPropertyChanged point in collection)
                point.PropertyChanged -= OnPointPropertyChanged;
        }

        private void OnPointCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RegisterCollectionItemPropertyChanged(e.NewItems);
            UnRegisterCollectionItemPropertyChanged(e.OldItems);
            SetPathData();
        }

        // 이게 가장 근본적인 Changed 함수
        private void OnPointPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "X" || e.PropertyName == "Y")
            {
                SetPathData();
                
            }
        }
    }
}
