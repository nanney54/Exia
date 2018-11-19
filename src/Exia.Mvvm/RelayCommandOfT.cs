using System;
using System.Windows.Input;

namespace Exia.Mvvm {
    public class RelayCommand<T> : ICommand {
        public RelayCommand(Action<T> action)
            : this(action, e => true) {

        }

        public RelayCommand(Action<T> action, Func<T, bool> canExecute) {
            this.action = action ?? throw new ArgumentNullException(nameof(action));
            this.canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }

        public bool CanExecute(object parameter) {
            return this.canExecute((T)parameter);
        }

        public void Execute(object parameter) {
            this.action((T)parameter);
        }

        public void RaiseCanExecuteChanged() {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        private readonly Action<T> action;
        private readonly Func<T, bool> canExecute;

        public event EventHandler CanExecuteChanged;
    }
}
