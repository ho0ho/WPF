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

namespace MeditSmile2D.View
{
    /// <summary>
    /// ToothTemplate.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ToothTemplate : UserControl
    {
        public ToothTemplate()
        {
            InitializeComponent();

            Binding binding = new Binding("Main");
            binding.Source = FindResource("Locator");
            SetBinding(DataContextProperty, binding);
        }
    }
}
