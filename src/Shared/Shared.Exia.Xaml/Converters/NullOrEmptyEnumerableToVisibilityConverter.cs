using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Exia.Xaml {
    public class NullOrEmptyEnumerableToVisibilityConverter : IValueConverter {
        /// <summary>
        ///     Convert a null or empty enumerable to a visibility value.
        /// </summary>
        /// <param name="value">An Enumerable value. </param>
        /// <param name="targetType">This parameter is not used.</param>
        /// <param name="parameter">This parameter is not used.</param>
        /// <param name="culture">This parameter is not used.</param>
        /// <returns>Visibility.Collapse if value is an empty enumerable; otherwise Visibility.Visible</returns>
        /// <exception cref="FormatException">Thrown when value is not an enumerable or collection</exception>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if(value != null) {
                if(value is ICollection) {
                    return (value as ICollection).Count > 0 ? Visibility.Visible : Visibility.Collapsed;
                }
                else if(value is IEnumerable) {
                    return (value as IEnumerable).GetEnumerator().MoveNext() ? Visibility.Visible : Visibility.Collapsed;
                }
                else {
                    throw new FormatException(nameof(value));
                }
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}