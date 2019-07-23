using MeditSmile2D.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MeditSmile2D
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 

    using TemplatesType = ObservableCollection<ObservableCollection<ObservableCollection<PointViewModel>>>;
    using ToothType = ObservableCollection<ObservableCollection<PointViewModel>>;
    using TeethType = ObservableCollection<PointViewModel>;
    public partial class App : Application
    {
        #region Data of Points

        float[,,] fx = {{{ 49.666667f, 67.499668f, 85.832994f, 86.833335f, 78.833525f, 8.4989281f, 0.83212362f, 2.1658091f, 11.165936f, 30.83288f },
                         { 123.33333f, 135.50034f, 147.50034f, 150.50034f, 150.167f, 137.83367f, 94.500336f, 86.833669f, 92.500642f, 104.50058f },
                         { 182f, 193.83333f, 194.83369f, 195.16738f, 175.16647f, 150.49836f, 147.83158f, 148.83194f, 159.83236f, 170.83278f }},

                        {{ 61.333333f, 89.166999f, 104.16649f, 113.4993f, 111.16602f, 100.16623f, 12.501293f, 3.8347971f, 3.5018047f, 24.501328f },
                         { 146.66667f, 165.16634f, 171.83255f, 177.83285f, 178.49986f, 165.49978f, 114.49948f, 104.49942f, 104.83276f, 122.83286f },
                         { 210.33333f, 226.83366f, 234.50082f, 242.83433f, 230.50095f, 205.16683f,  179.49882f, 169.49843f, 175.16532f, 190.16591f }},

                        {{ 64.5f, 88f, 104f, 102.5f, 93f, 50.5f, 12f, 1.5f, 3f, 32.5f },
                         { 156.5f, 179f, 181.5f, 174.5f, 165.5f, 136f, 108.5f, 103f, 107f, 129f },
                         { 232.5f, 245.5f, 245f, 238.5f, 223.5f, 175f, 171.5f, 181f, 197.5f, 210.5f } },

                        {{ 58f, 88.5f, 108.5f, 108.5f, 95.5f, 49.5f, 12.5f, 1f, 3.5f, 27.5f },
                         { 164f, 184.5f, 191f, 185f, 175f, 120f, 109.5f, 109.5f, 114.5f, 135.5f },
                         { 233.5f, 246f, 245.5f, 236.5f, 226f, 212.5f, 181.5f, 179.5f, 188.5f, 211f } },

                        {{ 71f, 94.5f, 111.5f, 111.5f, 101.5f, 54f, 5.5f, 0.5f, 10.5f, 42.5f },
                         { 159f, 178f, 182f, 180.5f, 167.5f, 141f, 112f, 114f, 120f, 141.5f },
                         { 238f, 245.5f, 240f, 232.5f, 213.5f, 192.5f, 181.5f, 183.5f, 193f, 213.5f }}

                        };

        float[,,] fy = {{{ 5.6666667f, 19.499667f, 62.499665f, 107.16633f, 123.83261f, 125.83294f, 118.16659f, 65.833136f, 36.833071f, 13.833018f },
                         { 13.333333f, 21.167f, 47.832199f, 73.498355f, 94.497661f, 115.16364f, 115.49663f, 83.164467f, 53.498876f, 26.499855f },
                         { 0f, 13.166667f, 44.498875f, 86.831137f, 110.83096f, 94.498257f, 74.831953f, 48.165778f, 24.499548f, 7.4998617f }},

                        {{ 25f, 44.833333f, 87.501046f, 126.5017f, 152.50213f, 170.50242f, 172.16912f, 163.83533f, 112.50126f, 54.167085f },
                         { 20.666667f, 46.166333f, 73.832303f, 102.83244f, 131.16603f, 154.16557f, 154.83189f, 126.83238f, 88.165937f, 54.499467f },
                         { 3.6666667f, 25.833f, 50.83371f, 84.168113f, 120.83511f, 148.50268f, 129.83563f, 100.16834f, 70.8344f, 33.833629f }},

                        {{ 25.5f, 40f, 87f, 154.5f, 168f, 178f, 171.5f, 148f, 81f, 34f },
                         { 26.5f, 57.5f, 102f, 141f, 160.5f, 167f, 162.5f, 131f, 75f, 39.5f },
                         { 7f, 34.5f, 96.5f, 134.5f, 157f, 140.5f, 111f, 55.5f, 36f, 18.5f }},

                        {{ 32f, 40f, 95.5f, 142.5f, 166f, 176.5f, 170f, 142.5f, 85.5f, 42f },
                         { 36f, 53f, 99f, 132.5f, 148f, 152f, 141f, 109.5f, 76.5f, 48f },
                         {  2f, 21.5f, 61f, 111f, 138.5f, 145f, 128f, 105.5f, 45.5f, 17f } },

                        {{ 24f, 40.5f, 93.5f, 135.5f, 164f, 176f, 172.5f, 119.5f, 70.5f, 28f },
                         { 26.5f, 45.5f, 83f, 123.5f, 147.5f, 154.5f, 135f, 88.5f, 63.5f, 40f },
                         { 5.5f, 35.5f, 63.5f, 103f, 147.5f, 140.5f, 114.5f, 80f, 50f, 25f } }

                        };


        #endregion

        public TemplatesType templates;
        public ToothType template;

        public CheckBox cb_mirror;
        public Canvas canvas;

        public Dictionary<string, int> dic;

        public App()
        {
            dic = new Dictionary<string, int>();
            dic.Add("CanineL", 5);
            dic.Add("LateralIncisorL", 4);
            dic.Add("CentralIncisorL", 3);
            dic.Add("CentralIncisorR", 0);
            dic.Add("LateralIncisorR", 1);
            dic.Add("CanineR", 2);
            
            // PointViewModel guideline = new PointViewModel(500, 300, 0);
            templates = new TemplatesType();       
            for (int k = 0; k < 5; k++)
            {
                template = new ToothType();
                for (int i = 0; i < 6; i++)
                {
                    TeethType teeth = new TeethType();
                    for (int j = 0; j < 10; j++)
                    {
                        PointViewModel dot;
                        if (i >= 0 && i < 3)
                        {
                            dot = new PointViewModel(fx[k, i, j], fy[k, i, j], j);
                        }
                        else
                        {
                            dot = new PointViewModel(-1 * fx[k, i - 3, j], fy[k, i - 3, j], j);
                        }
                        //dot.X += guideline.X;
                        //dot.Y += guideline.Y;
                        teeth.Add(dot);
                    }
                    template.Add(teeth);
                }
                templates.Add(template);
            }
        }

    }
}
