using System;
using System.Windows.Input;

namespace Exia.Mvvm {
    public class RelayCommand : ICommand {
        public RelayCommand(Action action) : this(action, () => true) {
            
        }

        public RelayCommand(Action action, Func<bool> canExecute) {
            this.action = action ?? throw new ArgumentNullException(nameof(action));
            this.canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }

        public bool CanExecute(object parameter) => this.canExecute();

        public void Execute(object parameter) => this.action();

        public void RaiseCanExecuteChanged() {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler CanExecuteChanged;

        private readonly Action action;
        private readonly Func<bool> canExecute;
    }
}
