using MeditSmile2D.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

namespace MeditSmile2D
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 
    using ToothList = ObservableCollection<ObservableCollection<ObservableCollection<PointViewModel>>>;
    using ToothType = ObservableCollection<ObservableCollection<PointViewModel>>;
    using TeethType = ObservableCollection<PointViewModel>;

    public partial class MainWindow : Window
    {

        #region PointsData

        double[,,] fx = {{{ 49.666667, 67.499668, 85.832994, 86.833335, 78.833525, 8.4989281, 0.83212362, 2.1658091, 11.165936, 30.83288 },
                         { 123.33333, 135.50034, 147.50034, 150.50034, 150.167, 137.83367, 94.500336, 86.833669, 92.500642, 104.50058 },
                         { 182, 193.83333, 194.83369, 195.16738, 175.16647, 150.49836, 147.83158, 148.83194, 159.83236, 170.83278 }},

                        {{ 61.333333, 89.166999, 104.16649, 113.4993, 111.16602, 100.16623, 12.501293, 3.8347971, 3.5018047, 24.501328 },
                         { 146.66667, 165.16634, 171.83255, 177.83285, 178.49986, 165.49978, 114.49948, 104.49942, 104.83276, 122.83286 },
                         { 210.33333, 226.83366, 234.50082, 242.83433, 230.50095, 205.16683,  179.49882, 169.49843, 175.16532, 190.16591 }},

                        {{ 64.5, 88, 104, 102.5, 93, 50.5, 12, 1.5, 3, 32.5 },
                         { 156.5, 179, 181.5, 174.5, 165.5, 136, 108.5, 103, 107, 129 },
                         { 232.5, 245.5, 245, 238.5, 223.5, 175, 171.5, 181, 197.5, 210.5 } },

                        {{ 58, 88.5, 108.5, 108.5, 95.5, 49.5, 12.5, 1, 3.5, 27.5 },
                         { 164, 184.5, 191, 185, 175, 120, 109.5, 109.5, 114.5, 135.5 },
                         { 233.5, 246, 245.5, 236.5, 226, 212.5, 181.5, 179.5, 188.5, 211 } },

                        {{ 71, 94.5, 111.5, 111.5, 101.5, 54, 5.5, 0.5, 10.5, 42.5 },
                         { 159, 178, 182, 180.5, 167.5, 141, 112, 114, 120, 141.5 },
                         { 238, 245.5, 240, 232.5, 213.5, 192.5, 181.5, 183.5, 193, 213.5 }}

                        };

        double[,,] fy = {{{ 5.6666667, 19.499667, 62.499665, 107.16633, 123.83261, 125.83294, 118.16659, 65.833136, 36.833071, 13.833018 },
                         { 13.333333, 21.167, 47.832199, 73.498355, 94.497661, 115.16364, 115.49663, 83.164467, 53.498876, 26.499855 },
                         { 0, 13.166667, 44.498875, 86.831137, 110.83096, 94.498257, 74.831953, 48.165778, 24.499548, 7.4998617 }},

                        {{ 25, 44.833333, 87.501046, 126.5017, 152.50213, 170.50242, 172.16912, 163.83533, 112.50126, 54.167085 },
                         { 20.666667, 46.166333, 73.832303, 102.83244, 131.16603, 154.16557, 154.83189, 126.83238, 88.165937, 54.499467 },
                         { 3.6666667, 25.833, 50.83371, 84.168113, 120.83511, 148.50268, 129.83563, 100.16834, 70.8344, 33.833629 }},

                        {{ 25.5, 40, 87, 154.5, 168, 178, 171.5, 148, 81, 34 },
                         { 26.5, 57.5, 102, 141, 160.5, 167, 162.5, 131, 75, 39.5 },
                         { 7, 34.5, 96.5, 134.5, 157, 140.5, 111, 55.5, 36, 18.5 }},

                        {{ 32, 40, 95.5, 142.5, 166, 176.5, 170, 142.5, 85.5, 42 },
                         { 36, 53, 99, 132.5, 148, 152, 141, 109.5, 76.5, 48 },
                         {  2, 21.5, 61, 111, 138.5, 145, 128, 105.5, 45.5, 17 } },

                        {{ 24, 40.5, 93.5, 135.5, 164, 176, 172.5, 119.5, 70.5, 28 },
                         { 26.5, 45.5, 83, 123.5, 147.5, 154.5, 135, 88.5, 63.5, 40 },
                         { 5.5, 35.5, 63.5, 103, 147.5, 140.5, 114.5, 80, 50, 25 } }

                        };
        #endregion 

        public ToothList initList, templates;
        public Dictionary<string, int> dic;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();

            dic = new Dictionary<string, int>();
            dic.Add("CanineL", 5);
            dic.Add("LateralIncisorL", 4);
            dic.Add("CentralIncisorL", 3);
            dic.Add("CentralIncisorR", 0);
            dic.Add("LateralIncisorR", 1);
            dic.Add("CanineR", 2);

            //Point guideline = new Point(canvas.Width / 2, 300);
            initList = new ToothList();
            templates = new ToothList();
            for (int k = 0; k < 5; k++)
            {
                ToothType template = new ToothType();
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
                        teeth.Add(dot);
                    }
                    template.Add(teeth);
                }
                templates.Add(template);
                initList.Add(template);
            }


        }

        private static int i = 0;
        private string fileName = " capture_img.jpeg";        
        private string savePath = "../../results/";
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var rect = new Rect(canvas.RenderSize);
            var visual = new DrawingVisual();
            using (var dc = visual.RenderOpen())
            {
                dc.DrawRectangle(new VisualBrush(canvas), null, rect);
            }

            RenderTargetBitmap renderBitmap = new RenderTargetBitmap(
                (int)rect.Width, (int)rect.Height, 96, 96, PixelFormats.Default);
            renderBitmap.Render(visual);

            FileStream stream = File.Create(savePath + i.ToString() + fileName);
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
            encoder.Save(stream);
            stream.Close();

            MessageBox.Show($"{i++} : 결과가 저장되었습니다.");
        }
    }
}
