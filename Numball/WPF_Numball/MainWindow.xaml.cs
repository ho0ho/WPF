using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
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
using System.Windows.Threading;

using OpenCvSharp;
using Window = System.Windows.Window;

namespace WPF_Numball
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        private string number;
        private List<int> answer;
        private int dimension;
        private int round = 0;

        public class NumBall
        {
            public int Round { get; set; }
            public string Input { get; set; }
            public int Strike { get; set; }
            public int Ball { get; set; }
            public string Out { get; set; }
        }

        public MainWindow()
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(ResolveAssembly);

            InitializeComponent();

        }

        static Assembly ResolveAssembly(object sender, ResolveEventArgs args)
        {
            Assembly thisAssembly = Assembly.GetExecutingAssembly();
            var name = args.Name.Substring(0, args.Name.IndexOf(',')) + ".dll";

            var resources = thisAssembly.GetManifestResourceNames().Where(s => s.EndsWith(name));
            if (resources.Count() > 0)
            {
                string resourceName = resources.First();
                using (Stream stream = thisAssembly.GetManifestResourceStream(resourceName))
                {
                    if (stream != null)
                    {
                        byte[] assembly = new byte[stream.Length];
                        stream.Read(assembly, 0, assembly.Length);
                        Console.WriteLine("Dll file load : " + resourceName);
                        return Assembly.Load(assembly);
                    }
                }
            }
            return null;
        }


        private void Click_Enter(object sender, RoutedEventArgs e)
        {
            if (btnStart.IsEnabled == true)
            {
                MessageBox.Show("시작버튼을 먼저 눌러주세요!");
                return;
            }

            if (numballValue.Text == "" || numballValue.Text.Length < dimension)
            {
                MessageBox.Show("숫자를 입력해주세요!");
                return;
            }

            number = numballValue.Text;
            List<int> userIn = new List<int>();

            // 중복검사
            foreach (char c in number)
                userIn.Add(int.Parse(c.ToString()));
            if (Check_Dup(userIn))
            {
                MessageBox.Show("Try Again!");
                numballValue.Text = "";
                return;
            }

            // update
            NumBall nb = updateData(userIn);            // Add()

            // check for answer
            if (nb.Strike == dimension)
                check_answer(nb);
        }

        private void capturePlay(ref string path)
        {
            double captureWidth = resultView.ActualWidth;
            double captureHeight = resultView.ActualHeight;

            System.Windows.Point curPoint = resultView.TransformToAncestor(this).Transform(new System.Windows.Point(0, 0));
            var startPoint = PointToScreen(curPoint);

            using (Bitmap bmp = new Bitmap((int)(captureWidth), (int)(captureHeight)))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    String userID = name.Text;
                    String filename = "D:\\BaseBall-" + userID + " " + DateTime.Now.ToString("yyyy-MM-dd HH시 mm분 ss초") + ".png";
                    //Opacity = .0;
                    /* Graphics.CopyFromScreen(int srcX, int srcY, int tarX, int tarY, Size bmp_sz)
                     *  Screen의 (srcX, srcY)좌표에서부터 bmp_sz만큼 이미지를 떠서(캡쳐하여) 메모리에 저장한 다음,
                     *  메모리에 저장한 이미지를 Target 이미지(Bitmap) 안에서의 (tarX, tarY) 좌표에 붙여넣는다.           
                     *      결국 => Graphics.CopyFromScreen(int screenX, int screenY, int wndX, int wndY, Bitmap.Size) */
                    g.CopyFromScreen((int)startPoint.X, (int)startPoint.Y, 0, 0, bmp.Size);
                    //g.CopyFromScreen((int)curPoint.X, (int)curPoint.Y, 0, 0, bmp.Size);
                    bmp.Save(filename);
                    //Opacity = 1;
                    path = filename;
                }
            }

        }

       private void check_answer(NumBall nb)
        {
            string msg = String.Format($"성공! {round}회차 만에 성공하셨습니다!");
            MessageBox.Show(msg);

            // 경기결과 저장(캡쳐)
            if (check_capture.IsChecked)
            {
                string path = null;
                capturePlay(ref path);
                MessageBox.Show("기록이 저장되었습니다.\n저장위치: " + path);
            }


            // 세팅 초기화
            round = 0;
            btnStart.IsEnabled = true;            
            cbDim.IsEnabled = true;
            check_capture.IsChecked = false;
            name.IsEnabled = true;
            foreach (Control ctl in grid_num.Children)
                if (ctl is CheckBox)
                    ((CheckBox)ctl).IsChecked = false;
            //resultView.Items.Clear();
        }

        private NumBall updateData(List<int> userIn)
        {
            NumBall nb = Check(userIn);
            resultView.Items.Add(nb);

            // list(DataGrid) 스크롤 아래로 내리기
            if (resultView.Items.Count > 0)
            {
                var border = VisualTreeHelper.GetChild(resultView, 0) as Decorator;
                if (border != null)
                {
                    var scroll = border.Child as ScrollViewer;
                    if (scroll != null) scroll.ScrollToEnd();
                }
            }

            // 입력창 리셋
            numballValue.Text = "";
            return nb;
        }

        private bool Check_Dup(List<int> putNum)
        {

            for (int i = 0; i < putNum.Count; i++)
                for (int j = i + 1; j < putNum.Count; j++)
                    if (putNum.ElementAt(i) == putNum.ElementAt(j))
                        return true;
            return false;
        }

        private NumBall Check(List<int> userIn)
        {
            NumBall nb = new NumBall() { Round = 0, Strike = 0, Ball = 0, Out = "Safe" };
            nb.Input = number;
            nb.Round = ++round;
            foreach (int u in userIn)
            {
                foreach (int a in answer)
                {
                    if (u == a)
                    {
                        if (userIn.IndexOf(u) == answer.IndexOf(a))
                            nb.Strike++;
                        else nb.Ball++;
                        break;
                    }

                }
            }

            if (nb.Strike == 0 && nb.Ball == 0)
                nb.Out = "Out";

            return nb;
        }

        private void Click_Start(object sender, RoutedEventArgs e)
        {
            btnStart.IsEnabled = false;
            cbDim.IsEnabled = false;
            overlapMode.IsEnabled = false;
            if (name.Text == "")
                name.Text = "Unknown";
            name.IsEnabled = false;

            dimension = int.Parse(cbDim.Text);
            answer = new List<int>();
            makeAnswer(dimension);

        }

        private void makeAnswer(int dim)
        {
            Random r = new Random();
            int cnt = 0;

            int flag = 0;
            while (cnt < dimension)
            {
                int num = r.Next(0, 10);
                foreach (int a in answer)
                {
                    if (a == num)
                    {
                        flag = 1;
                        break;
                    }
                }

                if (flag == 0)
                {
                    answer.Add(num);
                    cnt++;
                }
                flag = 0;
            }
        }

        private void Click_Reset(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"포기하시나요? ㅠㅠ 좀만 더 해보세요...", "초기화", MessageBoxButton.YesNo, 
                MessageBoxImage.Exclamation, MessageBoxResult.None, MessageBoxOptions.DefaultDesktopOnly) 
                == MessageBoxResult.Yes)
            {
                e.Handled = true;
            }
            else
            {
                string text = "정답은 ";
                foreach (int a in answer)
                    text += (a.ToString() + " ");

                MessageBox.Show($"{text}였습니다 희희");

                btnStart.IsEnabled = true;
                cbDim.IsEnabled = true;
                overlapMode.IsEnabled = true;

                round = 0;
                resultView.Items.Clear();
                foreach (Control ctl in grid_num.Children)
                    if (ctl is CheckBox)
                        ((CheckBox)ctl).IsChecked = false;
            }
        }

        private void Click_Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private void prvTextInput(object sender, TextCompositionEventArgs e)
        {
            if (numballValue.Text.Length >= dimension)
            {
                e.Handled = true;
            }

            foreach (char c in e.Text)
            {
                if (!char.IsDigit(c))
                {
                    e.Handled = true;
                    break;
                }
            }
        }

        private void Click_Tip(object sender, RoutedEventArgs e)
        {
            //BitmapImage img = new BitmapImage(new Uri("/Resources/tip.png", UriKind.RelativeOrAbsolute));
            Bitmap img = Properties.Resources.tip;
            Window w = new Window();
            
            w.Show();
            
        }

        private void Click_record(object sender, RoutedEventArgs e)
        {
            //
        }
    }
}
