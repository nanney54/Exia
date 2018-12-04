using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Exia.Mvvm.Sample.ViewModels {
    public class UserViewModel : ViewModelBase {
        public UserViewModel() {
            this.ValidateCommand = new AsyncRelayCommand(this.OnValidateAsync);
        }

        private async Task OnValidateAsync() {
            var client = new System.Net.WebClient();

            string result = await client.DownloadStringTaskAsync("https://jsonplaceholder.typicode.com/photos");
            result += await client.DownloadStringTaskAsync("https://jsonplaceholder.typicode.com/photos");
            result += await client.DownloadStringTaskAsync("https://jsonplaceholder.typicode.com/photos");
            result += await client.DownloadStringTaskAsync("https://jsonplaceholder.typicode.com/photos");
            result += await client.DownloadStringTaskAsync("https://jsonplaceholder.typicode.com/photos");

            await Task.Delay(2000);
        }

        private string login;
        [Required]
        public string Login {
            get { return this.login; }
            set {
                this.SetProperty(ref this.login, value);
            }
        }

        private string email;
        [EmailAddress]
        [Required]
        public string Email {
            get { return this.email; }
            set { this.SetProperty(ref this.email, value); }
        }

        private int age;
        [ValidateAge(ErrorMessage = "Age must be greater than 18")]
        public int Age {
            get { return this.age; }
            set { this.SetProperty(ref this.age, value); }
        }

        public ICommand ValidateCommand { get; }

        private class ValidateAge : ValidationAttribute {
            public override bool IsValid(object value) {
                return (int)value >= 18;
            }
        } 
    }
}