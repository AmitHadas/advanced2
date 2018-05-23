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
    /// <summary>
    /// Class MsgToColor.
    /// </summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    class MsgToColor : IValueConverter
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        /// <exception cref="System.InvalidOperationException">Must convert to a brush!</exception>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Brush))
                throw new InvalidOperationException("Must convert to a brush!");
            string type = (string)value;
            if (type == "INFO")
            {
                return Brushes.LightGreen;
            } else if (type == "WARNING")
            {
                return Brushes.LightYellow;
            } else
            {
                return Brushes.LightPink;
            }
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        /// <exception cref="System.InvalidOperationException">Must convert to a brush!</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Brush))
                throw new InvalidOperationException("Must convert to a brush!");
            string type = (string)value;
            if (type == "INFO")
            {
                return Brushes.LightGreen;
            }
            else if (type == "WARNING")
            {
                return Brushes.LightYellow;
            }
            else
            {
                return Brushes.LightPink;
            }
        }
    }
}
