using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Exia.Mvvm {
    /// <summary>
    /// This class is an implementation of <see cref="INotifyPropertyChanged"/> and <see cref="INotifyPropertyChanging"/>.
    /// When a property value has changes the UI is automatically notified.
    /// </summary>
    public abstract class ObservableObjectBase : INotifyPropertyChanged, INotifyPropertyChanging {
        /// <summary>
        /// Notify client UI that a property value has changed.
        /// </summary>
        /// <param name="propertyName">The property that has a new value</param>
        protected void RaisePropertyChanged([CallerMemberName]string propertyName = null) {
            if(string.IsNullOrEmpty(propertyName)) {
                throw new ArgumentNullException(nameof(propertyName));
            }

            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Notify client UI that a property value is changing.
        /// </summary>
        /// <param name="propertyName">The property that has a new value</param>
        protected void RaisePropertyChanging([CallerMemberName]string propertyName = null) {
            if (string.IsNullOrEmpty(propertyName)) {
                throw new ArgumentNullException(nameof(propertyName));
            }

            this.PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        /// <summary>
        /// Assign a new value to the property if it is different from old value and
        /// notifies clients that a property value has changed.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="field">The field storing the property's value.</param>
        /// <param name="value">Desired value for the property.</param>
        /// <returns>
        /// True if the value has changed otherwise false if the old value is equal to the new value.
        /// </returns>
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null) {
            if (EqualityComparer<T>.Default.Equals(field, value)) {
                return false;
            }

            this.RaisePropertyChanging(propertyName);
            field = value;
            this.RaisePropertyChanged(propertyName);

            return true;
        }

        /// <summary>
        /// Occurs when a property value has changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Occurs when a property value is changing.
        /// </summary>
        public event PropertyChangingEventHandler PropertyChanging;
    }
}