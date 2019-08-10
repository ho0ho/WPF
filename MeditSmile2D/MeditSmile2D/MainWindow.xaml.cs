using MeditSmile2D.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MeditSmile2D
{
    using ToothList = ObservableCollection<ObservableCollection<ObservableCollection<PointViewModel>>>;
    using ToothType = ObservableCollection<ObservableCollection<PointViewModel>>;
    using TeethType = ObservableCollection<PointViewModel>;

    public partial class MainWindow : Window
    {
        #region PointsData

        double[,,] UpperX = {{{ 64.333333, 88.500336, 101.167, 109.16659, 100.49974, 15.167815,3.1676365 , 5.1672837, 13.833865, 34.500329 },
                         { 153.66667, 180.49967, 187.16633, 179.83274,169.83284 , 117.1667, 109.83307, 109.83307, 116.50001, 131.16654 },
                         { 226.33333, 238.50033, 237.83367, 240.50034, 213.83366, 186.50097, 179.83439, 185.16797, 194.50114, 207.83424 }},

                        {{ 61, 88.5, 104.5, 113.83333, 105.16695, 8.4996662, 1.8329572, 6.4999729, 20.50002, 37.833412 },
                         { 145, 163.83333, 172.50048, 179.16725, 164.50014, 112.49976, 102.49969, 106.49972,121.83316, 128.49988 },
                         { 211.66667, 233.16634, 242.49919, 231.16603, 206.49975, 175.16691, 170.49997, 176.4999, 187.16646, 198.49967 }},

                        {{ 64.333333, 88.500336, 101.83367, 105.16737, 97.167027, 16.500301, 1.1669568, 0.49995428, 11.833332, 31.833411 },
                         { 155.66667, 178.49967, 180.49966, 176.49967, 166.49967, 111.16635, 99.832633,105.1663, 118.49964, 135.16631 },
                         { 230.33333, 245.16699, 245.83331, 240.50032, 227.167, 172.49905, 168.49899, 172.49905, 191.83271, 207.83299 } },

                        {{ 60.333333, 81.166999, 97.833067,108.49999, 107.16667, 96.500077, 17.833965, 1.1678108, 5.167447, 23.833971 },
                         { 164.33333, 184.50033, 189.167, 182.50009, 178.50015, 119.83424, 107.16813, 111.16774, 122.50092, 141.16733 },
                         { 234.33333, 245.83366, 243.16725, 238.50045, 225.83341, 210.50023, 189.83331, 178.50024, 187.83364, 207.16709 } },

                        {{ 71, 94.5, 111.5, 111.5, 101.5, 54, 5.5, 0.5, 10.5, 42.5 },
                         { 159, 178, 182, 180.5, 167.5, 141, 112, 114, 120, 141.5 },
                         { 238, 245.5, 240, 232.5, 213.5, 192.5, 181.5, 183.5, 193, 213.5 }}

                        };

        double[,,] UpperY = {{{ 9, 33.833333, 63.167391, 122.50082, 164.50113, 169.8345, 142.50125, 103.83422, 59.833808, 26.500164 },
                          { 21, 63.833333, 110.50069, 145.16763, 156.50069, 153.83401, 137.83393, 105.16709 ,65.16689, 35.166738 },
                          { 3, 22.5, 65.833333, 117.16728, 148.50035, 131.16698, 95.833557, 62.500144, 36.500081, 15.833364 }},

                        {{ 27, 55.166667, 102.49911, 138.49868, 160.49843,164.49838 , 141.83198, 82.499347, 53.166359, 35.166571 },
                         { 21, 43.166667, 74.499199, 128.49839, 153.83135, 152.49838, 121.83209,82.499241, 61.166171, 31.8332 },
                         { 4.3333333, 47.167, 84.499709, 119.16611, 147.83213, 125.83265, 103.83284, 65.833154, 43.833338, 14.500249 }},

                        {{ 25.666667, 41.166333, 83.831747, 137.83123, 163.83124, 173.16481, 142.4982, 96.498775, 59.165911, 33.166238 },
                         { 27, 55.166667, 102.49911, 138.49868, 160.49843, 164.49838, 141.83198, 82.499347, 53.166359, 35.166571 },
                         { 7, 32.5, 82.5, 125.83333, 155.83375, 135.83399, 107.83385, 61.833612, 45.83353, 21.833409 }},

                        {{ 34.333333, 33.833665, 57.818067, 90.463834, 143.76251, 165.08238, 171.74439, 129.77189,78.471767 , 47.158704 },
                         { 37.666667, 51.166335, 99.164823, 133.16347,144.49609 ,150.49589 ,125.83005 ,91.83119 ,63.832126, 46.499373 },
                         { 5, 29.166667, 65.832493, 105.16583, 134.49848, 145.83167, 132.49881, 99.832445, 48.499592, 21.833176 } },

                        {{ 24, 40.5, 93.5, 135.5, 164, 176, 172.5, 119.5, 70.5, 28 },
                         { 26.5, 45.5, 83, 123.5, 147.5, 154.5, 135, 88.5, 63.5, 40 },
                         { 5.5, 35.5, 63.5, 103, 147.5, 140.5, 114.5, 80, 50, 25 } }

                        };

        double[,,] LowerX = { {{ 63.333333, 107.50033, 114.167, 106.16672, 78.833564, 62.833669, 42.834261, 18.835021, 10.168872, 21.501669 },
                               { 170, 207.5, 218.83333, 214.16696, 198.83353, 176.16671, 147.49985, 110.83293, 114.16661, 129.5 } } };

        double[,,] LowerY = { { { 12.333333, 15.167, 27.831844, 111.15751, 167.15161, 171.15087, 163.15137, 110.49016, 24.499073, 13.166913 },
                                { 10.333333, 10.333333, 19.167, 98.497008, 149.82841, 157.82847, 149.82906, 81.16466, 26.499799, 11.166972 } } };


        #endregion 

        public ToothList UpperTooth, LowerTooth;
        public Dictionary<string, int> dic;
        public Point UpperGuide, LowerGuide;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();

            // Set data for UpperTooth.
            UpperTooth = new ToothList();
            UpperGuide = new Point(450, 100);
            for (int k = 0; k < 5; k++)
            {
                ToothType template = new ToothType();
                for (int i = 0; i < 6; i++)
                {
                    TeethType teeth = new TeethType();
                    for (int j = 0; j < 10; j++)
                    {
                        if (i >= 0 && i < 3)
                            teeth.Add(new PointViewModel(UpperX[k, i, j] + UpperGuide.X, UpperY[k, i, j] + UpperGuide.Y, j));
                        else
                            teeth.Add(new PointViewModel(-1 * UpperX[k, i - 3, j] + UpperGuide.X, UpperY[k, i - 3, j] + UpperGuide.Y, j));
                    }
                    template.Add(teeth);
                }
                UpperTooth.Add(template);
            }

            // Set data for LowerTooth.
            LowerTooth = new ToothList();
            LowerGuide = new Point(450, 300);
            for (int k = 0; k < 1; k++)
            {
                ToothType tooth = new ToothType();
                for (int i = 0; i < 4; i++)
                {
                    TeethType teeth = new TeethType();
                    for (int j = 0; j < 10; j++)
                    {
                        if (i >= 0 && i < 2)
                            teeth.Add(new PointViewModel(LowerX[k, i, j] + LowerGuide.X, LowerY[k, i, j] + LowerGuide.Y, j));
                        else
                            teeth.Add(new PointViewModel(-1 * LowerX[k, i - 2, j] + LowerGuide.X, LowerY[k, i - 2, j] + LowerGuide.Y, j));
                    }
                    tooth.Add(teeth);
                }
                LowerTooth.Add(tooth);
            }

            // Set global dictionary.
            dic = new Dictionary<string, int>();
            dic.Add("CanineL", 5);
            dic.Add("LateralIncisorL", 4);
            dic.Add("CentralIncisorL", 3);
            dic.Add("CentralIncisorR", 0);
            dic.Add("LateralIncisorR", 1);
            dic.Add("CanineR", 2);
        }

        #region Save

        private static int i = 0;
        private string fileName = " capture_img.jpeg";
        private string savePath = "../../results/";
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var rect = new Rect(Book.RenderSize);
            var visual = new DrawingVisual();
            using (var dc = visual.RenderOpen())
            {
                dc.DrawRectangle(new VisualBrush(Book), null, rect);
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

        #endregion
    }
}
