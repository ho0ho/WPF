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

namespace WPF0603
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Employee> employees;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnClick_find(object sender, RoutedEventArgs e)
        {
            employees = HrDAC.Instance.GetEmployees();
            listEmp.ItemsSource = employees;
        }

        private void btnClick_cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }//스내씨 러쀼러쀼 s2
}
