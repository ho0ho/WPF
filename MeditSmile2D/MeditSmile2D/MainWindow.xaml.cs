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
    using TemplateType = ObservableCollection<ObservableCollection<ObservableCollection<PointViewModel>>>;
    using ToothType = ObservableCollection<ObservableCollection<PointViewModel>>;
    using TeethType = ObservableCollection<PointViewModel>;

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();

            Point guideline = new Point(canvas.Width / 2, 300);
            TemplateType templates = ((App)Application.Current).templates;
            foreach (ToothType tooth in templates)
            {
                foreach (TeethType teeth in tooth)
                {
                    foreach (PointViewModel point in teeth)
                    {
                        point.X += (float)guideline.X;
                        point.Y += (float)guideline.Y;
                    }
                }
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

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.InitialDirectory = "D:\\ho-ho\\WPF_\\MeditSmile2D\\MeditSmile2D\\images\\";
            dlg.Title = "이미지 불러오기";
            dlg.DefaultExt = ".bmp|.jpg|.jpeg|.png";
            dlg.Filter = "Image Files (*.bmp, *.jpg, *.jpeg, *.png) | *.bmp; *.jpg; *jpeg; *.png";

            if (dlg.ShowDialog() == true)
                img.ImageSource = new BitmapImage(new Uri(dlg.FileName));
        }
    }
}
