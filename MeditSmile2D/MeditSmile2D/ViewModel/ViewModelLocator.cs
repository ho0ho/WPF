/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:MeditSmile2D"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
//using Microsoft.Practices.ServiceLocation;

namespace MeditSmile2D.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>     
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>

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

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}
            
            
            


            SimpleIoc.Default.Register<MainViewModel>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
    }
}