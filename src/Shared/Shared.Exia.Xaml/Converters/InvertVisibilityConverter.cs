using System;
using System.Windows;
using System.Windows.Data;

namespace Exia.Controls.Converters {
    public class InvertVisibilityConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return ((value is Visibility) && (((Visibility)value) == Visibility.Visible))
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return this.Convert(value, targetType, parameter, culture);
        }
    }
}
