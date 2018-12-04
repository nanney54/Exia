using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xunit;
using Exia.Mvvm;
using System.Threading.Tasks;

namespace Exia.Mvvm.Test {
    public class RelayCommandTest {
        [Fact]
        public void Execute_NonNullAction() {
            //Assign
            bool isExecuted = false;

            ICommand command = new RelayCommand(() => isExecuted = true);

            //Act
            command.Execute(null);

            //Assert
            Assert.True(isExecuted);
        }

        [Fact]
        public void CanExecute_WithCanExecuteTrue_ShouldReturnTrue() {
            //Assign
            ICommand command = new RelayCommand(() => { }, () => true);

            //Act
            bool result = command.CanExecute(null);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void CanExecute_WithCanExecuteFalse_ShouldReturnFalse() {
            //Assign
            ICommand command = new RelayCommand(() => { }, () => false);

            //Act
            bool result = command.CanExecute(null);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void Execute_WithStringParameter() {
            //Assign
            string sentence = "Hello world !";
            bool isExecuted = false;

            ICommand command = new RelayCommand<string>(s => isExecuted = (s == sentence));

            //Act
            command.Execute(sentence);

            //Assert
            Assert.True(isExecuted);
        }

        [Fact]
        public void Execute_WithNullParameter() {
            //Assign
            string sentence = null;
            bool isExecuted = false;

            ICommand command = new RelayCommand<string>(s => isExecuted = (s == sentence));

            //Act
            command.Execute(sentence);

            //Assert
            Assert.True(isExecuted);
        }

        [Fact]
        public void Execute_WithIntegerParameter() {
            //Assign
            int number = 42;
            bool isExecuted = false;

            ICommand command = new RelayCommand<int>(s => isExecuted = (s == number));

            //Act
            command.Execute(number);

            //Assert
            Assert.True(isExecuted);
        }

        [Fact]
        public void Execute_WithNullableIntegerParameter() {
            //Assign
            int? number = 42;
            bool isExecuted = false;

            ICommand command = new RelayCommand<int?>(s => isExecuted = (s == number));

            //Act
            command.Execute(number);

            //Assert
            Assert.True(isExecuted);
        }

        [Fact]
        public void CanExecute_WithStringParameter() {
            //Assign
            string sentence = "Hello world !";

            ICommand command = new RelayCommand<string>(s => { }, s => !string.IsNullOrEmpty(s));

            //Act
            bool result = command.CanExecute(sentence);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void CanExecute_WithNullParameter() {
            //Assign
            string sentence = null;

            ICommand command = new RelayCommand<string>(s => { }, s => !string.IsNullOrEmpty(s));

            //Act
            bool result = command.CanExecute(sentence);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void CanExecute_WithIntegerParameter() {
            //Assign
            int number = 42;

            ICommand command = new RelayCommand<int>(n => { }, n => n == number);

            //Act
            bool result = command.CanExecute(number);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void CanExecute_WithNullableIntegerParameter() {
            //Assign
            int? number = null;

            ICommand command = new RelayCommand<int?>(n => { }, n => n == number);

            //Act
            bool result = command.CanExecute(number);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void Execute_WithoutParameter_RaisedIsBusyProperty() {
            //Assign
            bool isExecuted = false;

            RelayCommand command = new RelayCommand(() => isExecuted = true);

            //Act
            command.Execute(null);

            //Assert
            Assert.True(isExecuted);
        }

        [Fact]
        public async Task Execute_AsyncFunction_RaisedIsBusyProperty() {
            //Assign
            bool isExecuted = false;
            int countChange = 0;
            Task func() => Task.Factory.StartNew(() => isExecuted = true);

            AsyncRelayCommand command = new AsyncRelayCommand(func);

            command.PropertyChanged += (o, e) => {
                if (e.PropertyName == nameof(IAsyncCommand.IsBusy)) {
                    countChange++;
                }
            };

            //Act
            await command.ExecuteAsync(null);

            //Assert
            Assert.True(isExecuted);
            Assert.Equal(2, countChange);
        }

        [Fact]
        public async Task Execute_AsyncFunctionOfT_RaisedIsBusyProperty() {
            //Assign
            bool isExecuted = false;
            int countChange = 0;
            Task func<T>(T parameter) => Task.Factory.StartNew(() => {
                isExecuted = parameter is bool;
            });

            AsyncRelayCommand<bool> command = new AsyncRelayCommand<bool>(func);

            command.PropertyChanged += (o, e) => {
                if (e.PropertyName == nameof(IAsyncCommand.IsBusy)) {
                    countChange++;
                }
            };

            //Act
            await command.ExecuteAsync(true);

            //Assert
            Assert.True(isExecuted);
            Assert.Equal(2, countChange);
        }
    }
}
