using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MeditSmile2D.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace MeditSmile2D.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    using ToothType = ObservableCollection<ObservableCollection<PointViewModel>>;    

    public class MainViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public MainViewModel()
        {
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

        private List<bool> SelectedTemplates = new List<bool>();

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
            /// 2.
            // ���ø��� ���õ��� ���� �ʱ���¿����� GetAllPoints()���� null���� ����
            //      �� ��� Teeth.Points�� ������� ���� => �� PointViewModel��ü���� OnPropertyChanged�� �߻����� ����
            // ���ø��� ���õǾ� GetAllPoints()���� non-null�� ���ϵ� ��� 
            // (���ϵ� ��) Teeth.Points ������Ƽ�� Set �Ǳ� ������ Teeth.PropertyChangedCallback()�� �Ѿ
            // �Ѿ�� �����ʹ� ToothType(�� 10���� �� ��Ʈ�� 6�� �ִ� Tooth�� �Ѱ�����, 
            // <Teeth Points="{Binding Points[0]}"> ... �̹Ƿ� 
            // Teeth.Points �� �޴� ���� �����ʹ� TeethType�� Points[n]���� �Ѿ
            get { return _Points = GetAllPoints(); }
        }

        private ToothType GetAllPoints()
        {
            int idx = Idx_Templates();
            if (idx < 0)
                return null;
            return ((App)Application.Current).templates[idx];
        }

        #region ����ġ�� ���ε� �ڵ�
        //private ToothType _Points;
        //public ToothType Points
        //{
        //    get { return _Points = GetAllPoints(); }
        //}

        //private ToothType GetAllPoints()
        //{
        //    int rb_idx = Idx_Templates();
        //    if (rb_idx < 0)
        //        return null;
        //    else
        //    {
        //        ToothType tooth = new ToothType();
        //        tooth.Add(_CanineL);
        //        tooth.Add(_LateralIncisorL);
        //        tooth.Add(_CentralIncisorL);
        //        tooth.Add(_CentralIncisorR);
        //        tooth.Add(_LateralIncisorR);
        //        tooth.Add(_CanineR);
        //        return tooth;
        //    }
        //}

        //private TeethType _CanineL;
        //public TeethType CanineL
        //{
        //    get {
        //        int idx = Idx_Templates();
        //        if (idx < 0)
        //            return null;
        //        _CanineL = ((App)Application.Current).templates[idx][5];
        //        RaisePropertyChanged("Points");
        //        return _CanineL;
        //    }
        //}

        //private TeethType _LateralIncisorL;
        //public TeethType LateralIncisorL
        //{
        //    get
        //    {
        //        int idx = Idx_Templates();
        //        if (idx < 0)
        //            return null;
        //        _LateralIncisorL = ((App)Application.Current).templates[idx][4];
        //        RaisePropertyChanged("Points");
        //        return _LateralIncisorL;
        //    }
        //}

        //private TeethType _CentralIncisorL;
        //public TeethType CentralIncisorL
        //{
        //    get
        //    {
        //        int idx = Idx_Templates();
        //        if (idx < 0)
        //            return null;
        //        _CentralIncisorL = ((App)Application.Current).templates[idx][3];
        //        RaisePropertyChanged("Points");
        //        return _CentralIncisorL;
        //    }
        //}

        //private TeethType _CentralIncisorR;
        //public TeethType CentralIncisorR
        //{
        //    get
        //    {
        //        int idx = Idx_Templates();
        //        if (idx < 0)
        //            return null;
        //        _CentralIncisorR = ((App)Application.Current).templates[idx][0];
        //        RaisePropertyChanged("Points");
        //        return _CentralIncisorR;
        //    }
        //}

        //private TeethType _LateralIncisorR;
        //public TeethType LateralIncisorR
        //{
        //    get
        //    {
        //        int idx = Idx_Templates();
        //        if (idx < 0)
        //            return null;
        //        _LateralIncisorR = ((App)Application.Current).templates[idx][1];
        //        RaisePropertyChanged("Points");
        //        return _LateralIncisorR;
        //    }
        //}

        //private TeethType _CanineR;
        //public TeethType CanineR
        //{
        //    get
        //    {
        //        int idx = Idx_Templates();
        //        if (idx < 0)
        //            return null;
        //        _CanineR = ((App)Application.Current).templates[idx][2];
        //        RaisePropertyChanged("Points");
        //        return _CanineR;
        //    }
        //}
        #endregion

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

            #region IsClosedCurve ����

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

        #region IsMirror
        private bool _IsMirror;
        public bool IsMirror
        {
            get { return _IsMirror; }
            set { _IsMirror = value; }
        }
        #endregion

        #region ShowLength
        private bool _ShowLength;
        public bool ShowLength
        {
            get { return _ShowLength; }
            set
            {
                if (_ShowLength != value)
                {

                }
            }
        }

        #endregion

        #region Data Members of Events

        /// <summary>
        /// Set to 'true' when the left mouse-button is down.
        /// </summary>
        private bool isLeftMouseButtonDownOnWindow = false;

        /// <summary>
        /// Set to 'true' when dragging the 'selection rectangle'.
        /// Dragging of the selection rectangle only starts when the left mouse-button is held down and the mouse-cursor
        /// is moved more than a threshold distance.
        /// </summary>
        private bool isDraggingSelectionRect = false;

        /// <summary>
        /// Records the location of the mouse (relative to the window) when the left-mouse button has pressed down.
        /// </summary>
        private Point origMouseDownPoint;

        /// <summary>
        /// The threshold distance the mouse-cursor must move before drag-selection begins.
        /// </summary>
        private static readonly double DragThreshold = 5;

        /// <summary>
        /// Set to 'true' when the left mouse-button is held down on a rectangle.
        /// </summary>
        private bool isLeftMouseDownOnRectangle = false;

        /// <summary>
        /// Set to 'true' when the left mouse-button and control are held down on a rectangle.
        /// </summary>
        private bool isLeftMouseAndControlDownOnRectangle = false;

        /// <summary>
        /// Set to 'true' when dragging a rectangle.
        /// </summary>
        private bool isDraggingRectangle = false;

        #endregion Data Members

        #region Event            

        private bool IsCaptured = false;

        private RelayCommand<object> _LeftDown;
        public RelayCommand<object> LeftDown
        {
            get
            {
                if (_LeftDown == null)
                    return _LeftDown = new RelayCommand<object>(param => ExecuteMouseLeftDown((MouseEventArgs)param));
                return _LeftDown;
            }
            set { _LeftDown = value; }
        }

        private void ExecuteMouseLeftDown(MouseEventArgs e)
        {
            var sender = e.Source;
            IsCaptured = true;
            origMouseDownPoint = e.GetPosition((IInputElement)e.Source);
            Mouse.Capture((IInputElement)e.Source);
        }

        private RelayCommand<object> _MouseMove;
        public RelayCommand<object> MouseMove
        {
            get
            {
                if (_MouseMove == null) return _MouseMove = new RelayCommand<object>(param => ExecuteMouseMove((MouseEventArgs)param));
                return _MouseMove;
            }
            set { _MouseMove = value; }
        }

        private void ExecuteMouseMove(MouseEventArgs e)
        {

            Teeth Test = (Teeth)e.Source;
            if (IsCaptured == true)
            {
                Point curMouseDownPoint = e.GetPosition((IInputElement)e.Source);
                var dragDelta = curMouseDownPoint - origMouseDownPoint;
                origMouseDownPoint = curMouseDownPoint;

                foreach (PointViewModel point in Test.Points)
                {
                    point.X += (float)dragDelta.X;
                    point.Y += (float)dragDelta.Y;
                }
            }
        }

        private RelayCommand<object> _LeftUp;
        public RelayCommand<object> LeftUp
        {
            get
            {
                if (_LeftUp == null) return _LeftUp = new RelayCommand<object>(param => ExecuteMouseLeftUp((MouseEventArgs)param));
                return _LeftUp;
            }
            set { _LeftUp = value; }
        }

        private void ExecuteMouseLeftUp(MouseEventArgs e)
        {
            IsCaptured = false;
            Mouse.Capture(null);
        }

        #endregion
    }
}