using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MeditSmile2D.ViewModel
{
    using TeethType = ObservableCollection<PointViewModel>;
    using ToothType = ObservableCollection<ObservableCollection<PointViewModel>>;
    using TemplatesType = List<ObservableCollection<ObservableCollection<PointViewModel>>>;

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
    public class MainViewModel : ViewModelBase
    {
        // Data of Tooth Templates.
        public TeethType teethdots;
        public ToothType toothdots;
        public TemplatesType templates;
        
        #region Data of Points

        float[,,] fx = { {{ 49.666667f, 67.499668f, 85.832994f, 86.833335f, 78.833525f, 8.4989281f, 0.83212362f, 2.1658091f, 11.165936f, 30.83288f },
                           { 123.33333f, 135.50034f, 147.50034f, 150.50034f, 150.167f, 137.83367f, 94.500336f, 86.833669f, 92.500642f, 104.50058f },
                           { 182f, 193.83333f, 194.83369f, 195.16738f, 175.16647f, 150.49836f, 147.83158f, 148.83194f, 159.83236f, 170.83278f }},

                          {{ 61.333333f, 89.166999f, 104.16649f, 113.4993f, 111.16602f, 100.16623f, 12.501293f, 3.8347971f, 3.5018047f, 24.501328f },
                           { 146.66667f, 165.16634f, 171.83255f, 177.83285f, 178.49986f, 165.49978f, 114.49948f, 104.49942f, 104.83276f, 122.83286f },
                           { 210.33333f, 226.83366f, 234.50082f, 242.83433f, 230.50095f, 205.16683f,  179.49882f, 169.49843f, 175.16532f, 190.16591f }}                  
                        };

        float[,,] fy = { {{ 5.6666667f, 19.499667f, 62.499665f, 107.16633f, 123.83261f, 125.83294f, 118.16659f, 65.833136f, 36.833071f, 13.833018f },
                          { 13.333333f, 21.167f, 47.832199f, 73.498355f, 94.497661f, 115.16364f, 115.49663f, 83.164467f, 53.498876f, 26.499855f },
                          { 0f, 13.166667f, 44.498875f, 86.831137f, 110.83096f, 94.498257f, 74.831953f, 48.165778f, 24.499548f, 7.4998617f }},

                         {{ 25f, 44.833333f, 87.501046f, 126.5017f, 152.50213f, 170.50242f, 172.16912f, 163.83533f, 112.50126f, 54.167085f },
                          { 20.666667f, 46.166333f, 73.832303f, 102.83244f, 131.16603f, 154.16557f, 154.83189f, 126.83238f, 88.165937f, 54.499467f },
                          { 3.6666667f, 25.833f, 50.83371f, 84.168113f, 120.83511f, 148.50268f, 129.83563f, 100.16834f, 70.8344f, 33.833629f }}
                        };

        

        #endregion

        public MainViewModel()
        {
            // initiation of Templates
            templates = new TemplatesType();
            for (int k = 0; k < 2; k++) {                
                toothdots = new ToothType();
                for (int i = 0; i < 6; i++)
                {
                    teethdots = new TeethType();
                    for (int j = 0; j < 10; j++)
                    {
                        //if (i >= 0 && i < 3)
                        //    teethdots.Add(new PointViewModel(fx[i, j], fy[i, j]));
                        //else
                        //    teethdots.Add(new PointViewModel(-1 * fx[i - 3, j], fy[i, j]));
                        teethdots.Add(new PointViewModel(((i >= 0 && i < 3) ? (-1 * fx[k, i - 3, j]) : (fx[k, i, j])), fy[k, i, j]));
                    }
                    toothdots.Add(teethdots);
                }
                templates.Add(toothdots);
            }
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

        private bool _IsTemplate1 = false;
        private string IsTemplate1Name = "IsTemplate1";
        public bool IsTemplate1
        {
            get { return _IsTemplate1; }
            set
            {
                if (_IsTemplate1 != value)
                {
                    _IsTemplate1 = value;
                    SelectedTemplates.Add(_IsTemplate1);
                    RaisePropertyChanged(IsTemplate1Name);
                }
            }
        }

        private bool _IsTemplate2 = false;
        private string IsTemplate2Name = "IsTemplate2";
        public bool IsTemplate2
        {
            get { return _IsTemplate2; }
            set
            {
                if (_IsTemplate2 != value)
                {
                    _IsTemplate2 = value;
                    SelectedTemplates.Add(_IsTemplate2);
                    RaisePropertyChanged(IsTemplate2Name);
                }
            }
        }

        private bool _IsTemplate3 = false;
        private string IsTemplate3Name = "IsTemplate3";
        public bool IsTemplate3
        {
            get { return _IsTemplate3; }
            set
            {
                if (_IsTemplate3 != value)
                {
                    _IsTemplate3 = value;
                    SelectedTemplates.Add(_IsTemplate3);
                    RaisePropertyChanged(IsTemplate3Name);
                }
            }
        }
        private bool _IsTemplate4 = false;
        private string IsTemplate4Name = "IsTemplate4";
        public bool IsTemplate4
        {
            get { return _IsTemplate4; }
            set
            {
                if (_IsTemplate4 != value)
                {
                    _IsTemplate4 = value;
                    SelectedTemplates.Add(_IsTemplate4);
                    RaisePropertyChanged(IsTemplate4Name);
                }
            }
        }

        private bool _IsTemplate5 = false;
        private string IsTemplate5Name = "IsTemplate5";
        public bool IsTemplate5
        {
            get { return _IsTemplate5; }
            set
            {
                if (_IsTemplate5 != value)
                {
                    _IsTemplate5 = value;
                    SelectedTemplates.Add(_IsTemplate5);
                    RaisePropertyChanged(IsTemplate5Name);
                }
            }
        }

        #endregion

        #region Points

        private ToothType _Points;
        public ToothType Points
        {
            get { return _Points ?? (_Points = GetAllPoints()); }
        }

        private ToothType GetAllPoints()
        {
            PointViewModel guideline = new PointViewModel(260, 180);
            for (int k = 0; k < 5; k++)
            {
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        PointViewModel t = templates[k][i][j];
                        t.X += guideline.X;
                        // templatesp[k][i][j].X += guideline.X;
                    }
                }
            }

            // check
            int indexOfTemplates = 0;
            foreach (bool selected in SelectedTemplates)
            {
                if (selected)
                {
                    indexOfTemplates = SelectedTemplates.IndexOf(selected);
                    break;
                }
            }

            return templates[indexOfTemplates];
        }

        #endregion

        #region IsClosedCurve

        private const string IsClosedCurveName = "IsClosedCurve";
        public bool IsClosedCurve
        {
            get { return true; }

            #region IsClosedCurve ¿©ºÎ

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