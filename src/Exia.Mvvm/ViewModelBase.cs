using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Exia.Mvvm {
    public abstract class ViewModelBase : ObservableObjectBase, INotifyDataErrorInfo {
        public ViewModelBase() {
            this.internalErrors = new ConcurrentDictionary<string, List<ValidationResult>>();
        }

        private void AddModelError(string propertyName, List<ValidationResult> newPropertyErrors) {
            if (string.IsNullOrEmpty(propertyName)) {
                throw new ArgumentNullException(nameof(propertyName));
            }

            if (newPropertyErrors == null) {
                throw new ArgumentNullException(nameof(newPropertyErrors));
            }

            this.internalErrors.AddOrUpdate(
                propertyName,
                newPropertyErrors,
                (key, errors) => newPropertyErrors.Except(errors).ToList()
            );

            if (this.internalErrors[propertyName].Count == 0) {
                this.internalErrors.TryRemove(propertyName, out List<ValidationResult> result);
            }

            this.RaiseErrorsChanged(propertyName);
        }

        private void AddModelError(List<ValidationResult> errors) {
            var validationErrors = (
                from vr in errors
                from mn in vr.MemberNames
                group new { mn, vr } by mn into g
                select new {
                    PropertyName = g.Key,
                    Errors = g.Select(v => v.vr).ToList()
                }
            );

            foreach (var item in validationErrors) {
                this.AddModelError(item.PropertyName, item.Errors);
            }
        }

        private void RemoveModelError(string propertyName) {
            if (this.internalErrors.TryRemove(propertyName, out List<ValidationResult> errors)) {
                this.RaiseErrorsChanged(propertyName);
            }
        }

        protected void ClearErrors() {
            foreach (var item in this.internalErrors) {
                this.RemoveModelError(item.Key);
            }
        }

        /// <summary>
        /// Validates the property.
        /// </summary>
        /// <param name="propertyName">The name of property</param>
        /// <param name="value">The value to validate.</param>
        /// <returns>true if the property validates; otherwise, false.</returns>
        protected bool TryValidateProperty(string propertyName, object value) {
            this.RemoveModelError(propertyName);

            ValidationContext validationContext = new ValidationContext(this) {
                MemberName = propertyName
            };

            List<ValidationResult> validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateProperty(value, validationContext, validationResults);
            
            if(!isValid) {
                this.AddModelError(propertyName, validationResults);
            }

            return isValid;
        }

        /// <summary>
        /// Assign a new value to the property if it is different from old value and validation
        /// conditions are met.
        /// Notifies clients that a property value has changed.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="field">The field storing the property's value.</param>
        /// <param name="value">Desired value for the property.</param>
        /// <param name="propertyName">The name of property</param>
        /// <returns>True if the value has changed otherwise false if the old value is equal to the new value.</returns>
        protected override bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null) {
            return !this.IsSameValue(field, value)
                && base.SetProperty(ref field, value, propertyName)
                && this.TryValidateProperty(propertyName, value);
        }

        /// <summary>
        /// Called when the errors have changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void RaiseErrorsChanged([CallerMemberName] string propertyName = null) {
            if (string.IsNullOrEmpty(propertyName)) {
                throw new ArgumentNullException(nameof(propertyName));
            }

            this.RaisePropertyChanged(nameof(this.HasErrors));
            this.RaisePropertyChanged(nameof(this.Errors));

            this.OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
        }

        protected virtual void OnErrorsChanged(DataErrorsChangedEventArgs e) {
            this.ErrorsChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Gets the validation errors for a specified property.
        /// </summary>
        /// <param name="propertyName">Name of the property to retrieve errors.
        /// <returns>A collection of errors. If property doesn't exist return an Empty Collection</returns>
        public IEnumerable GetErrors(string propertyName) {
            return this.internalErrors.TryGetValue(propertyName, out List<ValidationResult> errors)
                ? errors
                : Enumerable.Empty<ValidationResult>();
        }

        public Task<bool> IsModelValidAsync {
            get {
                return Task.Run(() => {
                    this.ClearErrors();

                    ValidationContext vc = new ValidationContext(this);
                    List<ValidationResult> results = new List<ValidationResult>();

                    bool isValid = Validator.TryValidateObject(this, vc, results, true);

                    if(!isValid) {
                        this.AddModelError(results);
                    }
                    
                    return isValid;
                });
            }
        }

        /// <summary>
        /// Gets a value indicating whether the object has validation errors. 
        /// </summary>
        /// <value>true if this instance has errors, otherwise false</value>
        public bool HasErrors => this.internalErrors.Count > 0;

        /// <summary>
        /// Gets the validation errors for the entire object.
        /// </summary>
        /// <returns>A collection of errors.</returns>
        public IEnumerable<string> Errors 
            => this.internalErrors.SelectMany(e => e.Value).Select(e => e.ErrorMessage);

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private readonly ConcurrentDictionary<string, List<ValidationResult>> internalErrors;
    }
}