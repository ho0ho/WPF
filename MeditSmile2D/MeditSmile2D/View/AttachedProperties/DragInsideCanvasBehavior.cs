using MeditSmile2D.View.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MeditSmile2D.View.AttachedProperties
{
    class DragInsideCanvasBehavior
    {
        const int MouseTimeDif = 20;

        #region DragInsideCanvas

        // Property{get;set;} 로 만들지 않는 이유 => DragInsideCanvasBehabior 이 UserControl이 아니기 때문.
        public static bool GetDragInsideCanvas(DependencyObject o)
        {
            return (bool)o.GetValue(DragInsideCanvasProperty);
        }

        public static void SetDragInsideCanvas(DependencyObject o, bool value)
        {
            o.SetValue(DragInsideCanvasProperty, value);
        }

        public static readonly DependencyProperty DragInsideCanvasProperty =
            DependencyProperty.Register("DragInsideCanvas", typeof(bool), typeof(DragInsideCanvasBehavior), new PropertyMetadata(false, OnPropertyChanged));

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var frameworkElement = d as FrameworkElement;
            if (frameworkElement == null) return;

            var canvas = ViewUtils.GetParent(frameworkElement, (t) => t is Canvas) as Canvas;
            if (canvas == null) return;

            var mousePosition = Mouse.GetPosition(canvas);
            int lastMouseMoveTime = Environment.TickCount;
            bool itemIsClicked = false;

            double lastMousePosX = -1;
            double lastMousePosY = -1;
            double x = 0;
            double y = 0;

            // 어떤 컨트롤의 마우스이벤트인지 Dictionary frameworkElement에서 검색
            RoutedEventHandler mouseDown = RegisteredElements.ContainsKey(frameworkElement) ? RegisteredElements[frameworkElement].MouseDown : null;
            RoutedEventHandler mouseUp = RegisteredElements.ContainsKey(frameworkElement) ? RegisteredElements[frameworkElement].MouseUp : null;
            RoutedEventHandler mouseMove = RegisteredElements.ContainsKey(frameworkElement) ? RegisteredElements[frameworkElement].MouseMove : null;

            if ((bool)e.NewValue)
            {
                if (!RegisteredElements.ContainsKey(frameworkElement))
                {
                    /// Register the Element & 's Mouse Event(Down)
                    // Register the Element
                    RegisteredElements.Add(frameworkElement, new HandlersData());

                    // Register the Element's MouseDown Event
                    RegisteredElements[frameworkElement].MouseDown = (_, __) =>
                    {
                        itemIsClicked = true;

                        // always mous down (start dragging), update the x, y
                        // 해당 Element(우리의 Teeth가 될 수 있음)의 canvas 상의 위치(left, top) 뽑기
                        x = (double)frameworkElement.GetValue(Canvas.LeftProperty);
                        y = (double)frameworkElement.GetValue(Canvas.TopProperty);

                        // Validation Check.
                        x = double.IsNaN(x) ? 0 : x;
                        y = double.IsNaN(y) ? 0 : y;

                        mousePosition = Mouse.GetPosition(canvas);
                        lastMousePosX = mousePosition.X;
                        lastMousePosY = mousePosition.Y;
                        Mouse.Capture(frameworkElement);
                        // "Capture Mouse" => When an object captures the mouse, all mouse related events are treated as if the object with mouse capture perform the event, even if the mouse pointer is over another object.
                    };

                    // Register the Element's MouseUp Event
                    RegisteredElements[frameworkElement].MouseUp = (_, __) =>
                    {
                        // Release MouseCapture
                        itemIsClicked = false;
                        Mouse.Capture(null);
                    };

                    // Register the Element's MouseMove Event
                    RegisteredElements[frameworkElement].MouseMove = (_, __) =>
                    {
                        // Dragging...
                        if (itemIsClicked && ((Environment.TickCount - lastMouseMoveTime) > MouseTimeDif))
                        {
                            mousePosition = Mouse.GetPosition(canvas);
                            var containerHeight = (double)canvas.GetValue(FrameworkElement.ActualHeightProperty);
                            var containerWidth = (double)canvas.GetValue(FrameworkElement.ActualWidthProperty);
                            var mouseDiffX = mousePosition.X - lastMousePosX;
                            var mouseDiffY = mousePosition.Y - lastMousePosY;

                            //  if (x + mouseDiffX > 0 && y + mouseDiffY > 0 
                            //     && (containerWidth  <= 0 || (x + mouseDiffX <= containerWidth) && (mousePosition.X <= containerWidth))
                            //     && (containerHeight <= 0 || (y + mouseDiffY <= containerHeight && mousePosition.Y <= containerHeight)))
                            //  {
                            //      x = x + mouseDiffX;
                            //      y = y + mouseDiffY;
                            //      frameworkElement.SetValue(Canvas.LeftProperty, x);
                            //      frameworkElement.SetValue(Canvas.TopProperty, y);
                            //  }

                            if (x + mouseDiffX > 0 && mousePosition.X >= 0 && (containerWidth <= 0 || (x + mouseDiffX <= containerWidth) && (mousePosition.X <= containerWidth)))
                            {
                                x += mouseDiffX;
                                frameworkElement.SetValue(Canvas.LeftProperty, x);
                            }

                            if (y + mouseDiffY > 0 && mousePosition.Y >= 0 && (containerHeight <= 0 || (y + mouseDiffY <= containerHeight) && (mousePosition.Y <= containerHeight)))
                            {
                                y += mouseDiffY;
                                frameworkElement.SetValue(Canvas.TopProperty, y);
                            }

                            lastMouseMoveTime = Environment.TickCount;
                            lastMousePosX = mousePosition.X;
                            lastMousePosY = mousePosition.Y;
                        }
                    };

                    frameworkElement.AddHandler(Mouse.MouseDownEvent, RegisteredElements[frameworkElement].MouseDown, true);
                    frameworkElement.AddHandler(Mouse.MouseDownEvent, RegisteredElements[frameworkElement].MouseDown, true);
                    frameworkElement.AddHandler(Mouse.MouseDownEvent, RegisteredElements[frameworkElement].MouseDown, true);
                }
            }
            // e.NewValue is null
            else
            {
                if (mouseDown != null)
                    frameworkElement.RemoveHandler(Mouse.MouseDownEvent, mouseDown);
                if (mouseUp != null)
                    frameworkElement.RemoveHandler(Mouse.MouseUpEvent, mouseUp);
                if (mouseMove != null)
                    frameworkElement.RemoveHandler(Mouse.MouseMoveEvent, mouseMove);
                if(RegisteredElements.ContainsKey(frameworkElement))
                    RegisteredElements.Remove(frameworkElement);
            }            
        }

        #endregion

        #region RegisteredElements

        private static Dictionary<FrameworkElement, HandlersData> _registeredElements;
        private static Dictionary<FrameworkElement, HandlersData> RegisteredElements
        {
            get { return _registeredElements ?? (_registeredElements = new Dictionary<FrameworkElement, HandlersData>()); }
            set { _registeredElements = value; }
        }

        #endregion

        private class HandlersData
        {
            public RoutedEventHandler MouseDown { get; set; }
            public RoutedEventHandler MouseUp { get; set; }
            public RoutedEventHandler MouseMove { get; set; }
        }
    }
} 
