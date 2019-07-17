using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Ex
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        //public MainWindow()
        //{
        //    InitializeComponent();

        //    AddHandler(CheckBox.CheckedEvent, new RoutedEventHandler(chk_Checked));
        //    AddHandler(CheckBox.UncheckedEvent, new RoutedEventHandler(chk_Unchecked));
        //}

        //private void chk_Checked(object sender, RoutedEventArgs e)
        //{
        //    CheckBox chk = (CheckBox)e.OriginalSource;
        //    DependencyObject obj = LogicalTreeHelper.FindLogicalNode(this, chk.Content.ToString());
        //    ((FrameworkElement)obj).Visibility = Visibility.Visible;
        //}

        //private void chk_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    CheckBox chk = (CheckBox)e.OriginalSource;
        //    DependencyObject obj = LogicalTreeHelper.FindLogicalNode(this, chk.Content.ToString());
        //    ((FrameworkElement)obj).Visibility = Visibility.Collapsed;
        //}

        public MainWindow()
        {
            InitializeComponent();

        }

        protected int eventCounter = 0;
        /// BubbledLabelEvent Ex.
        //private void SomethingClicked(object sender, RoutedEventArgs e)
        //{
        //    eventCounter++;
        //    string message = "#" + eventCounter.ToString() + ":\r\n" +
        //        " Sender: " + sender.ToString() + "\r\n" +
        //        " Source: " + e.Source + "\r\n" +
        //        " Original Source: " + e.OriginalSource;
        //    lstMessages.Items.Add(message);
        //    e.Handled = (bool)chkHandle.IsChecked;
        //}

        //private void cmdClear_Click(object sender, RoutedEventArgs e)
        //{
        //    eventCounter = 0;
        //    lstMessages.Items.Clear();
        //}

        private void SomeKeyPressed(object sender, RoutedEventArgs e)
        {
            eventCounter++;
            string message = "#" + eventCounter.ToString() + ":\r\n" +
                " Sender: " + sender.ToString() + "\r\n" +
                " Source: " + e.Source + "\r\n" +
                " Original Source: " + e.OriginalSource + "\r\n" +
                " Event: " + e.RoutedEvent;
            lstMessages.Items.Add(message);
            e.Handled = (bool)chkHandle.IsChecked;
        }

        private void cmdClear_Click(object sender, RoutedEventArgs e)
        {
            eventCounter = 0;
            lstMessages.Items.Clear();
        }
    }
}
