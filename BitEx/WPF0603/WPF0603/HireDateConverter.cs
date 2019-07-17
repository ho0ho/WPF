using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WPF0603
{
    [ValueConversion(typeof(DateTime), typeof(string))]
    public class HireDateConverter : IValueConverter

    {
        /// DB에서 값 읽어오면서 변환(순)
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime hiredate = (DateTime)value;
            return hiredate.ToShortDateString();
        }

        /// DB로 값을 업데이하기 전에 변환(역)
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string hiredate = value as string;
            DateTime result;
            if (DateTime.TryParse(hiredate, out result))
            {
                return result;
            }
            return value;
        }
    }
}
