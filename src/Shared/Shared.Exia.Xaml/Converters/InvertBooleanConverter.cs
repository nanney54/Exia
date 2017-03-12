using System;
using System.Windows.Data;

namespace Exia.Xaml {
    /// <summary>
    ///     Represents the converter that invert Boolean values. 
    /// </summary>
    public class InvertBooleanConverter : IValueConverter {
        /// <summary>
        ///     Invert a boolean value
        /// </summary>
        /// <param name="value">The Boolean value to inverse. This value can be a standard Boolean value or a nullable Boolean value.</param>
        /// <param name="targetType">This parameter is not used.</param>
        /// <param name="parameter">This parameter is not used.</param>
        /// <param name="culture">This parameter is not used.</param>
        /// <returns>Boolean. Return the inverse of in value, If the value is true the result will false else if the value is false the result will true</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            bool flag = false;
            if (value is bool) {
                flag = (bool)value;
            }
            else if (value is bool?) {
                bool? nullable = (bool?)value;
                flag = nullable.HasValue ? nullable.Value : false;
            }

            return !flag;
        }

        /// <summary>
        ///     Invert a boolean value
        /// </summary>
        /// <param name="value">The Boolean value to inverse. This value can be a standard Boolean value or a nullable Boolean value.</param>
        /// <param name="targetType">This parameter is not used.</param>
        /// <param name="parameter">This parameter is not used.</param>
        /// <param name="culture">This parameter is not used.</param>
        /// <returns>Boolean. Return the inverse of in value, If the value is true the result will false else if the value is false the result will true</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return (bool)this.Convert(value, targetType, parameter, culture);
        }
    }
}
