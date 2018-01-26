using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Exia.Mvvm.Test {
    public class ValidationObjectBaseTest {
        [Fact]
        public void Errors_are_notified() {
            bool hasChanged = false;

            ViewModelBaseMock vm = new ViewModelBaseMock();
            vm.ErrorsChanged += (o, e) => {
                hasChanged = e.PropertyName == nameof(vm.IntegerBetweenZeroAndTen);
            };

            vm.IntegerBetweenZeroAndTen = 42;

            Assert.True(hasChanged);
        }

        [Fact]
        public async Task GetErrors_for_specified_property_Test() {
            ViewModelBaseMock vm = new ViewModelBaseMock() {
                TestProperty1 = "a"
            };

            await vm.IsModelValidAsync;

            IEnumerable<ValidationResult> errors = vm.
                GetErrors(nameof(vm.TestProperty1))
                .Cast<ValidationResult>();

            Assert.Equal(
                (new string[] { PatternError, ErrorMessage1 }).OrderBy(e => e),
                errors.Select(e => e.ErrorMessage).OrderBy(e => e)
            );
        }

        [Fact]
        public async Task GetAllErrors_Test() {
            ViewModelBaseMock vm = new ViewModelBaseMock() {
                TestProperty1 = "a",
                TestProperty2 = "b",
            };

            await vm.IsModelValidAsync;

            Assert.Equal(
                (new string[] { PatternError, ErrorMessage1, PatternError, ErrorMessage2 }).OrderBy(e => e), 
                vm.Errors.OrderBy(e => e)
            );
        }

        [Fact]
        public void HasErrors_return_true_Test() {
            ViewModelBaseMock vm = new ViewModelBaseMock() {
                IntegerBetweenZeroAndTen = 20
            };

            Assert.True(vm.HasErrors);
        }

        [Fact]
        public void HasErrors_return_false_Test() {
            ViewModelBaseMock vm = new ViewModelBaseMock() {
                IntegerBetweenZeroAndTen = 4
            };

            Assert.False(vm.HasErrors);
        }

        [Fact]
        public async Task IsModelValidAsync_with_errors_Test() {
            ViewModelBaseMock vm = new ViewModelBaseMock() {
                TestProperty1 = "t",
                IntegerBetweenZeroAndTen = 42,
                TestProperty2 = "a"
            };

            bool notValidResult = await vm.IsModelValidAsync;

            Assert.False(notValidResult);
            Assert.Equal(5, vm.Errors.Count());
        }

        [Fact]
        public async Task IsModelValidAsync_with_no_errors_Test() {
            ViewModelBaseMock vm = new ViewModelBaseMock() {
                TestProperty1 = "$1.13",
                IntegerBetweenZeroAndTen = 4,
                IntegerBetweenZeroAndFifty = 42
            };

            bool validResult = await vm.IsModelValidAsync;

            Assert.True(validResult);
            Assert.False(vm.HasErrors);
        }

        [Fact]
        public void TryValidateProperty_with_error_Test() {
            ViewModelBaseMock vm = new ViewModelBaseMock() {
                IntegerBetweenZeroAndTen = 42
            };

            Assert.True(vm.HasErrors);
        }

        [Fact]
        public void TryValidateProperty_with_no_error_Test() {
            ViewModelBaseMock vm = new ViewModelBaseMock() {
                IntegerBetweenZeroAndFifty = 42
            };

            Assert.False(vm.HasErrors);
            Assert.Equal(42, vm.IntegerBetweenZeroAndFifty);
        }

        [Fact]
        public void SetPropertyTwice_with_same_error_Test() {
            ViewModelBaseMock vm = new ViewModelBaseMock() {
                IntegerBetweenZeroAndTen = 42
            };

            Assert.Single(vm.Errors);

            vm.IntegerBetweenZeroAndTen = 48;

            Assert.Single(vm.Errors);

            vm.IntegerBetweenZeroAndTen = 8;

            Assert.Empty(vm.Errors);
        }

        private const string ErrorMessage1 = "Error 1 !!!";
        private const string ErrorMessage2 = "Error 2 !!!";
        private const string PatternError = "Regular expression error";

        private class ViewModelBaseMock : ViewModelBase {
            private string testProperty1;

            [MinLength(2, ErrorMessage = ErrorMessage1)]
            [RegularExpression(@"^\$?\d+(\.(\d{2}))?$", ErrorMessage = PatternError)]
            public string TestProperty1 {
                get { return this.testProperty1; }
                set { this.testProperty1 = value; }
            }

            private string testProperty2;

            [MinLength(2, ErrorMessage = ErrorMessage2)]
            [RegularExpression(@"^\$?\d+(\.(\d{2}))?$", ErrorMessage = PatternError)]
            public string TestProperty2 {
                get { return this.testProperty2; }
                set { this.testProperty2 = value; }
            }

            private int integerBetweenZeroAndTen;

            [Range(0.0, 10.0)]
            public int IntegerBetweenZeroAndTen {
                get { return this.integerBetweenZeroAndTen; }
                set {
                    this.SetProperty(ref this.integerBetweenZeroAndTen, value);
                }
            }

            private int integerBetweenZeroAndFifty;

            [Range(0.0, 50.0)]
            public int IntegerBetweenZeroAndFifty {
                get { return this.integerBetweenZeroAndFifty; }
                set {
                    this.SetProperty(ref this.integerBetweenZeroAndFifty, value);
                }
            }
        }
    }
}