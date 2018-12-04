using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exia.Mvvm {
    public class AsyncRelayCommand : AsyncRelayCommandBase {
        public AsyncRelayCommand(Func<Task> func) : this(func, () => true) {
            
        }

        public AsyncRelayCommand(Func<Task> func, Func<bool> canExecute) {
            this.currentTask = func ?? throw new ArgumentNullException(nameof(func));
            this.canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }

        public override async void Execute(object parameter) {
            await this.ExecuteAsync(parameter);
        }

        public override bool CanExecute(object parameter) => this.canExecute();

        public override async Task ExecuteAsync(object parameter) {
            this.IsBusy = true;

            try {
                await this.currentTask();
            }
            catch (Exception) {
                throw;
            }
            finally {
                this.IsBusy = false;
            }
        }

        private readonly Func<bool> canExecute;
        private readonly Func<Task> currentTask;
    }

    public class AsyncRelayCommand<T> : AsyncRelayCommandBase {
        public AsyncRelayCommand(Func<T, Task> func) : this(func, _ => true) {
            
        }

        public AsyncRelayCommand(Func<T, Task> func, Func<T, bool> canExecute) {
            this.currentTask = func ?? throw new ArgumentNullException(nameof(func));
            this.canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }

        public override async void Execute(object parameter) {
            await this.ExecuteAsync(parameter);
        }

        public override bool CanExecute(object parameter) => this.canExecute((T)parameter);

        public override Task ExecuteAsync(object parameter) {
            return this.ExecuteAsync((T)parameter);
        }

        public async Task ExecuteAsync(T parameter) {
            this.IsBusy = true;

            try {
                await this.currentTask(parameter);
            }
            catch (Exception) {
                throw;
            }
            finally {
                this.IsBusy = false;
            }
        }

        private readonly Func<T, Task> currentTask;
        private readonly Func<T, bool> canExecute;
    }
}
