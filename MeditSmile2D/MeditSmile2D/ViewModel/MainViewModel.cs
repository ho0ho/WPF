using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MeditSmile2D.View;
using MeditSmile2D.View.Utils;
using MeditSmile2D.ViewModel.Command;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MeditSmile2D.ViewModel
{
    
    using ToothType = ObservableCollection<ObservableCollection<PointViewModel>>;

    public class MainViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public FaceDetector.FacePoint facePoint;       

        Point guideMark = new Point(400, 200);

        MainWindow main;
        public MainViewModel()
        {
            main = Application.Current.MainWindow as MainWindow;
            dlgOpen = new OpenFileDialog();

            facePoint = new FaceDetector.FacePoint();
            SelectedTemplates = new List<bool>();

            OpenFileClicked = new MainViewCommand(openFile);
            RedoCommand = new MainViewCommand(redo);
            UndoCommand = new MainViewCommand(undo);

            UndoStack = new Stack<Path>();
            RedoStack = new Stack<Path>();

            captured_teeth = false;
            captured_face = false;

            isSizing = false;
            isFirstTimeMovedOnSizing = true;

            SelectedTemplates.Add(IsTemplate0 = false);
            SelectedTemplates.Add(IsTemplate1 = false);
            SelectedTemplates.Add(IsTemplate2 = false);
            SelectedTemplates.Add(IsTemplate3 = false);
            SelectedTemplates.Add(IsTemplate4 = false);
        }

        #region Title

        private string _Title = "Medit Smile 2D - Tooth Template";
        private const string TitleName = "Title";
        public string Title
        {
            get { return _Title; }
            set
            {
                if (_Title != value)
                {
                    _Title = value;
                    RaisePropertyChanged(TitleName);
                }
            }
        }

        #endregion

        #region SelectedTemplate

        private List<bool> SelectedTemplates;

        private bool _IsTemplate0;
        public bool IsTemplate0
        {
            get { return _IsTemplate0; }
            set
            {
                if (_IsTemplate0 != value)
                {
                    
                    _IsTemplate0 = value;
                    SelectedTemplates[0] = value;
                    if (value == false)
                        flag = false;
                    RaisePropertyChanged("IsTemplate0");
                    RaisePropertyChanged("Points");
                }
            }
        }

        private bool _IsTemplate1;
        private string IsTemplate1Name = "IsTemplate1";
        public bool IsTemplate1
        {
            get { return _IsTemplate1; }
            set
            {
                if (_IsTemplate1 != value)
                {
                    _IsTemplate1 = value;
                    SelectedTemplates[1] = value;
                    if (value == false)
                        flag = false;
                    RaisePropertyChanged("Points");
                    RaisePropertyChanged(IsTemplate1Name);
                }
            }
        }

        private bool _IsTemplate2;
        private string IsTemplate2Name = "IsTemplate2";
        public bool IsTemplate2
        {
            get { return _IsTemplate2; }
            set
            {
                if (_IsTemplate2 != value)
                {
                    _IsTemplate2 = value;
                    SelectedTemplates[2] = value;
                    if (value == false)
                        flag = false;
                    RaisePropertyChanged(IsTemplate2Name);
                    RaisePropertyChanged("Points");
                }
            }
        }

        private bool _IsTemplate3;
        private string IsTemplate3Name = "IsTemplate3";
        public bool IsTemplate3
        {
            get { return _IsTemplate3; }
            set
            {
                if (_IsTemplate3 != value)
                {
                    _IsTemplate3 = value;
                    SelectedTemplates[3] = value;
                    if (value == false)
                        flag = false;
                    RaisePropertyChanged(IsTemplate3Name);
                    RaisePropertyChanged("Points");
                }
            }
        }
        private bool _IsTemplate4;
        private string IsTemplate4Name = "IsTemplate4";
        public bool IsTemplate4
        {
            get { return _IsTemplate4; }
            set
            {
                if (_IsTemplate4 != value)
                {
                    _IsTemplate4 = value;
                    SelectedTemplates[4] = value;
                    if (value == false)
                        flag = false;
                    RaisePropertyChanged(IsTemplate4Name);
                    RaisePropertyChanged("Points");
                }
            }
        }

        #endregion

        #region Points

        private ToothType _Points;
        public ToothType Points
        {
            get { return _Points = GetAllPoints(); }
        }

        private bool flag = true;
        private ToothType GetAllPoints()
        {
            int idx = Idx_Templates();
            if (idx < 0)
                return null;
            return main.templates[idx];
        }

        private int Idx_Templates()
        {
            int indexOfTemplates = -1;
            foreach (bool selected in SelectedTemplates)
            {
                if (selected)
                {
                    indexOfTemplates = SelectedTemplates.IndexOf(selected);
                    break;
                }
            }
            return indexOfTemplates;
        }

        #endregion

        #region IsClosedCurve

        private const string IsClosedCurveName = "IsClosedCurve";
        public bool IsClosedCurve
        {
            get { return true; }

            #region IsClosedCurve 여부

            //get { return _IsClosedCurve1; }
            //set
            //{
            //    if (_IsClosedCurve1 != value)
            //    {
            //        _IsClosedCurve1 = value;
            //        RaisePropertyChanged(IsClosedCurveName1);
            //    }
            //}

            #endregion
        }

        #endregion

        #region Image Load

        public ImageSource Source
        {
            get { return orgImage; }
        }

        BitmapImage orgImage;
        OpenFileDialog dlgOpen = new OpenFileDialog();
        
        // Command for OpenFile
        public MainViewCommand OpenFileClicked { get; set; }
        private void openFile()
        {
            dlgOpen.InitialDirectory = "D:\\ho-ho\\WPF_\\MeditSmile2D\\MeditSmile2D\\images\\";
            dlgOpen.Title = _Title + " - 이미지 불러오기";
            dlgOpen.DefaultExt = ".bmp|.jpg|.jpeg|.png";
            dlgOpen.Filter = "Image Files (*.bmp, *.jpg, *.jpeg, *.png) | *.bmp; *.jpg; *jpeg; *.png";

            if (dlgOpen.ShowDialog() == true)
            {
                FaceDetector faceDetector = new FaceDetector(dlgOpen.FileName);
                orgImage = faceDetector.faceImg;

                RaisePropertyChanged("Source");
                this.facePoint = faceDetector.facePoint;

                DrawFaceLine();
            }
        }

        #region FaceInfo(mark)

        EllipseGeometry _eye_L = new EllipseGeometry();
        public EllipseGeometry EyeL
        {
            get { return _eye_L; }
        }

        EllipseGeometry _eye_R = new EllipseGeometry();
        public EllipseGeometry EyeR
        {
            get { return _eye_R; }
        }

        EllipseGeometry _mouth_L = new EllipseGeometry();
        public EllipseGeometry MouthL
        {
            get { return _mouth_L; }
        }

        EllipseGeometry _mouth_R = new EllipseGeometry();
        public EllipseGeometry MouthR
        {
            get { return _mouth_R; }
        }

        #endregion

        #region FaceInfo(line)
        LineGeometry _midline = new LineGeometry();
        public LineGeometry MidLine
        {
            get { return _midline; }
        }

        LineGeometry _noseline_L = new LineGeometry();
        public LineGeometry NoseLineL
        {
            get { return _noseline_L; }
        }

        LineGeometry _noseline_R = new LineGeometry();
        public LineGeometry NoseLineR
        {
            get { return _noseline_R; }
        }

        LineGeometry _eyeline = new LineGeometry();
        public LineGeometry EyeLine
        {
            get { return _eyeline; }
        }

        LineGeometry _lipline = new LineGeometry();
        public LineGeometry LipLine
        {
            get { return _lipline; }
        }
        #endregion

        DrawFaceAlign drawFaceAlign;
        public void DrawFaceLine()
        {
            ((MainWindow)Application.Current.MainWindow).UpdateLayout();
            double height = ((MainWindow)Application.Current.MainWindow).dental_img.ActualHeight;

            drawFaceAlign = new DrawFaceAlign(facePoint, height);

            // mark
            _eye_L = drawFaceAlign.eyeL;
            _eye_R = drawFaceAlign.eyeR;            
            //nose
            _mouth_L = drawFaceAlign.mouthEndL;
            _mouth_R = drawFaceAlign.mouthEndR;

            // line
            _midline = drawFaceAlign.midline;
            _noseline_L = drawFaceAlign.noselineL;
            _noseline_R = drawFaceAlign.noselineR;
            _eyeline = drawFaceAlign.eyeline;
            _lipline = drawFaceAlign.lipline;

            RaisePropertyChanged("EyeL");
            RaisePropertyChanged("EyeR");
            RaisePropertyChanged("MouthL");
            RaisePropertyChanged("MouthR");
            
            RaisePropertyChanged("MidLine");
            RaisePropertyChanged("NoseLineL");
            RaisePropertyChanged("NoseLineR");
            RaisePropertyChanged("EyeLine");
            RaisePropertyChanged("LipLine");

            GetGuideMark();
            var data = ((MainWindow)Application.Current.MainWindow).templates;
            for(int a = 0; a < 5; a++)
            {
                for(int b = 0; b < 6; b++)
                {
                    for (int c = 0; c < 10; c++)
                    {
                        data[a][b][c].X += guideMark.X;
                        data[a][b][c].Y += guideMark.Y;
                    }
                }
            }
        }

        public void GetGuideMark()
        {
            Point midStart = _midline.StartPoint;
            Point midEnd = _midline.EndPoint;
            Point lipStart = _lipline.StartPoint;
            Point lipEnd = _lipline.EndPoint;

            double _slope_mid = (midStart.Y - midEnd.Y) / (midStart.X - midEnd.X);
            double b_mid = -1.0 * (_slope_mid * midStart.X) + midStart.Y;

            double _slope_lip = (lipStart.Y - lipEnd.Y) / (lipStart.X - lipEnd.X);
            double b_lip = -1.0 * (_slope_lip * lipStart.X) + lipStart.Y;

            guideMark.X = (b_lip - b_mid) / (_slope_mid - _slope_lip);
            guideMark.Y = guideMark.X * _slope_mid + b_mid;
        }

        #endregion

        #region Redo & Undo

        Stack<Path> UndoStack, RedoStack;

        public MainViewCommand RedoCommand { get; set; }
        private void redo()
        {
            if (RedoStack.Count <= 0)
                return;

            Path redoPath = RedoStack.Pop();
            // for mark
            if (redoPath.Name.Equals("EyeL"))
            {
                EyeL.Center = (redoPath.Data as EllipseGeometry).Center;
                EyeLine.StartPoint = EyeL.Center;
            }
            else if (redoPath.Name.Equals("EyeR"))
            {
                EyeR.Center = (redoPath.Data as EllipseGeometry).Center;
                EyeLine.EndPoint = EyeR.Center;
            }
            else if (redoPath.Name.Equals("MouthL"))
            {
                MouthL.Center = (redoPath.Data as EllipseGeometry).Center;
                LipLine.StartPoint = MouthL.Center;
            }
            else if (redoPath.Name.Equals("MouthR"))
            {
                MouthR.Center = (redoPath.Data as EllipseGeometry).Center;
                LipLine.EndPoint = MouthR.Center;
            }

            // for line
            else if (redoPath.Name.Equals("MidLine"))
            {
                MidLine.StartPoint = (redoPath.Data as LineGeometry).StartPoint;
                MidLine.EndPoint = (redoPath.Data as LineGeometry).EndPoint;
            }
            else if (redoPath.Name.Equals("NoseLineL"))
            {
                NoseLineL.StartPoint = (redoPath.Data as LineGeometry).StartPoint;
                NoseLineL.EndPoint = (redoPath.Data as LineGeometry).EndPoint;
            }
            else if (redoPath.Name.Equals("NoseLineR"))
            {
                NoseLineR.StartPoint = (redoPath.Data as LineGeometry).StartPoint;
                NoseLineR.EndPoint = (redoPath.Data as LineGeometry).EndPoint;
            }

            UndoStack.Push(redoPath);
        }

        public MainViewCommand UndoCommand { get; set; }
        private void undo()
        {
            if (UndoStack.Count <= 0)
                return;

            Path undoPath = UndoStack.Pop();
            // for mark
            if (undoPath.Name.Equals("EyeL"))
            {
                EyeL.Center = (undoPath.Data as EllipseGeometry).Center;
                EyeLine.StartPoint = EyeL.Center;
            }
            else if (undoPath.Name.Equals("EyeR"))
            {
                EyeR.Center = (undoPath.Data as EllipseGeometry).Center;
                EyeLine.EndPoint = EyeR.Center;
            }
            else if (undoPath.Name.Equals("MouthL"))
            {
                MouthL.Center = (undoPath.Data as EllipseGeometry).Center;
                LipLine.StartPoint = MouthL.Center;
            }
            else if (undoPath.Name.Equals("MouthR"))
            {
                MouthR.Center = (undoPath.Data as EllipseGeometry).Center;
                LipLine.EndPoint = MouthR.Center;
            }

            // for line
            else if (undoPath.Name.Equals("MidLine"))
            {
                MidLine.StartPoint = (undoPath.Data as LineGeometry).StartPoint;
                MidLine.EndPoint = (undoPath.Data as LineGeometry).EndPoint;
            }
            else if (undoPath.Name.Equals("NoseLineL"))
            {
                NoseLineL.StartPoint = (undoPath.Data as LineGeometry).StartPoint;
                NoseLineL.EndPoint = (undoPath.Data as LineGeometry).EndPoint;
            }
            else if (undoPath.Name.Equals("NoseLineR"))
            {
                NoseLineR.StartPoint = (undoPath.Data as LineGeometry).StartPoint;
                NoseLineR.EndPoint = (undoPath.Data as LineGeometry).EndPoint;
            }

            RedoStack.Push(undoPath);
        }

        #endregion

        #region DragDrop for Face Align

        private bool captured_face;
        private double originalX;

        // Command - MouseMove For FaceAlign
        private RelayCommand<object> _mouseLeftDownForFaceAlign;
        public RelayCommand<object> MouseLeftDownForFaceAlign
        {
            get
            {
                if (_mouseLeftDownForFaceAlign == null)
                    return _mouseLeftDownForFaceAlign
                        = new RelayCommand<object>(param => ExecuteMouseLeftDownForFaceAlign((MouseEventArgs)param));
                return _mouseLeftDownForFaceAlign;
            }
            set { _mouseLeftDownForFaceAlign = value; }
        }

        private void ExecuteMouseLeftDownForFaceAlign(MouseEventArgs e)
        {
            Path originalPath = e.Source as Path;
            IInputElement i = e.Source as IInputElement;

            // Save(for Undo)
            Path savePath = new Path();
            savePath.Name = originalPath.Name;
            savePath.Data = originalPath.Data.CloneCurrentValue();

            // 
            captured_face = true;
            originalX = e.GetPosition(i).X;
            Mouse.Capture(i);

            if (originalPath.Data.GetType() == Type.GetType(originalPath.Name))
                originalPath.Stroke = Brushes.OrangeRed;
            else
                originalPath.Stroke = Brushes.Violet;
        }

        private RelayCommand<object> _mouseMoveForFaceAlign;
        public RelayCommand<object> MouseMoveForFaceAlign
        {
            get
            {
                if (_mouseMoveForFaceAlign == null)
                    return _mouseMoveForFaceAlign
                        = new RelayCommand<object>(param => ExecuteMouseMoveForFaceAlign((MouseEventArgs)param));
                return _mouseMoveForFaceAlign;
            }
            set { _mouseMoveForFaceAlign = value; }
        }

        private void ExecuteMouseMoveForFaceAlign(MouseEventArgs e)
        {
            if (captured_face == true)
            {
                Path p = e.Source as Path;
                IInputElement i = e.Source as IInputElement;

                // for mark
                if (p.Data == EyeL)
                {
                    _eye_L.Center = e.GetPosition(i);
                    RaisePropertyChanged("EyeL");
                    _eyeline.StartPoint = _eye_L.Center;
                    RaisePropertyChanged("EyeLine");
                }
                else if (p.Data == EyeR)
                {
                    _eye_R.Center = e.GetPosition(i);
                    RaisePropertyChanged("EyeR");
                    _eyeline.EndPoint = _eye_R.Center;
                    RaisePropertyChanged("EyeLine");
                }
                else if (p.Data == MouthL)
                {
                    _mouth_L.Center = e.GetPosition(i);
                    RaisePropertyChanged("MouthL");
                    _lipline.StartPoint = _mouth_L.Center;
                    RaisePropertyChanged("LipLine");
                }
                else if (p.Data == MouthR)
                {
                    _mouth_R.Center = e.GetPosition(i);
                    RaisePropertyChanged("MouthR");
                    _lipline.EndPoint = _mouth_R.Center;
                    RaisePropertyChanged("LipLine");
                }
                // for line - 좌우로만 움직임 가능
                else if (p.Data == MidLine)
                {
                    double dx = e.GetPosition(i).X - originalX;
                    _midline.StartPoint = new Point(_midline.StartPoint.X + dx, _midline.StartPoint.Y);
                    _midline.EndPoint = new Point(_midline.EndPoint.X + dx, _midline.EndPoint.Y);
                    RaisePropertyChanged("MidLine");

                    originalX = e.GetPosition(i).X;
                }
                else if (p.Data == NoseLineL)
                {
                    double dx = e.GetPosition(i).X - originalX;
                    _noseline_L.StartPoint = new Point(_noseline_L.StartPoint.X + dx, _noseline_L.StartPoint.Y);
                    _noseline_L.EndPoint = new Point(_noseline_L.EndPoint.X + dx, _noseline_L.EndPoint.Y);
                    RaisePropertyChanged("NoseLineL");

                    originalX = e.GetPosition(i).X;
                }
                else if (p.Data == NoseLineR)
                {
                    double dx = e.GetPosition(i).X - originalX;
                    _noseline_R.StartPoint = new Point(_noseline_R.StartPoint.X + dx, _noseline_R.StartPoint.Y);
                    _noseline_R.EndPoint = new Point(_noseline_R.EndPoint.X + dx, _noseline_R.EndPoint.Y);
                    RaisePropertyChanged("NoseLineR");

                    originalX = e.GetPosition(i).X;
                }
            }

        }

        private RelayCommand<object> _mouseLeftUpForFaceAlign;
        public RelayCommand<object> MouseLeftUpForFaceAlign
        {
            get
            {
                if (_mouseLeftUpForFaceAlign == null)
                    return _mouseLeftUpForFaceAlign
                         = new RelayCommand<object>(param => ExecuteMouseLeftUpForFaceAlign((MouseEventArgs)param));
                return _mouseLeftUpForFaceAlign;
            }
            set { _mouseLeftUpForFaceAlign = value; }
        }

        private void ExecuteMouseLeftUpForFaceAlign(MouseEventArgs e)
        {
            Path moved = e.Source as Path;

            captured_face = false;
            if (moved.Data.GetType() == Type.GetType(moved.Name))
                moved.Stroke = Brushes.DeepSkyBlue;
            else
                moved.Stroke = Brushes.Black;
            Mouse.Capture(null);
        }

        #endregion

        #region DragDrop for Teeth 

        private bool captured_teeth;
        private Point originalPoint;

        private RelayCommand<object> _mouseLeftDownForDragAndDropTeeth;
        public RelayCommand<object> MouseLeftDownForDragAndDropTeeth
        {
            get
            {
                if (_mouseLeftDownForDragAndDropTeeth == null)
                    return _mouseLeftDownForDragAndDropTeeth = new RelayCommand<object>(param => ExecuteMouseLeftDownForDragAndDropTeeth((MouseEventArgs)param));
                return _mouseLeftDownForDragAndDropTeeth;
            }
            set { _mouseLeftDownForDragAndDropTeeth = value; }
        }
        public void ExecuteMouseLeftDownForDragAndDropTeeth(MouseEventArgs e)
        {
            Rectangle rect = e.Source as Rectangle;
            //Grid grid = (Grid)rect.Parent;
            //Border border = (Border)grid.Parent;
            //WrapTeeth teeth = (WrapTeeth)border.Parent;
            originalPoint = e.GetPosition((IInputElement)e.Source);

            // padding
            originalPoint.X += 5;
            originalPoint.Y += 5;

            captured_teeth = true;
            Mouse.Capture((IInputElement)e.Source);
        }

        private RelayCommand<object> _mouseMoveForDragAndDropTeeth;
        public RelayCommand<object> MouseMoveForDragAndDropTeeth
        {
            get
            {
                if (_mouseMoveForDragAndDropTeeth == null)
                    return _mouseMoveForDragAndDropTeeth = new RelayCommand<object>(param => ExecuteMouseMoveForDragAndDropTeeth((MouseEventArgs)param));
                return _mouseMoveForDragAndDropTeeth;
            }
            set { _mouseMoveForDragAndDropTeeth = value; }
        }

        public void ExecuteMouseMoveForDragAndDropTeeth(MouseEventArgs e)
        {
            Rectangle rect = (Rectangle)e.Source;
            Grid grid = (Grid)rect.Parent;
            Border border = (Border)grid.Parent;
            WrapTeeth wrap = (WrapTeeth)border.Parent;

            if (captured_teeth == true)
            {
                Point curPoint = e.GetPosition((IInputElement)e.Source);
                var dragDelta = curPoint - originalPoint;

                foreach (PointViewModel point in wrap.Points)
                {
                    point.X += dragDelta.X;
                    point.Y += dragDelta.Y;
                }
            }
        }

        private RelayCommand<object> _mouseLeftUpForDragAndDropTeeth;
        public RelayCommand<object> MouseLeftUpForDragAndDropTeeth
        {
            get
            {
                if (_mouseLeftUpForDragAndDropTeeth == null)
                    return _mouseLeftUpForDragAndDropTeeth = new RelayCommand<object>(param => ExecuteMouseLeftUpForDragAndDropTeeth((MouseEventArgs)param));
                return _mouseLeftUpForDragAndDropTeeth;
            }
            set { _mouseLeftUpForDragAndDropTeeth = value; }
        }

        public void ExecuteMouseLeftUpForDragAndDropTeeth(MouseEventArgs e)
        {
            captured_teeth = false;
            Mouse.Capture(null);
        }

        #endregion

        #region Resize for Teeth


        private bool isSizing;
        private Point anchorPoint;

        private bool isFirstTimeMovedOnSizing;
        private readonly double sizeThreshold = 50;

        private Point anchorMin, anchorMax;

        private void SetSizing(MouseEventArgs e)
        {
            if (!isSizing)
                return;

            Border border = (Border)e.Source;
            Grid grid = (Grid)border.Parent;
            Border borderSecond = (Border)grid.Parent;
            WrapTeeth wrapTeeth = (WrapTeeth)borderSecond.Parent;

            Point maxPoint = GetMaxPoint(wrapTeeth);
            Point minPoint = GetMinPoint(wrapTeeth);

            if (isFirstTimeMovedOnSizing)
            {
                anchorMin = new Point(minPoint.X, minPoint.Y);
                anchorMax = new Point(maxPoint.X, maxPoint.Y);
                isFirstTimeMovedOnSizing = false;
            }

            if (e.LeftButton != MouseButtonState.Pressed)
                return;

            double actualWidth1 = minPoint.X + wrapTeeth.ActualWidth;
            double actualHeight1 = minPoint.Y + wrapTeeth.ActualHeight;
            double actualWidth2 = maxPoint.X - wrapTeeth.ActualWidth;
            double actualHeight2 = maxPoint.X - wrapTeeth.ActualHeight;

            Point curPoint = e.GetPosition((IInputElement)e.Source);
            double dx = Math.Abs((actualWidth1 + curPoint.X) - anchorMin.X);
            double dy = Math.Abs((actualHeight1 + curPoint.Y) - anchorMin.Y);

            if (border.Name.Equals("Border_Top"))
            {
                foreach (PointViewModel point in wrapTeeth.Points)
                {
                    point.Y = point.Y * ((actualHeight2 - curPoint.Y) / actualHeight2);
                }
            }
            else if (border.Name.Equals("Border_Bottom"))
            {
                foreach (PointViewModel point in wrapTeeth.Points)
                {
                    point.Y = point.Y * ((actualHeight1 + curPoint.Y) / actualHeight1) - (minPoint.Y - anchorMin.Y);
                }
            }
            else if (border.Name.Equals("Border_Left"))
            {
                foreach (PointViewModel point in wrapTeeth.Points)
                {
                    point.X = point.X * ((actualWidth2 - curPoint.X) / actualWidth2);
                }
            }
            else if (border.Name.Equals("Border_Right"))
            {
                foreach (PointViewModel point in wrapTeeth.Points)
                {
                    point.X = point.X * ((actualWidth1 + curPoint.X) / actualWidth1) - (minPoint.X - anchorMin.X);
                }
            }
            else if (border.Name.Equals("Border_TopLeft"))
            {
                foreach (PointViewModel point in wrapTeeth.Points)
                {
                    point.X = point.X * ((actualWidth2 - curPoint.X) / actualWidth2);
                    point.Y = point.Y * ((actualHeight2 - curPoint.Y) / actualHeight2);
                }
            }
            else if (border.Name.Equals("Border_TopRight"))
            {
                foreach (PointViewModel point in wrapTeeth.Points)
                {
                    point.X = point.X * ((actualWidth2 + curPoint.X) / actualWidth2);
                    point.Y = point.Y * ((actualHeight2 - curPoint.Y) / actualHeight2);
                }
            }
            else if (border.Name.Equals("Border_BottomLeft"))
            {
                foreach (PointViewModel point in wrapTeeth.Points)
                {
                    point.X = point.X * ((actualWidth2 - curPoint.X) / actualWidth2);
                    point.Y = point.Y * ((actualHeight2 + curPoint.Y) / actualHeight2);
                }
            }
            else if (border.Name.Equals("Border_BottomRight"))
            {
                foreach (PointViewModel point in wrapTeeth.Points)
                {
                    point.X = point.X * ((actualWidth1 + curPoint.X) / actualWidth1) - (minPoint.X - anchorMin.X);
                    point.Y = point.Y * ((actualHeight1 + curPoint.Y) / actualHeight1) - (minPoint.Y - anchorMin.Y);
                }
            }
        }

        private Point GetMaxPoint(WrapTeeth t)
        {
            double maxX = double.MinValue;
            double maxY = double.MinValue;
            foreach (PointViewModel point in t.Points)
            {
                if (point.X > maxX)
                    maxX = point.X;
                if (point.Y > maxY)
                    maxY = point.Y;
            }
            return new Point(maxX, maxY);
        }

        private Point GetMinPoint(WrapTeeth t)
        {
            double minX = double.MaxValue;
            double minY = double.MaxValue;
            foreach (PointViewModel point in t.Points)
            {
                if (point.X < minX)
                    minX = point.X;
                if (point.Y < minY)
                    minY = point.Y;
            }
            return new Point(minX, minY);
        }


        #region Resize for CommandProperties


        private RelayCommand<object> _mouseLeftDownForResize;
        public RelayCommand<object> MouseLeftDownForResize
        {
            get
            {
                if (_mouseLeftDownForResize == null)
                    return _mouseLeftDownForResize = new RelayCommand<object>(param => ExecuteMouseLeftDownForResize((MouseEventArgs)param));
                return _mouseLeftDownForResize;
            }
            set { _mouseLeftDownForResize = value; }
        }

        public void ExecuteMouseLeftDownForResize(MouseEventArgs e)
        {
            isSizing = true;
            anchorPoint = e.GetPosition((IInputElement)e.Source);
            Mouse.Capture((IInputElement)e.Source);
        }

        private RelayCommand<object> _mouseMoveForResize;
        public RelayCommand<object> MouseMoveForResize
        {
            get
            {
                if (_mouseMoveForResize == null)
                    return _mouseMoveForResize = new RelayCommand<object>(param => ExecuteMouseMoveForResize((MouseEventArgs)param));
                return _mouseMoveForResize;
            }
            set { _mouseMoveForResize = value; }
        }

        public void ExecuteMouseMoveForResize(MouseEventArgs e)
        {
            SetSizing(e);
        }

        private RelayCommand<object> _mouseLeftUpForResize;
        public RelayCommand<object> MouseLeftUpForResize
        {
            get
            {
                if (_mouseLeftUpForResize == null)
                    return _mouseLeftUpForResize = new RelayCommand<object>(param => ExecuteMouseLeftUpForResize((MouseEventArgs)param));
                return _mouseLeftUpForResize;
            }
            set { _mouseLeftUpForResize = value; }
        }

        public void ExecuteMouseLeftUpForResize(MouseEventArgs e)
        {
            isSizing = false;
            Mouse.Capture(null);
            isFirstTimeMovedOnSizing = true;
        }







        

        //private RelayCommand<object> _mouseLeftDownForResizeTop;
        //public RelayCommand<object> MouseLeftDownForResizeTop
        //{
        //    get
        //    {
        //        if (_mouseLeftDownForResizeTop == null)
        //            return _mouseLeftDownForResizeTop = new RelayCommand<object>(param => ExecuteMouseLeftDownForResizeTop((MouseEventArgs)param));
        //        return _mouseLeftDownForResizeTop;
        //    }
        //    set { _mouseLeftDownForResizeTop = value; }
        //}

        //// Resize Top
        //public void ExecuteMouseLeftDownForResizeTop(MouseEventArgs e)
        //{
        //    isSizing = true;
        //    anchorPoint = e.GetPosition((IInputElement)e.Source);
        //    Mouse.Capture((IInputElement)e.Source);
        //}

        //private RelayCommand<object> _mouseMoveForResizeTop;
        //public RelayCommand<object> MouseMoveForResizeTop
        //{
        //    get
        //    {
        //        if (_mouseMoveForResizeTop == null)
        //            return _mouseMoveForResizeTop = new RelayCommand<object>(param => ExecuteMouseMoveForResizeTop((MouseEventArgs)param));
        //        return _mouseMoveForResizeTop;
        //    }
        //    set { _mouseMoveForResizeTop = value; }
        //}

        //public void ExecuteMouseMoveForResizeTop(MouseEventArgs e)
        //{
        //    SetSizing(e);
        //}

        //private RelayCommand<object> _mouseLeftUpForResizeTop;
        //public RelayCommand<object> MouseLeftUpForResizeTop
        //{
        //    get
        //    {
        //        if (_mouseLeftUpForResizeTop == null)
        //            return _mouseLeftUpForResizeTop = new RelayCommand<object>(param => ExecuteMouseLeftUpForResizeTop((MouseEventArgs)param));
        //        return _mouseLeftUpForResizeTop;
        //    }
        //    set { _mouseLeftUpForResizeTop = value; }
        //}

        //public void ExecuteMouseLeftUpForResizeTop(MouseEventArgs e)
        //{
        //    isSizing = false;
        //    Mouse.Capture(null);
        //    isFirstTimeMovedOnSizing = true;
        //}

        //// Resize Bottom
        //private RelayCommand<object> _mouseLeftDownForResizeBottom;
        //public RelayCommand<object> MouseLeftDownForResizeBottom
        //{
        //    get
        //    {
        //        if (_mouseLeftDownForResizeBottom == null)
        //            return _mouseLeftDownForResizeBottom = new RelayCommand<object>(param => ExecuteMouseLeftDownForResizeBottom((MouseEventArgs)param));
        //        return _mouseLeftDownForResizeBottom;
        //    }
        //    set { _mouseLeftDownForResizeBottom = value; }
        //}

        //public void ExecuteMouseLeftDownForResizeBottom(MouseEventArgs e)
        //{

        //}

        //private RelayCommand<object> _mouseMoveForResizeBottom;
        //public RelayCommand<object> MouseMoveForResizeBottom
        //{
        //    get
        //    {
        //        if (_mouseMoveForResizeBottom == null)
        //            return _mouseMoveForResizeBottom = new RelayCommand<object>(param => ExecuteMouseMoveForResizeBottom((MouseEventArgs)param));
        //        return _mouseMoveForResizeBottom;
        //    }
        //    set { _mouseMoveForResizeBottom = value; }
        //}

        //public void ExecuteMouseMoveForResizeBottom(MouseEventArgs e)
        //{

        //}

        //private RelayCommand<object> _mouseLeftUpForResizeBottom;
        //public RelayCommand<object> MouseLeftUpForResizeBottom
        //{
        //    get
        //    {
        //        if (_mouseLeftUpForResizeBottom == null)
        //            return _mouseLeftUpForResizeBottom = new RelayCommand<object>(param => ExecuteMouseLeftUpForResizeBottom((MouseEventArgs)param));
        //        return _mouseLeftUpForResizeBottom;
        //    }
        //    set { _mouseLeftUpForResizeBottom = value; }
        //}

        //public void ExecuteMouseLeftUpForResizeBottom(MouseEventArgs e)
        //{

        //}

        //// Resize Left
        //private RelayCommand<object> _mouseLeftDownForResizeLeft;
        //public RelayCommand<object> MouseLeftDownForResizeLeft
        //{
        //    get
        //    {
        //        if (_mouseLeftDownForResizeLeft == null)
        //            return _mouseLeftDownForResizeLeft = new RelayCommand<object>(param => ExecuteMouseLeftDownForResizeLeft((MouseEventArgs)param));
        //        return _mouseLeftDownForResizeLeft;
        //    }
        //    set { _mouseLeftDownForResizeLeft = value; }
        //}

        //public void ExecuteMouseLeftDownForResizeLeft(MouseEventArgs e)
        //{

        //}

        //private RelayCommand<object> _mouseMoveForResizeLeft;
        //public RelayCommand<object> MouseMoveForResizeLeft
        //{
        //    get
        //    {
        //        if (_mouseMoveForResizeLeft == null)
        //            return _mouseMoveForResizeLeft = new RelayCommand<object>(param => ExecuteMouseMoveForResizeLeft((MouseEventArgs)param));
        //        return _mouseMoveForResizeLeft;
        //    }
        //    set { _mouseMoveForResizeLeft = value; }
        //}

        //public void ExecuteMouseMoveForResizeLeft(MouseEventArgs e)
        //{

        //}

        //private RelayCommand<object> _mouseLeftUpForResizeLeft;
        //public RelayCommand<object> MouseLeftUpForResizeLeft
        //{
        //    get
        //    {
        //        if (_mouseLeftUpForResizeLeft == null)
        //            return _mouseLeftUpForResizeLeft = new RelayCommand<object>(param => ExecuteMouseLeftUpForResizeLeft((MouseEventArgs)param));
        //        return _mouseLeftUpForResizeLeft;
        //    }
        //    set { _mouseLeftUpForResizeLeft = value; }
        //}

        //public void ExecuteMouseLeftUpForResizeLeft(MouseEventArgs e)
        //{

        //}

        //// Resize Right
        //private RelayCommand<object> _mouseLeftDownForResizeRight;
        //public RelayCommand<object> MouseLeftDownForResizeRight
        //{
        //    get
        //    {
        //        if (_mouseLeftDownForResizeRight == null)
        //            return _mouseLeftDownForResizeRight = new RelayCommand<object>(param => ExecuteMouseLeftDownForResizeRight((MouseEventArgs)param));
        //        return _mouseLeftDownForResizeRight;
        //    }
        //    set { _mouseLeftDownForResizeLeft = value; }
        //}

        //public void ExecuteMouseLeftDownForResizeRight(MouseEventArgs e)
        //{

        //}

        //private RelayCommand<object> _mouseMoveForResizeRight;
        //public RelayCommand<object> MouseMoveForResizeRight
        //{
        //    get
        //    {
        //        if (_mouseMoveForResizeRight == null)
        //            return _mouseMoveForResizeRight = new RelayCommand<object>(param => ExecuteMouseMoveForResizeRight((MouseEventArgs)param));
        //        return _mouseMoveForResizeRight;
        //    }
        //    set { _mouseMoveForResizeRight = value; }
        //}

        //public void ExecuteMouseMoveForResizeRight(MouseEventArgs e)
        //{

        //}

        //private RelayCommand<object> _mouseLeftUpForResizeRight;
        //public RelayCommand<object> MouseLeftUpForResizeRight
        //{
        //    get
        //    {
        //        if (_mouseLeftUpForResizeRight == null)
        //            return _mouseLeftUpForResizeRight = new RelayCommand<object>(param => ExecuteMouseLeftUpForResizeRight((MouseEventArgs)param));
        //        return _mouseLeftUpForResizeRight;
        //    }
        //    set { _mouseLeftUpForResizeRight = value; }
        //}

        //public void ExecuteMouseLeftUpForResizeRight(MouseEventArgs e)
        //{

        //}

        //// Resize TopLeft
        //private RelayCommand<object> _mouseLeftDownForResizeTopLeft;
        //public RelayCommand<object> MouseLeftDownForResizeTopLeft
        //{
        //    get
        //    {
        //        if (_mouseLeftDownForResizeTopLeft == null)
        //            return _mouseLeftDownForResizeTopLeft = new RelayCommand<object>(param => ExecuteMouseLeftDownForResizeTopLeft((MouseEventArgs)param));
        //        return _mouseLeftDownForResizeTopLeft;
        //    }
        //    set { _mouseLeftDownForResizeTopLeft = value; }
        //}

        //public void ExecuteMouseLeftDownForResizeTopLeft(MouseEventArgs e)
        //{

        //}

        //private RelayCommand<object> _mouseMoveForResizeTopLeft;
        //public RelayCommand<object> MouseMoveForResizeTopLeft
        //{
        //    get
        //    {
        //        if (_mouseMoveForResizeTopLeft == null)
        //            return _mouseMoveForResizeTopLeft = new RelayCommand<object>(param => ExecuteMouseMoveForResizeTopLeft((MouseEventArgs)param));
        //        return _mouseMoveForResizeTopLeft;
        //    }
        //    set { _mouseMoveForResizeTopLeft = value; }
        //}

        //public void ExecuteMouseMoveForResizeTopLeft(MouseEventArgs e)
        //{

        //}

        //private RelayCommand<object> _mouseLeftUpForResizeTopLeft;
        //public RelayCommand<object> MouseLeftUpForResizeTopLeft
        //{
        //    get
        //    {
        //        if (_mouseLeftUpForResizeTopLeft == null)
        //            return _mouseLeftUpForResizeTopLeft = new RelayCommand<object>(param => ExecuteMouseLeftUpForResizeTopLeft((MouseEventArgs)param));
        //        return _mouseLeftUpForResizeTopLeft;
        //    }
        //    set { _mouseLeftUpForResizeTopLeft = value; }
        //}

        //public void ExecuteMouseLeftUpForResizeTopLeft(MouseEventArgs e)
        //{

        //}

        //// Resize TopRight
        //private RelayCommand<object> _mouseLeftDownForResizeTopRight;
        //public RelayCommand<object> MouseLeftDownForResizeTopRight
        //{
        //    get
        //    {
        //        if (_mouseLeftDownForResizeTopRight == null)
        //            return _mouseLeftDownForResizeTopRight = new RelayCommand<object>(param => ExecuteMouseLeftDownForResizeTopRight((MouseEventArgs)param));
        //        return _mouseLeftDownForResizeTopRight;
        //    }
        //    set { _mouseLeftDownForResizeTopRight = value; }
        //}

        //public void ExecuteMouseLeftDownForResizeTopRight(MouseEventArgs e)
        //{

        //}

        //private RelayCommand<object> _mouseMoveForResizeTopRight;
        //public RelayCommand<object> MouseMoveForResizeTopRight
        //{
        //    get
        //    {
        //        if (_mouseMoveForResizeTopRight == null)
        //            return _mouseMoveForResizeTopRight = new RelayCommand<object>(param => ExecuteMouseMoveForResizeTopRight((MouseEventArgs)param));
        //        return _mouseMoveForResizeTopRight;
        //    }
        //    set { _mouseMoveForResizeTopRight = value; }
        //}

        //public void ExecuteMouseMoveForResizeTopRight(MouseEventArgs e)
        //{

        //}

        //private RelayCommand<object> _mouseLeftUpForResizeTopRight;
        //public RelayCommand<object> MouseLeftUpForResizeTopRight
        //{
        //    get
        //    {
        //        if (_mouseLeftUpForResizeTopRight == null)
        //            return _mouseLeftUpForResizeTopRight = new RelayCommand<object>(param => ExecuteMouseLeftUpForResizeTopRight((MouseEventArgs)param));
        //        return _mouseLeftUpForResizeTopRight;
        //    }
        //    set { _mouseLeftUpForResizeTopRight = value; }
        //}

        //public void ExecuteMouseLeftUpForResizeTopRight(MouseEventArgs e)
        //{

        //}

        //// Resize BottomLeft
        //private RelayCommand<object> _mouseLeftDownForResizeBottomLeft;
        //public RelayCommand<object> MouseLeftDownForResizeBottomLeft
        //{
        //    get
        //    {
        //        if (_mouseLeftDownForResizeBottomLeft == null)
        //            return _mouseLeftDownForResizeBottomLeft = new RelayCommand<object>(param => ExecuteMouseLeftDownForResizeBottomLeft((MouseEventArgs)param));
        //        return _mouseLeftDownForResizeBottomLeft;
        //    }
        //    set { _mouseLeftDownForResizeBottomLeft = value; }
        //}

        //public void ExecuteMouseLeftDownForResizeBottomLeft(MouseEventArgs e)
        //{

        //}

        //private RelayCommand<object> _mouseMoveForResizeBottomLeft;
        //public RelayCommand<object> MouseMoveForResizeTopBottomLeft
        //{
        //    get
        //    {
        //        if (_mouseMoveForResizeBottomLeft == null)
        //            return _mouseMoveForResizeBottomLeft = new RelayCommand<object>(param => ExecuteMouseMoveForResizeBottomLeft((MouseEventArgs)param));
        //        return _mouseMoveForResizeBottomLeft;
        //    }
        //    set { _mouseMoveForResizeBottomLeft = value; }
        //}

        //public void ExecuteMouseMoveForResizeBottomLeft(MouseEventArgs e)
        //{

        //}

        //private RelayCommand<object> _mouseLeftUpForResizeBottomLeft;
        //public RelayCommand<object> MouseLeftUpForResizeBottomLeft
        //{
        //    get
        //    {
        //        if (_mouseLeftUpForResizeBottomLeft == null)
        //            return _mouseLeftUpForResizeBottomLeft = new RelayCommand<object>(param => ExecuteMouseLeftUpForResizeBottomLeft((MouseEventArgs)param));
        //        return _mouseLeftUpForResizeBottomLeft;
        //    }
        //    set { _mouseLeftUpForResizeBottomLeft = value; }
        //}

        //public void ExecuteMouseLeftUpForResizeBottomLeft(MouseEventArgs e)
        //{

        //}

        //// Resize BottomRight
        //private RelayCommand<object> _mouseLeftDownForResizeBottomRight;
        //public RelayCommand<object> MouseLeftDownForResizeBottomRight
        //{
        //    get
        //    {
        //        if (_mouseLeftDownForResizeBottomRight == null)
        //            return _mouseLeftDownForResizeBottomRight = new RelayCommand<object>(param => ExecuteMouseLeftDownForResizeBottomRight((MouseEventArgs)param));
        //        return _mouseLeftDownForResizeBottomRight;
        //    }
        //    set { _mouseLeftDownForResizeBottomRight = value; }
        //}

        //public void ExecuteMouseLeftDownForResizeBottomRight(MouseEventArgs e)
        //{

        //}

        //private RelayCommand<object> _mouseMoveForResizeBottomRight;
        //public RelayCommand<object> MouseMoveForResizeTopBottomRight
        //{
        //    get
        //    {
        //        if (_mouseMoveForResizeBottomRight == null)
        //            return _mouseMoveForResizeBottomRight = new RelayCommand<object>(param => ExecuteMouseMoveForResizeBottomRight((MouseEventArgs)param));
        //        return _mouseMoveForResizeBottomRight;
        //    }
        //    set { _mouseMoveForResizeBottomRight = value; }
        //}

        //public void ExecuteMouseMoveForResizeBottomRight(MouseEventArgs e)
        //{

        //}

        //private RelayCommand<object> _mouseLeftUpForResizeBottomRight;
        //public RelayCommand<object> MouseLeftUpForResizeBottomRight
        //{
        //    get
        //    {
        //        if (_mouseLeftUpForResizeBottomRight == null)
        //            return _mouseLeftUpForResizeBottomRight = new RelayCommand<object>(param => ExecuteMouseLeftUpForResizeBottomRight((MouseEventArgs)param));
        //        return _mouseLeftUpForResizeBottomRight;
        //    }
        //    set { _mouseLeftUpForResizeBottomRight = value; }
        //}

        //public void ExecuteMouseLeftUpForResizeBottomRight(MouseEventArgs e)
        //{

        //}

        #endregion

        #endregion
    }
}
