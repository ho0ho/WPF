using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MeditSmile2D.Common;
using MeditSmile2D.View;
using MeditSmile2D.View.Utils;
using MeditSmile2D.ViewModel.Command;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MeditSmile2D.ViewModel
{
    using ToothList = ObservableCollection<ObservableCollection<ObservableCollection<PointViewModel>>>;
    using ToothType = ObservableCollection<ObservableCollection<PointViewModel>>;
    using TeethType = ObservableCollection<PointViewModel>;

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

            OpenFileClicked = new MainViewCommand(openFile);
            RedoCommand = new MainViewCommand(redo);
            UndoCommand = new MainViewCommand(undo);

            UndoStack = new Stack<Path>();
            RedoStack = new Stack<Path>();

            captured = false;
            captured_face = false;

            isSizing = false;
            isFirstTimeMovedOnSizing = true;
            //anchor = new Point();

            UpperToothList = new List<bool>();
            UpperToothList.Add(UpperTmpNo = false);
            UpperToothList.Add(UpperTmp1 = false);
            UpperToothList.Add(UpperTmp2 = false);
            UpperToothList.Add(UpperTmp3 = false);
            UpperToothList.Add(UpperTmp4 = false);
            UpperToothList.Add(UpperTmp5 = false);

            LowerToothList = new List<bool>();
            LowerToothList.Add(LowerTmpNo = false);
            LowerToothList.Add(LowerTmp1 = false);
            //LowerToothList.Add( = false);
            //LowerToothList.Add( = false);
            //LowerToothList.Add( = false);
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

        #region ToothSelection

        // Upper Tooth
        private List<bool> UpperToothList;

        private bool _UpperTmpNo;
        public bool UpperTmpNo
        {
            get { return _UpperTmpNo; }
            set
            {
                if (_UpperTmpNo != value)
                {
                    _UpperTmpNo = value;
                    UpperToothList[0] = value;
                    RaisePropertyChanged("UpperTmp0");
                    RaisePropertyChanged("UpperPoints");
                }
            }
        }

        private bool _UpperTmp1;
        public bool UpperTmp1
        {
            get { return _UpperTmp1; }
            set
            {
                if (_UpperTmp1 != value)
                {
                    _UpperTmp1 = value;
                    UpperToothList[1] = value;
                    RaisePropertyChanged("UpperTmp1");
                    RaisePropertyChanged("UpperPoints");
                }
            }
        }

        private bool _UpperTmp2;
        public bool UpperTmp2
        {
            get { return _UpperTmp2; }
            set
            {
                if (_UpperTmp2 != value)
                {
                    _UpperTmp2 = value;
                    UpperToothList[2] = value;
                    RaisePropertyChanged("Points");
                    RaisePropertyChanged("UpperPoints");
                }
            }
        }

        private bool _UpperTmp3;
        public bool UpperTmp3
        {
            get { return _UpperTmp3; }
            set
            {
                if (_UpperTmp3 != value)
                {
                    _UpperTmp3 = value;
                    UpperToothList[3] = value;
                    RaisePropertyChanged("_UpperTmp3");
                    RaisePropertyChanged("UpperPoints");
                }
            }
        }

        private bool _UpperTmp4;
        public bool UpperTmp4
        {
            get { return _UpperTmp4; }
            set
            {
                if (_UpperTmp4 != value)
                {
                    _UpperTmp4 = value;
                    UpperToothList[4] = value;
                    RaisePropertyChanged("UpperTmp4");
                    RaisePropertyChanged("UpperPoints");
                }
            }
        }

        private bool _UpperTmp5;
        public bool UpperTmp5
        {
            get { return _UpperTmp5; }
            set
            {
                if (_UpperTmp5 != value)
                {
                    _UpperTmp5 = value;
                    UpperToothList[5] = value;
                    RaisePropertyChanged("UpperTmp5");
                    RaisePropertyChanged("UpperPoints");
                }
            }
        }


        // Lower Tooth
        private List<bool> LowerToothList;

        private bool _LowerTmpNo;
        public bool LowerTmpNo
        {
            get { return _LowerTmpNo; }
            set
            {
                if (_LowerTmpNo != value)
                {
                    _LowerTmpNo = value;
                    LowerToothList[0] = value;
                    RaisePropertyChanged("LowerTmpNo");
                    RaisePropertyChanged("LowerPoints");
                }
            }
        }

        private bool _LowerTmp1;
        public bool LowerTmp1
        {
            get { return _LowerTmp1; }
            set
            {
                if (_LowerTmp1 != value)
                {
                    _LowerTmp1 = value;
                    LowerToothList[1] = value;
                    RaisePropertyChanged("LowerTmp1");
                    RaisePropertyChanged("LowerPoints");
                }
            }
        }

        #endregion

        #region Points

        // Upper
        private ToothType _UpperPoints;
        public ToothType UpperPoints
        {
            get { return _UpperPoints = GetAllPointsUpper(); }
            set
            {
                _UpperPoints = value;
                RaisePropertyChanged("UpperPoints");
            }
        }

        private ToothType GetAllPointsUpper()
        {
            int idx_up = Idx_Templates(UpperToothList);
            if (idx_up > 0)
                return main.UpperTooth[idx_up - 1];
            return null;
        }

        // Lower
        private ToothType _LowerPoints;
        public ToothType LowerPoints
        {
            get { return _LowerPoints = GetAllPointsLower(); }
        }

        private ToothType GetAllPointsLower()
        {
            int idx_down = Idx_Templates(LowerToothList);
            if (idx_down > 0)
                return main.LowerTooth[idx_down - 1];
            return null;
        }

        private int Idx_Templates(List<bool> list)
        {
            int indexOfTemplates = -1;
            foreach (bool selected in list)
            {
                if (selected)
                {
                    indexOfTemplates = list.IndexOf(selected);
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

            main.eye_L.Visibility = Visibility.Visible;
            main.eye_R.Visibility = Visibility.Visible;
            main.mouth_L.Visibility = Visibility.Visible;
            main.mouth_R.Visibility = Visibility.Visible;

            RaisePropertyChanged("EyeL");
            RaisePropertyChanged("EyeR");
            RaisePropertyChanged("MouthL");
            RaisePropertyChanged("MouthR");

            RaisePropertyChanged("MidLine");
            RaisePropertyChanged("NoseLineL");
            RaisePropertyChanged("NoseLineR");
            RaisePropertyChanged("EyeLine");
            RaisePropertyChanged("LipLine");


            //GetGuideMark();
            //var data = ((MainWindow)Application.Current.MainWindow).Upper;
            //for (int a = 0; a < 5; a++)
            //{
            //    for (int b = 0; b < 6; b++)
            //    {
            //        for (int c = 0; c < 10; c++)
            //        {
            //            data[a][b][c].X += guideMark.X - 400;
            //            data[a][b][c].Y += guideMark.Y - 200;
            //        }
            //    }
            //}
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

        public Rectangle rect, bef_rect = null;
        private Border border, bef_border = null;
        private Brush orgBrush;

        private bool captured;
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
            rect = e.Source as Rectangle;
            if (bef_rect != null)
            {
                bef_rect.Opacity = 0;
                bef_border.Opacity = 0;
            }

            Grid grid = (Grid)rect.Parent;
            border = (Border)grid.Parent;

            bef_rect = rect;
            bef_border = border;
            rect.Opacity = 1;
            border.Opacity = 1;

            originalPoint = e.GetPosition((IInputElement)e.Source);

            // padding
            originalPoint.X += 5;
            originalPoint.Y += 5;

            orgBrush = border.BorderBrush;
            rect.Stroke = Brushes.LightSalmon;
            border.BorderBrush = Brushes.LightSalmon;

            captured = true;
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
            if (captured == true)
            {
                Rectangle rect = e.Source as Rectangle;
                var findWrap = ViewUtils.FindParent(rect, Type.GetType("MeditSmile2D.View.WrapTeeth"));
                Debug.Assert(findWrap != null);

                //var find = ViewUtils.GetParent(rect, );

                WrapTeeth wrap = findWrap as WrapTeeth;
                Teeth you = null;
                if (main.mirror.IsChecked == true)
                {
                    var findTeeth = ViewUtils.FindParent(wrap, (new Teeth()).GetType());
                    var findGrid = ViewUtils.FindParent(wrap, (new Grid()).GetType());
                    Debug.Assert(findTeeth != null && findGrid != null);
                    //var findTeeth = FindUpElement(wrap, Type.GetType("MeditSmile2D.View.Teeth"));
                    //var findGrid = FindUpElement(wrap, Type.GetType("System.Windows.Controls.Grid"));

                    Teeth me = findTeeth as Teeth;
                    Grid upper = findGrid as Grid;

                    int idx_me = main.dic[me.Name];
                    int idx_you = idx_me + (idx_me >= 0 && idx_me < 3 ? +3 : -3);
                    var myKey = main.dic.FirstOrDefault(p => p.Value == idx_you).Key;
                    you = upper.FindName(myKey) as Teeth;
                }

                Point curPoint = e.GetPosition((IInputElement)e.Source);
                var dragDelta = curPoint - originalPoint;
                foreach (PointViewModel point in wrap.Points)
                {
                    point.X += dragDelta.X;
                    point.Y += dragDelta.Y;
                }

                if (you != null)
                {
                    foreach (PointViewModel point in you.Points)
                    {
                        point.X -= dragDelta.X;
                        point.Y += dragDelta.Y;
                    }
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
            rect.Stroke = orgBrush;
            border.BorderBrush = orgBrush;

            captured = false;
            Mouse.Capture(null);
        }

        #endregion

        #region DragDrop for Tooth

        //private bool captured;            // teeth와 공유
        //private Point originalPoint_tooth;

        private Rectangle rect2;
        private Border border2;
        private Brush orgBrush2;

        private RelayCommand<object> _mouseLeftDownForDragAndDropTooth;
        public RelayCommand<object> MouseLeftDownForDragAndDropTooth
        {
            get
            {
                if (_mouseLeftDownForDragAndDropTooth == null)
                    return _mouseLeftDownForDragAndDropTooth = new RelayCommand<object>(param => ExecuteMouseLeftDownForDragAndDropTooth((MouseEventArgs)param));
                return _mouseLeftDownForDragAndDropTooth;
            }
            set { _mouseLeftDownForDragAndDropTooth = value; }
        }
        public void ExecuteMouseLeftDownForDragAndDropTooth(MouseEventArgs e)
        {
            originalPoint = e.GetPosition((IInputElement)e.Source);

            //padding
            originalPoint.X += 5;
            originalPoint.Y += 5;

            // Clicked
            rect2 = e.Source as Rectangle;
            Grid grid = (Grid)rect2.Parent;
            border2 = (Border)grid.Parent;

            orgBrush2 = border2.BorderBrush;
            rect2.Stroke = Brushes.Red;
            border2.BorderBrush = Brushes.Red;

            captured = true;
            Mouse.Capture((IInputElement)e.Source);
        }

        private RelayCommand<object> _mouseMoveForDragAndDropTooth;
        public RelayCommand<object> MouseMoveForDragAndDropTooth
        {
            get
            {
                if (_mouseMoveForDragAndDropTooth == null)
                    return _mouseMoveForDragAndDropTooth = new RelayCommand<object>(param => ExecuteMouseMoveForDragAndDropTooth((MouseEventArgs)param));
                return _mouseMoveForDragAndDropTooth;
            }
            set { _mouseMoveForDragAndDropTooth = value; }
        }
        public void ExecuteMouseMoveForDragAndDropTooth(MouseEventArgs e)
        {
            rect2 = (Rectangle)e.Source;
            Grid grid = (Grid)rect2.Parent;
            border2 = (Border)grid.Parent;
            WrapTooth tooth = (WrapTooth)border2.Parent;

            if (captured == true)
            {
                Point curMouseDownPoint = e.GetPosition((IInputElement)e.Source);
                var dragDelta = curMouseDownPoint - originalPoint;

                foreach (TeethType points in tooth.Points)
                {
                    foreach (PointViewModel point in points)
                    {
                        point.X += dragDelta.X;
                        point.Y += dragDelta.Y;
                    }
                }
            }
        }

        private RelayCommand<object> _mouseLeftUpForDragAndDropTooth;
        public RelayCommand<object> MouseLeftUpForDragAndDropTooth
        {
            get
            {
                if (_mouseLeftUpForDragAndDropTooth == null)
                    return _mouseLeftUpForDragAndDropTooth = new RelayCommand<object>(param => ExecuteMouseLeftUpForDragAndDropTooth((MouseEventArgs)param));
                return _mouseLeftUpForDragAndDropTooth;
            }
            set { _mouseLeftUpForDragAndDropTooth = value; }
        }
        public void ExecuteMouseLeftUpForDragAndDropTooth(MouseEventArgs e)
        {
            rect2.Stroke = orgBrush2;
            border2.BorderBrush = orgBrush2;

            captured = false;
            Mouse.Capture(null);
        }

        #endregion

        #region Resize for Teeth

        private bool isSizing;
        private bool isFirstTimeMovedOnSizing;
        private readonly double sizeThreshold = 20;

        private Point anchorMin, anchorMax;

        private void SetSizingTeeth(MouseEventArgs e)
        {
            if (!isSizing)
                return;

            Border border = (Border)e.Source;
            Grid grid = (Grid)border.Parent;
            Border borderSecond = (Border)grid.Parent;
            WrapTeeth wrapTeeth = (WrapTeeth)borderSecond.Parent;

            Canvas cv = wrapTeeth.Parent as Canvas;
            Teeth teeth = cv.Parent as Teeth;
            var wrap = Numerics.TeethToList(teeth);

            Point maxPoint = new Point(Numerics.GetMaxX_Teeth(wrap).X, Numerics.GetMaxY_Teeth(wrap).Y);
            Point minPoint = new Point(Numerics.GetMinX_Teeth(wrap).X, Numerics.GetMinY_Teeth(wrap).Y);

            if (isFirstTimeMovedOnSizing)
            {
                anchorMin = new Point(minPoint.X, minPoint.Y);
                anchorMax = new Point(maxPoint.X, maxPoint.Y);
                isFirstTimeMovedOnSizing = false;
            }

            if (e.LeftButton != MouseButtonState.Pressed)
                return;

            Point moved = e.GetPosition(e.Source as IInputElement);
            double changedWidth = maxPoint.X - minPoint.X + moved.X;
            double changedHeight = maxPoint.Y - minPoint.Y + moved.Y;
            double changedWidth_rev = maxPoint.X - minPoint.X - moved.X;
            double changedHeight_rev = maxPoint.Y - minPoint.Y - moved.Y;

            if (border.Name.Equals("Border_Top"))
            {
                foreach (PointViewModel point in wrapTeeth.Points)
                {
                    //point.Y = point.Y * ((actualHeight2 - curPoint.Y) / actualHeight2);
                    if (changedHeight_rev > sizeThreshold)
                    {
                        double RatioY = Math.Abs((maxPoint.Y - moved.Y) / maxPoint.Y);
                        point.Y = point.Y * RatioY + anchorMax.Y * (1 - RatioY);
                    }
                }
            }
            else if (border.Name.Equals("Border_Bottom"))
            {
                foreach (PointViewModel point in wrapTeeth.Points)
                {
                    if (changedHeight > sizeThreshold)
                    {
                        double RatioY = Math.Abs((maxPoint.Y + moved.Y) / maxPoint.Y);
                        point.Y = point.Y * RatioY + anchorMin.Y * (1 - RatioY);
                    }
                    //point.Y = point.Y * ((actualHeight1 + curPoint.Y) / actualHeight1) - (minPoint.Y - anchorMin.Y);
                }
            }
            else if (border.Name.Equals("Border_Left"))
            {
                foreach (PointViewModel point in wrapTeeth.Points)
                {
                    //point.X = point.X * ((actualWidth2 - curPoint.X) / actualWidth2);
                    if (changedWidth_rev > sizeThreshold)
                    {
                        double RatioX = Math.Abs((maxPoint.X - moved.X) / maxPoint.X);
                        point.X = point.X * RatioX + anchorMax.X * (1 - RatioX);
                    }
                }
            }
            else if (border.Name.Equals("Border_Right"))
            {
                foreach (PointViewModel point in wrapTeeth.Points)
                {
                    //point.X = point.X * ((actualWidth1 + curPoint.X) / actualWidth1) - (minPoint.X - anchorMin.X);
                    if (changedWidth > sizeThreshold)
                    {
                        double RatioX = Math.Abs((maxPoint.X + moved.X) / maxPoint.X);
                        point.X = point.X * RatioX + anchorMin.X * (1 - RatioX);
                    }
                }
            }
            else if (border.Name.Equals("Border_TopLeft"))
            {
                foreach (PointViewModel point in wrapTeeth.Points)
                {
                    //point.X = point.X * ((actualWidth2 - curPoint.X) / actualWidth2);
                    //point.Y = point.Y * ((actualHeight2 - curPoint.Y) / actualHeight2);
                    if (changedWidth_rev > sizeThreshold)
                    {
                        double RatioX = Math.Abs((maxPoint.X - moved.X) / maxPoint.X);
                        point.X = point.X * RatioX + anchorMax.X * (1 - RatioX);
                    }
                    if (changedHeight_rev > sizeThreshold)
                    {
                        double RatioY = Math.Abs((maxPoint.Y - moved.Y) / maxPoint.Y);
                        point.Y = point.Y * RatioY + anchorMax.Y * (1 - RatioY);
                    }
                }
            }
            else if (border.Name.Equals("Border_TopRight"))
            {
                foreach (PointViewModel point in wrapTeeth.Points)
                {
                    //point.X = point.X * ((actualWidth2 + curPoint.X) / actualWidth2);
                    //point.Y = point.Y * ((actualHeight2 - curPoint.Y) / actualHeight2);
                    if (changedWidth > sizeThreshold)
                    {
                        double RatioX = Math.Abs((maxPoint.X + moved.X) / maxPoint.X);
                        point.X = point.X * RatioX + anchorMin.X * (1 - RatioX);
                    }
                    if (changedHeight_rev > sizeThreshold)
                    {
                        double RatioY = Math.Abs((maxPoint.Y - moved.Y) / maxPoint.Y);
                        point.Y = point.Y * RatioY + anchorMax.Y * (1 - RatioY);
                    }
                }
            }
            else if (border.Name.Equals("Border_BottomLeft"))
            {
                foreach (PointViewModel point in wrapTeeth.Points)
                {
                    //point.X = point.X * ((actualWidth2 - curPoint.X) / actualWidth2);
                    //point.Y = point.Y * ((actualHeight2 + curPoint.Y) / actualHeight2);
                    if (changedWidth_rev > sizeThreshold)
                    {
                        double RatioX = Math.Abs((maxPoint.X - moved.X) / maxPoint.X);
                        point.X = point.X * RatioX + anchorMax.X * (1 - RatioX);
                    }
                    if (changedHeight > sizeThreshold)
                    {
                        double RatioY = Math.Abs((maxPoint.Y + moved.Y) / maxPoint.Y);
                        point.Y = point.Y * RatioY + anchorMin.Y * (1 - RatioY);
                    }
                }
            }
            else if (border.Name.Equals("Border_BottomRight"))
            {
                foreach (PointViewModel point in wrapTeeth.Points)
                {
                    if (changedWidth > sizeThreshold)
                    {
                        double RatioX = Math.Abs((maxPoint.X + moved.X) / maxPoint.X);
                        point.X = point.X * RatioX + anchorMin.X * (1 - RatioX);
                    }
                    if (changedHeight > sizeThreshold)
                    {
                        double RatioY = Math.Abs((maxPoint.Y + moved.Y) / maxPoint.Y);
                        point.Y = point.Y * RatioY + anchorMin.Y * (1 - RatioY);
                    }
                    //point.X = point.X * ((actualWidth1 + curPoint.X) / actualWidth1) - (minPoint.X - anchorMin.X);
                    //point.Y = point.Y * ((actualHeight1 + curPoint.Y) / actualHeight1) - (minPoint.Y - anchorMin.Y);
                }
            }
        }

        #region Resize for CommandProperties

        // For Teeth
        private RelayCommand<object> _mouseLeftDownForResizeTeeth;
        public RelayCommand<object> MouseLeftDownForResizeTeeth
        {
            get
            {
                if (_mouseLeftDownForResizeTeeth == null)
                    return _mouseLeftDownForResizeTeeth = new RelayCommand<object>(param => ExecuteMouseLeftDownForResizeTeeth((MouseEventArgs)param));
                return _mouseLeftDownForResizeTeeth;
            }
            set { _mouseLeftDownForResizeTeeth = value; }
        }

        public void ExecuteMouseLeftDownForResizeTeeth(MouseEventArgs e)
        {
            isSizing = true;
            //anchorPoint = e.GetPosition((IInputElement)e.Source);
            Mouse.Capture((IInputElement)e.Source);
        }

        private RelayCommand<object> _mouseMoveForResizeTeeth;
        public RelayCommand<object> MouseMoveForResizeTeeth
        {
            get
            {
                if (_mouseMoveForResizeTeeth == null)
                    return _mouseMoveForResizeTeeth = new RelayCommand<object>(param => ExecuteMouseMoveForResizeTeeth((MouseEventArgs)param));
                return _mouseMoveForResizeTeeth;
            }
            set { _mouseMoveForResizeTeeth = value; }
        }

        public void ExecuteMouseMoveForResizeTeeth(MouseEventArgs e)
        {
            SetSizingTeeth(e);
        }

        private RelayCommand<object> _mouseLeftUpForResizeTeeth;
        public RelayCommand<object> MouseLeftUpForResizeTeeth
        {
            get
            {
                if (_mouseLeftUpForResizeTeeth == null)
                    return _mouseLeftUpForResizeTeeth = new RelayCommand<object>(param => ExecuteMouseLeftUpForResizeTeeth((MouseEventArgs)param));
                return _mouseLeftUpForResizeTeeth;
            }
            set { _mouseLeftUpForResizeTeeth = value; }
        }

        public void ExecuteMouseLeftUpForResizeTeeth(MouseEventArgs e)
        {
            isSizing = false;
            Mouse.Capture(null);
            isFirstTimeMovedOnSizing = true;
        }

        #endregion

        #endregion

        #region Resize for Tooth

        private void SetSizingTooth(MouseEventArgs e)
        {
            if (!isSizing)
                return;

            Border border = (Border)e.Source;
            Grid grid = (Grid)border.Parent;
            Border borderSecond = (Border)grid.Parent;
            WrapTooth wrapTooth = (WrapTooth)borderSecond.Parent;

            List<List<Point>> tooth = new List<List<Point>>();
            List<Point> teeth;
            foreach (TeethType points in wrapTooth.Points)
            {
                teeth = new List<Point>();
                foreach (PointViewModel point in points)
                    teeth.Add(new Point(point.X, point.Y));
                tooth.Add(teeth);
            }

            Point minPoint = Numerics.GetMinXY_Tooth(tooth);
            Point maxPoint = Numerics.GetMaxXY_Tooth(tooth);

            if (isFirstTimeMovedOnSizing)
            {
                //to memory the first location of anchor point
                anchorMin = new Point(minPoint.X, minPoint.Y);
                anchorMax = new Point(maxPoint.X, maxPoint.Y);
                isFirstTimeMovedOnSizing = false;
            }

            if (e.LeftButton != MouseButtonState.Pressed)
                return;

            Point moved = e.GetPosition(e.Source as IInputElement);
            double changedWidth = maxPoint.X - minPoint.X + moved.X;
            double changedHeight = maxPoint.Y - minPoint.Y + moved.Y;
            double changedWidth_rev = maxPoint.X - minPoint.X - moved.X;
            double changedHeight_rev = maxPoint.Y - minPoint.Y - moved.Y;

            if (border.Name.Equals("Border_Top"))
            {
                foreach (TeethType points in wrapTooth.Points)
                {
                    foreach (PointViewModel point in points)
                    {
                        //point.Y = point.Y * ((actualHeight2 - curPoint.Y) / actualHeight2);
                        if (changedHeight_rev > sizeThreshold)
                        {
                            double RatioY = Math.Abs((maxPoint.Y - moved.Y) / maxPoint.Y);
                            point.Y = point.Y * RatioY + anchorMax.Y * (1 - RatioY);
                        }
                    }
                }
            }
            else if (border.Name.Equals("Border_Bottom"))
            {
                foreach (TeethType points in wrapTooth.Points)
                {
                    foreach (PointViewModel point in points)
                    {
                        if (changedHeight > sizeThreshold)
                        {
                            double RatioY = Math.Abs((maxPoint.Y + moved.Y) / maxPoint.Y);
                            point.Y = point.Y * RatioY + anchorMin.Y * (1 - RatioY);
                        }
                        //point.Y = point.Y * ((actualHeight1 + curPoint.Y) / actualHeight1) - (minPoint.Y - anchorMin.Y);
                    }
                }
            }
            else if (border.Name.Equals("Border_Left"))
            {
                foreach (TeethType points in wrapTooth.Points)
                {
                    foreach (PointViewModel point in points)
                    {
                        //point.X = point.X * ((actualWidth2 - curPoint.X) / actualWidth2);
                        if (changedWidth_rev > sizeThreshold)
                        {
                            double RatioX = Math.Abs((maxPoint.X - moved.X) / maxPoint.X);
                            point.X = point.X * RatioX + anchorMax.X * (1 - RatioX);
                        }
                    }
                }
            }
            else if (border.Name.Equals("Border_Right"))
            {
                foreach (TeethType points in wrapTooth.Points)
                {
                    foreach (PointViewModel point in points)
                    {
                        //point.X = point.X * ((actualWidth1 + curPoint.X) / actualWidth1) - (minPoint.X - anchorMin.X);
                        if (changedWidth > sizeThreshold)
                        {
                            double RatioX = Math.Abs((maxPoint.X + moved.X) / maxPoint.X);
                            point.X = point.X * RatioX + anchorMin.X * (1 - RatioX);
                        }
                    }
                }
            }
            else if (border.Name.Equals("Border_TopLeft"))
            {
                foreach (TeethType points in wrapTooth.Points)
                {
                    foreach (PointViewModel point in points)
                    {
                        //point.X = point.X * ((actualWidth2 - curPoint.X) / actualWidth2);
                        //point.Y = point.Y * ((actualHeight2 - curPoint.Y) / actualHeight2);
                        if (changedWidth_rev > sizeThreshold)
                        {
                            double RatioX = Math.Abs((maxPoint.X - moved.X) / maxPoint.X);
                            point.X = point.X * RatioX + anchorMax.X * (1 - RatioX);
                        }
                        if (changedHeight_rev > sizeThreshold)
                        {
                            double RatioY = Math.Abs((maxPoint.Y - moved.Y) / maxPoint.Y);
                            point.Y = point.Y * RatioY + anchorMax.Y * (1 - RatioY);
                        }
                    }
                }
            }
            else if (border.Name.Equals("Border_TopRight"))
            {
                foreach (TeethType points in wrapTooth.Points)
                {
                    foreach (PointViewModel point in points)
                    {
                        //point.X = point.X * ((actualWidth2 + curPoint.X) / actualWidth2);
                        //point.Y = point.Y * ((actualHeight2 - curPoint.Y) / actualHeight2);
                        if (changedWidth > sizeThreshold)
                        {
                            double RatioX = Math.Abs((maxPoint.X + moved.X) / maxPoint.X);
                            point.X = point.X * RatioX + anchorMin.X * (1 - RatioX);
                        }
                        if (changedHeight_rev > sizeThreshold)
                        {
                            double RatioY = Math.Abs((maxPoint.Y - moved.Y) / maxPoint.Y);
                            point.Y = point.Y * RatioY + anchorMax.Y * (1 - RatioY);
                        }
                    }
                }
            }
            else if (border.Name.Equals("Border_BottomLeft"))
            {
                foreach (TeethType points in wrapTooth.Points)
                {
                    foreach (PointViewModel point in points)
                    {
                        //point.X = point.X * ((actualWidth2 - curPoint.X) / actualWidth2);
                        //point.Y = point.Y * ((actualHeight2 + curPoint.Y) / actualHeight2);
                        if (changedWidth_rev > sizeThreshold)
                        {
                            double RatioX = Math.Abs((maxPoint.X - moved.X) / maxPoint.X);
                            point.X = point.X * RatioX + anchorMax.X * (1 - RatioX);
                        }
                        if (changedHeight > sizeThreshold)
                        {
                            double RatioY = Math.Abs((maxPoint.Y + moved.Y) / maxPoint.Y);
                            point.Y = point.Y * RatioY + anchorMin.Y * (1 - RatioY);
                        }
                    }
                }
            }
            else if (border.Name.Equals("Border_BottomRight"))
            {
                foreach (TeethType points in wrapTooth.Points)
                {
                    foreach (PointViewModel point in points)
                    {
                        if (changedWidth > sizeThreshold)
                        {
                            double RatioX = Math.Abs((maxPoint.X + moved.X) / maxPoint.X);
                            point.X = point.X * RatioX + anchorMin.X * (1 - RatioX);
                        }
                        if (changedHeight > sizeThreshold)
                        {
                            double RatioY = Math.Abs((maxPoint.Y + moved.Y) / maxPoint.Y);
                            point.Y = point.Y * RatioY + anchorMin.Y * (1 - RatioY);
                        }
                        //point.X = point.X * ((actualWidth1 + curPoint.X) / actualWidth1) - (minPoint.X - anchorMin.X);
                        //point.Y = point.Y * ((actualHeight1 + curPoint.Y) / actualHeight1) - (minPoint.Y - anchorMin.Y);
                    }
                }
            }
        }

        #region Resize for CommandProperties

        // For Tooth
        private RelayCommand<object> _mouseLeftDownForResizeTooth;
        public RelayCommand<object> MouseLeftDownForResizeTooth
        {
            get
            {
                if (_mouseLeftDownForResizeTooth == null)
                    return _mouseLeftDownForResizeTooth = new RelayCommand<object>(param => ExecuteMouseLeftDownForResizeTooth((MouseEventArgs)param));
                return _mouseLeftDownForResizeTooth;
            }
            set { _mouseLeftDownForResizeTooth = value; }
        }

        public void ExecuteMouseLeftDownForResizeTooth(MouseEventArgs e)
        {
            isSizing = true;
            //anchorPoint = e.GetPosition((IInputElement)e.Source);
            Mouse.Capture((IInputElement)e.Source);
        }

        private RelayCommand<object> _mouseMoveForResizeTooth;
        public RelayCommand<object> MouseMoveForResizeTooth
        {
            get
            {
                if (_mouseMoveForResizeTooth == null)
                    return _mouseMoveForResizeTooth = new RelayCommand<object>(param => ExecuteMouseMoveForResizeTooth((MouseEventArgs)param));
                return _mouseMoveForResizeTooth;
            }
            set { _mouseMoveForResizeTooth = value; }
        }

        public void ExecuteMouseMoveForResizeTooth(MouseEventArgs e)
        {
            SetSizingTooth(e);
        }

        private RelayCommand<object> _mouseLeftUpForResizeTooth;
        public RelayCommand<object> MouseLeftUpForResizeTooth
        {
            get
            {
                if (_mouseLeftUpForResizeTooth == null)
                    return _mouseLeftUpForResizeTooth = new RelayCommand<object>(param => ExecuteMouseLeftUpForResizeTooth((MouseEventArgs)param));
                return _mouseLeftUpForResizeTooth;
            }
            set { _mouseLeftUpForResizeTooth = value; }
        }

        public void ExecuteMouseLeftUpForResizeTooth(MouseEventArgs e)
        {
            isSizing = false;
            Mouse.Capture(null);
            isFirstTimeMovedOnSizing = true;
        }

        #endregion

        #endregion

        #region DragDrop for SmileLine

        //private Rectangle rect3;
        private Point arc_origin;
        private bool captured_arc;

        private RelayCommand<object> _mouseLeftDownForSmileLine;
        public RelayCommand<object> MouseLeftDownForSmileLine
        {
            get
            {
                if (_mouseLeftDownForSmileLine == null)
                    return _mouseLeftDownForSmileLine = new RelayCommand<object>(param => ExecuteMouseLeftDownForSmileLine((MouseEventArgs)param));
                return _mouseLeftDownForSmileLine;
            }
            set { _mouseLeftDownForSmileLine = value; }
        }
        private void ExecuteMouseLeftDownForSmileLine(MouseEventArgs e)
        {
            Path smile = e.Source as Path;
            if (smile == null)
                return;

            //arc_origin = e.GetPosition(e.Source as IInputElement);
            captured_arc = true;
            Mouse.Capture(smile);
        }

        private RelayCommand<object> _mouseMoveForSmileLine;
        public RelayCommand<object> MouseMoveForSmileLine
        {
            get
            {
                if (_mouseMoveForSmileLine == null)
                    return _mouseMoveForSmileLine = new RelayCommand<object>(param => ExecuteMouseMoveForSmileLine((MouseEventArgs)param));
                return _mouseMoveForSmileLine;
            }
            set { _mouseMoveForSmileLine = value; }
        }
        private void ExecuteMouseMoveForSmileLine(MouseEventArgs e)
        {
            Path smile = e.Source as Path;
            if (smile == null)
                return;

            Ellipse me = e.Source as Ellipse;
            //FrameworkElement fr = me.Parent as FrameworkElement;
            //while (1)
            //{
            //    if (fr.Name.Equals("UpperCanvas"))
            //}
            //if (captured_arc == true)
            //{
            //    Point curMouseDownPoint = e.GetPosition((IInputElement)e.Source);
            //    var dragDelta = curMouseDownPoint - originalPoint;

            //    foreach (TeethType points in tooth.Points)
            //    {
            //        if ()
            //            foreach (PointViewModel point in points)
            //            {
            //                point.X += dragDelta.X;
            //                point.Y += dragDelta.Y;
            //            }
            //    }
            //}

        }

        private RelayCommand<object> _mouseLeftUpForSmileLine;
        public RelayCommand<object> MouseLeftUpForSmileLine
        {
            get
            {
                if (_mouseLeftUpForSmileLine == null)
                    return _mouseLeftUpForSmileLine = new RelayCommand<object>(param => ExecuteMouseLeftUpForSmileLine((MouseEventArgs)param));
                return _mouseLeftUpForSmileLine;
            }
            set { _mouseLeftUpForSmileLine = value; }
        }
        private void ExecuteMouseLeftUpForSmileLine(MouseEventArgs e)
        {
            captured_arc = false;
            Mouse.Capture(null);
        }

        #endregion


        List<Teeth> SelectedList = new List<Teeth>();

        #region Rotate for Teeth

        private bool captured_rotate = false;
        private List<bool> isFirstTimeMovedOnRotating = new List<bool>();
        //private bool[] iif = new bool[10];
        private double accAlangle = 0;
        private List<Point> RotateAnchor = new List<Point>();

        private RelayCommand<object> _mouseLeftDownForRotateTeeth;
        public RelayCommand<object> MouseLeftDownForRotateTeeth
        {
            get
            {
                if (_mouseLeftDownForRotateTeeth == null)
                    return _mouseLeftDownForRotateTeeth = new RelayCommand<object>(param => ExecuteMouseLeftDownForRotateTeeth(param as MouseEventArgs));
                return _mouseLeftDownForRotateTeeth;
            }
            set { _mouseLeftDownForRotateTeeth = value; }
        }

        private void ExecuteMouseLeftDownForRotateTeeth(MouseEventArgs e)
        {
            if (captured_rotate)
                return;
            captured_rotate = true;
            Mouse.Capture(e.Source as IInputElement);
        }

        private RelayCommand<object> _mouseMoveForRotateTeeth;
        public RelayCommand<object> MouseMoveForRotateTeeth
        {
            get
            {
                if (_mouseMoveForRotateTeeth == null)
                    return _mouseMoveForRotateTeeth = new RelayCommand<object>(param => ExecuteMouseMoveForRotateTeeth(param as MouseEventArgs));
                return _mouseMoveForRotateTeeth;
            }
            set { _mouseMoveForRotateTeeth = value; }
        }

        private void ExecuteMouseMoveForRotateTeeth(MouseEventArgs e)
        {
            if (!captured_rotate)
                return;

            RotateTeeth me = e.Source as RotateTeeth;
            List<Point> rotate = Numerics.TeethToList(me.Points);
            Point min = new Point(Numerics.GetMinX_Teeth(me.Points).X, Numerics.GetMinY_Teeth(me.Points).Y);
            Point max = new Point(Numerics.GetMaxX_Teeth(me.Points).X, Numerics.GetMaxY_Teeth(me.Points).Y);

            Point ctrl = new Point((max.X + min.X) / 2, (max.Y + min.Y) / 2);
            Point cur = e.GetPosition(e.Source as IInputElement);

            for(int i = 0; i < SelectedList.Count; i++)
            {
                if (isFirstTimeMovedOnRotating[i])
                {
                    RotateAnchor.Add(ctrl);
                    isFirstTimeMovedOnRotating[i] = false;
                }
            }

            double rad = Math.Atan2(cur.Y - ctrl.Y, cur.X - ctrl.X);
            double deg = rad * 180.0 / Math.PI;
            deg += accAlangle + 90;
            if (deg <= -30 || deg >= 30)
                return;

            int j = 0; 
            foreach(Teeth teeth in SelectedList)
            {
                RotateTransform rotate = new RotateTransform(deg, RotateAnchor[j].X, RotateAnchor[j].Y);
                teeth.RenderTransform = rotate;
                accAlangle = deg;
                j++;
            }
        }

        private RelayCommand<object> _mouseLeftUpForRotateTeeth;
        public RelayCommand<object> MouseLeftUpForRotateTeeth
        {
            get
            {
                if (_mouseLeftUpForRotateTeeth == null)
                    return _mouseLeftUpForRotateTeeth = new RelayCommand<object>(param => ExecuteMouseLeftUpForRotateTeeth(param as MouseEventArgs));
                return _mouseLeftUpForRotateTeeth;
            }
            set { _mouseLeftUpForRotateTeeth = value; }
        }
        
        private void ExecuteMouseLeftUpForRotateTeeth(MouseEventArgs e)
        {
            captured_rotate = false;
            for (int i = 0; i < isFirstTimeMovedOnRotating.Count; i++)
                isFirstTimeMovedOnRotating[i] = true;
            Mouse.Capture(null);
        }

        #endregion
    }
}
