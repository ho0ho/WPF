﻿using MeditSmile2D.View.Utils;
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
            var teeth = d as Teeth;
            if (teeth == null) return;

            if (e.NewValue is INotifyCollectionChanged)
            {
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
            DependencyProperty.Register("PathColor", typeof(Brush), typeof(Teeth), new PropertyMetadata(Brushes.Red));

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
            if (Points == null) return;
            var points = new List<Point>();

            foreach (var point in Points)
            {
                var pointProperties = point.GetType().GetProperties();
                if (pointProperties.All(p => p.Name != "X") || pointProperties.All(p => p.Name != "Y"))
                    continue;
                var x = (float)point.GetType().GetProperty("X").GetValue(point, new object[] { });
                var y = (float)point.GetType().GetProperty("X").GetValue(point, new object[] { });
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

        private void OnPointPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "X" || e.PropertyName == "Y")
                SetPathData();
        }
    }
}