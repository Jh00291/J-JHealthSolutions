using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace J_JHealthSolutions.Model
{
    /// <summary>
    /// Converts enum values to their description attributes for display.
    /// </summary>
    public class EnumDescriptionConverter : IValueConverter
    {
        /// <summary>
        /// Converts an enum value to its description.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        /// <summary>
        /// Converts a description back to the enum value.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Binding.DoNothing;

            foreach (var field in targetType.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == value.ToString())
                        return Enum.Parse(targetType, field.Name);
                }
                else
                {
                    if (field.Name == value.ToString())
                        return Enum.Parse(targetType, field.Name);
                }
            }

            return Binding.DoNothing;
        }
    }
}
