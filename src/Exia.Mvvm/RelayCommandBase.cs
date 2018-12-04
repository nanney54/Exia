using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Exia.Mvvm {
    public interface IAsyncCommand : ICommand, INotifyPropertyChanged {
        Task ExecuteAsync(object parameter);
        bool IsBusy { get; }
    }

    public abstract class AsyncRelayCommandBase : IAsyncCommand {
        public abstract bool CanExecute(object parameter);
        public abstract void Execute(object parameter);
        public abstract Task ExecuteAsync(object parameter);

        public void RaiseCanExecuteChanged() {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null) {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool isBusy;
        public bool IsBusy {
            get => this.isBusy;
            set {
                if (this.isBusy != value) {
                    this.isBusy = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public event EventHandler CanExecuteChanged;
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
