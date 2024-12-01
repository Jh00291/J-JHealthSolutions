using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace J_JHealthSolutions.Model
{
    public class StringToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts a non-empty string to Visibility.Visible and an empty or null string to Visibility.Collapsed.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <param name="targetType">The target binding type.</param>
        /// <param name="parameter">Optional parameter.</param>
        /// <param name="culture">The culture info.</param>
        /// <returns>Visibility based on string content.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value as string;
            return string.IsNullOrWhiteSpace(str) ? Visibility.Collapsed : Visibility.Visible;
        }

        /// <summary>
        /// ConvertBack is not implemented.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
