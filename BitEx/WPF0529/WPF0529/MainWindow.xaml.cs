using System;
using System.Collections.Generic;
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

namespace WPF0529
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //private void KeyEvent(object sender, KeyEventArgs e)
        //{
        //    if ((bool)chkIgnoreRepeat.IsChecked && e.IsRepeat) return;

        //    string message = //"At: " + e.Timestamp.ToString() +
        //        "Event: " + e.RoutedEvent + " " +
        //        " Key: " + e.Key;
        //    lstMessages.Items.Add(message);
        //}

        //private void TextInput(object sender, TextCompositionEventArgs e)
        //{
        //    string message = //"At: " + e.Timestamp.ToString() +
        //        "Event: " + e.RoutedEvent + " " +
        //        " Text: " + e.Text;
        //    lstMessages.Items.Add(message);
        //}

        //private void TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    string message =
        //        "Event: " + e.RoutedEvent;
        //    lstMessages.Items.Add(message);
        //}

        //private void cmdClear_Click(object sender, RoutedEventArgs e)
        //{
        //    lstMessages.Items.Clear();
        //}

        //private void pnl_PreviewTextInput(object sender, TextCompositionEventArgs e)
        //{
        //    short val;
        //    if (!Int16.TryParse(e.Text, out val))
        //    {
        //        e.Handled = true;
        //    }
        //}

        //private void pnl_PreviewKeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Space)
        //    {
        //        e.Handled = true;
        //    }
        //}


        //private void cmdCapture_Click(object sender, RoutedEventArgs e)
        //{
        //    this.AddHandler(
        //           Mouse.LostMouseCaptureEvent,
        //           new RoutedEventHandler(this.LostCapture));
        //    Mouse.Capture(rect);

        //    cmdCapture.Content = "[ Mouse is now captured ... ]";
        //}

        //private void MouseMoved(object sender, MouseEventArgs e)
        //{
        //    Point pt = e.GetPosition(this);
        //    lblInfo.Text =
        //        String.Format($"You are at ({pt.X},{pt.Y}) in window coordinates");
        //}

        //private void LostCapture(object sender, RoutedEventArgs e)
        //{
        //    MessageBox.Show("Lost capture");
        //    cmdCapture.Content = "Capture the Mouse";
        //}

        //private void cmd_SetSmall(object sender, RoutedEventArgs e)
        //{
        //    sliderFontSize.Value = 2;
        //}

        //private void cmd_SetNormal(object sender, RoutedEventArgs e)
        //{
        //    sliderFontSize.Value = this.FontSize;
        //}

        //private void cmd_SetLarge(object sender, RoutedEventArgs e)
        //{
        //    // Only works in two-way mode.
        //    lblSampleText.FontSize = 30;

        //}

        //private void cmd_GetBoundObject(object sender, RoutedEventArgs e)
        //{
        //    Binding binding = BindingOperations.GetBinding(txtBound, TextBox.TextProperty);
        //    MessageBox.Show("Bound " + binding.Path.Path + " to source element " + binding.ElementName);

        //    BindingExpression expression = BindingOperations.GetBindingExpression(txtBound, TextBox.TextProperty);
        //    MessageBox.Show("Bound " + expression.ResolvedSourcePropertyName + " with data " + ((TextBlock)expression.ResolvedSource).FontSize);
        //}

        private void NewCommand(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("New command triggered by " + e.Source.ToString());
        }

        private void cmdDoCommand_Click(object sender, RoutedEventArgs e)
        {
            this.CommandBindings[0].Command.Execute(null);
            // ApplicationCommands.New.Execute(null, (Button)sender);
        }




    }
}
