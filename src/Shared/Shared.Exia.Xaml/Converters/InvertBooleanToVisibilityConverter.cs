using System;
using System.Windows;
using System.Windows.Data;

namespace Exia.Xaml {
    /// <summary>
    ///     Represents the converter that converts Boolean values to and from Visibility enumeration values. 
    /// </summary>
    public class InvertBooleanToVisibilityConverter : IValueConverter {
        /// <summary>
        ///     Converts a Boolean value to a Visibility enumeration value.
        /// </summary>
        /// <param name="value">The Boolean value to convert. This value can be a standard Boolean value or a nullable Boolean value.</param>
        /// <param name="targetType">This parameter is not used.</param>
        /// <param name="parameter">This parameter is not used.</param>
        /// <param name="culture">This parameter is not used.</param>
        /// <returns>Visibility.Collapsed if value is true; otherwise, Visibility.Visible.</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            bool flag = false;
            if (value is bool) {
                flag = (bool)value;
            }
            else if (value is bool?) {
                bool? nullable = (bool?)value;
                flag = nullable.HasValue ? nullable.Value : false;
            }
            return (flag ? Visibility.Collapsed : Visibility.Visible);
        }

        /// <summary>
        ///     Converts a Visibility enumeration value to a Boolean value.
        /// </summary>
        /// <param name="value">A Visibility enumeration value. </param>
        /// <param name="targetType">This parameter is not used.</param>
        /// <param name="parameter">This parameter is not used.</param>
        /// <param name="culture">This parameter is not used.</param>
        /// <returns>false if value is Visible; otherwise, true.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return !(((value is Visibility) && (((Visibility)value) == Visibility.Visible)));
        }
    }
}
