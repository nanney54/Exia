using System.ComponentModel.DataAnnotations;

namespace Exia.Mvvm.Sample.ViewModels {
    public class UserViewModel : ViewModelBase {
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

        private class ValidateAge : ValidationAttribute {
            public override bool IsValid(object value) {
                return (int)value >= 18;
            }
        } 
    }
}