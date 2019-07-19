using GalaSoft.MvvmLight;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

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
        private string IsTemplate0Name = "IsTemplate0";
        public bool IsTemplate0
        {
            get { return _IsTemplate0; }
            set
            {
                if (_IsTemplate0 != value)
                {
                    _IsTemplate0 = value;
                    SelectedTemplates[0] = value;
                    RaisePropertyChanged(IsTemplate0Name);

                    /// 1.
                    // IsTemplate값이 변경되었을 때 
                    // Points 프로퍼티를 재구성해서 Teeth.Points를 다시 그려주도록
                    // "Points"라는 이름의 프로퍼티를 찾음(=> MainViewModel.Points를 호출)       
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
            // 템플릿이 선택되지 않은 초기상태에서는 GetAllPoints()에서 null값이 리턴
            //      이 경우 Teeth.Points가 변경되지 않음 => 각 PointViewModel객체들의 OnPropertyChanged가 발생하지 않음
            // 템플릿이 선택되어 GetAllPoints()에서 non-null이 리턴된 경우 
            // (리턴된 후) Teeth.Points 프로퍼티가 Set 되기 때문에 Teeth.PropertyChangedCallback()로 넘어감
            // 넘어가는 데이터는 ToothType(즉 10개의 점 세트가 6개 있는 Tooth로 넘겨지나, 
            // <Teeth Points="{Binding Points[0]}"> ... 이므로 
            // Teeth.Points 가 받는 실제 데이터는 TeethType의 Points[n]으로 넘어감
            get { return _Points = GetAllPoints(); }
        }

        private ToothType GetAllPoints()
        {
            int idx = Idx_Templates();
            if (idx < 0)
                return null;
            return ((App)Application.Current).templates[idx];
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
    }
}