using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace J_JHealthSolutions.Model
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BooleanToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean value to a Visibility value.
        /// True returns Visible, False returns Collapsed.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool booleanValue)
            {
                if (parameter as string == "Invert")
                {
                    booleanValue = !booleanValue;
                }

                return booleanValue ? Visibility.Visible : Visibility.Collapsed;
            }

            return Visibility.Collapsed;
        }

        /// <summary>
        /// Converts a Visibility value back to a boolean.
        /// Visible returns True, Collapsed or Hidden returns False.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibilityValue)
            {
                bool result = visibilityValue == Visibility.Visible;

                if (parameter as string == "Invert")
                {
                    result = !result;
                }

                return result;
            }

            return false;
        }
    }
}
