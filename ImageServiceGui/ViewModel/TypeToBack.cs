using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ImageServiceGui.ViewModel
{
    class TypeToBack : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Brush))
                throw new InvalidOperationException("Must convert to a brush!");
            string type = (string)value;
            if (type == "INFO")
            {
                return Brushes.Green;
            } else if (type == "WARNING")
            {
                return Brushes.Yellow;
            } else
            {
                return Brushes.Red;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Brush))
                throw new InvalidOperationException("Must convert to a brush!");
            string type = (string)value;
            if (type == "INFO")
            {
                return Brushes.Green;
            }
            else if (type == "WARNING")
            {
                return Brushes.Yellow;
            }
            else
            {
                return Brushes.Red;
            }
        }
    }
}
