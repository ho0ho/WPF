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
                wrapPoints.SetRectData();
        }

        #endregion


        void SetRectData()
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

            AdjSize(points);
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

            SetRectData();
        }

        private void OnPointPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "X" || e.PropertyName == "Y")
                SetRectData();
        }

        #region size&canvas 좌표

        public double Top;
        public double Left;

        readonly double padding = 10;
        private void AdjSize(List<Point> pts)
        {
            double xMin = float.MaxValue;
            double xMax = float.MinValue;
            double yMin = float.MaxValue;
            double yMax = float.MinValue;

            foreach (var point in pts)
            {
                if (point.X > xMax)
                    xMax = point.X;
                if (point.X < xMin)
                    xMin = point.X;
                if (point.Y > yMax)
                    yMax = point.Y;
                if (point.Y < yMin)
                    yMin = point.Y;
            }
            
            // Grid의 크기 설정
            innerWrap.Height = yMax - yMin + padding;
            innerWrap.Width = xMax - xMin + padding;

            Top = yMin - padding / 2;
            Left = xMin - padding / 2;

            // Grid의 좌표 보정
            var points = new List<Point>();
            foreach (var point in Points)
            {
                var pointProperties = point.GetType().GetProperties();
                if (pointProperties.All(p => p.Name != "X") ||
                pointProperties.All(p => p.Name != "Y"))
                    continue;
                var x = (float)point.GetType().GetProperty("X").GetValue(point, new object[] { }) - Left;
                var y = (float)point.GetType().GetProperty("Y").GetValue(point, new object[] { }) - Top;
                points.Add(new Point(x, y));
            }

            if (points.Count <= 1)
                return;

            Canvas.SetLeft(this, Left);
            Canvas.SetTop(this, Top);
        }

        #endregion

        //#region events

        //private Point origMouseDownPoint;
        //private bool isLeftMouseAndControlDownOnRectangle;
        //private bool isLeftMouseDownOnRectangle;
        //private bool isDraggingRectangle;
        //private readonly double DragThreshold = 5;


        //private void ToothTemplate_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        //{

        //    if (e.ChangedButton != MouseButton.Left)
        //    {
        //        return;
        //    }

        //    var rectangle = (FrameworkElement)sender;

        //    isLeftMouseDownOnRectangle = true;

        //    if ((Keyboard.Modifiers & ModifierKeys.Control) != 0)
        //    {
        //        //
        //        // Control key was held down.
        //        // This means that the rectangle is being added to or removed from the existing selection.
        //        // Don't do anything yet, we will act on this later in the MouseUp event handler.
        //        //
        //        isLeftMouseAndControlDownOnRectangle = true;
        //    }


        //    rectangle.CaptureMouse();
        //    origMouseDownPoint = e.GetPosition(this);

        //    e.Handled = true;
        //}

        //private void ToothTemplate_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        //{
        //    MessageBox.Show("눌림ㅋ");

        //    if (isLeftMouseDownOnRectangle)
        //    {
        //        var rectangle = (FrameworkElement)sender;

        //        if (!isDraggingRectangle)
        //        {
        //            //
        //            // Execute mouse up selection logic only if there was no drag operation.
        //            //

        //        }
        //        else
        //        {
        //            //
        //            // Control key was not held down.
        //            //

        //        }


        //        rectangle.ReleaseMouseCapture();
        //        isLeftMouseDownOnRectangle = false;
        //        isLeftMouseAndControlDownOnRectangle = false;

        //        e.Handled = true;
        //    }

        //    isDraggingRectangle = false;
        //}

        //private void ToothTemplate_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        //{
        //    if (isDraggingRectangle)
        //    {
        //        //
        //        // Drag-move selected rectangles.
        //        //
        //        Point curMouseDownPoint = e.GetPosition(this);
        //        var dragDelta = curMouseDownPoint - origMouseDownPoint;

        //        origMouseDownPoint = curMouseDownPoint;


        //    }
        //    else if (isLeftMouseDownOnRectangle)
        //    {
        //        //
        //        // The user is left-dragging the rectangle,
        //        // but don't initiate the drag operation until
        //        // the mouse cursor has moved more than the threshold value.
        //        //
        //        Point curMouseDownPoint = e.GetPosition(this);
        //        var dragDelta = curMouseDownPoint - origMouseDownPoint;
        //        double dragDistance = System.Math.Abs(dragDelta.Length);
        //        if (dragDistance > DragThreshold)
        //        {
        //            //
        //            // When the mouse has been dragged more than the threshold value commence dragging the rectangle.
        //            //
        //            isDraggingRectangle = true;
        //        }

        //        e.Handled = true;
        //    }
        //}

        //#endregion
    }
}
