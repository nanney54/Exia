using System;
using System.Windows;
using System.Windows.Data;

namespace Exia.Xaml {
    public class InvertVisibilityToBooleanConverter : IValueConverter {

        /// <summary>
        ///     Converts a Visibility enumeration value to a Boolean value.
        /// </summary>
        /// <param name="value">A Visibility enumeration value. </param>
        /// <param name="targetType">This parameter is not used.</param>
        /// <param name="parameter">This parameter is not used.</param>
        /// <param name="culture">This parameter is not used.</param>
        /// <returns>true if value is Visible; otherwise, false.</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return !((value is Visibility) && (((Visibility)value) == Visibility.Visible));
        }

        /// <summary>
        ///     Converts a Boolean value to a Visibility enumeration value.
        /// </summary>
        /// <param name="value">The Boolean value to convert. This value can be a standard Boolean value or a nullable Boolean value.</param>
        /// <param name="targetType">This parameter is not used.</param>
        /// <param name="parameter">This parameter is not used.</param>
        /// <param name="culture">This parameter is not used.</param>
        /// <returns>Visibility.Visible if value is true; otherwise, Visibility.Collapsed.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            bool flag = false;
            if (value is bool) {
                flag = (bool)value;
            }
            else if (value is bool?) {
                bool? nullable = (bool?)value;
                flag = nullable.HasValue ? nullable.Value : false;
            }
            return (!flag ? Visibility.Visible : Visibility.Collapsed);
        }
    }
}
