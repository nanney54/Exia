using System;
using System.Windows.Controls;
using System.Windows.Data;

namespace Exia.Controls.Converters {
    /// <summary>
    ///     Represents the converter that obtain index of <see cref="ListBoxItem"/> in a <see cref="ListBox"/> control.
    ///     It's possible to use parameter to add an integer to the index.
    /// </summary>
    public class ListBoxItemIndexConverter : IValueConverter {
        /// <summary>
        ///     Get the Index of <see cref="ListBoxItem"/>.
        /// </summary>
        /// <param name="value">ListBoxItem where we want index position</param>
        /// <param name="targetType">This parameter is not used.</param>
        /// <param name="parameter">This parameter is used to increment or decrement the index with add operator</param>
        /// <param name="culture">This parameter is not used.</param>
        /// <returns>Return the index of a <see cref="ListBoxItem"/></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            if (parameter == null) {
                parameter = 0;
            }

            if (!(value is ListBoxItem item)) {
                throw new ArgumentException("value must be a ListBoxItem");
            }

            ListBox view = ItemsControl.ItemsControlFromItemContainer(item) as ListBox;

            int index = view.ItemContainerGenerator.IndexFromContainer(item);
            
            if (int.TryParse(parameter.ToString(), out int increment)) {
                index = index + increment;
            }

            return index;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}