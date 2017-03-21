using System;
using Xunit;
using Exia.Mvvm;

namespace Exia.Mvvm.Test {
    public class ObservableObjectBaseTest {
        [Fact]
        public void RaisePropertyChanged_Test() {
            ObservableObjectBaseMock oob = new ObservableObjectBaseMock();

            Assert.PropertyChanged(oob, "Changed", () => {
                oob.Changed = "new value";
            });
        }

        [Fact]
        public void RaisePropertyChanging_Test() {
            ObservableObjectBaseMock oob = new ObservableObjectBaseMock();
            bool isChanging = false;

            oob.PropertyChanging += (o, e) => {
                isChanging = e.PropertyName == "Changing";
            };

            oob.Changing = "new value";
            Assert.True(isChanging);
        }

        [Fact]
        public void SetProperty_Test() {
            ObservableObjectBaseMock oob = new ObservableObjectBaseMock();

            bool hasChanged = false;
            oob.PropertyChanged += (o, e) => {
                hasChanged = e.PropertyName == "SetProperty";
            };

            bool isChanging = false;
            oob.PropertyChanging += (o, e) => {
                isChanging = e.PropertyName == "SetProperty";
            };

            oob.SetProperty = "new value";

            Assert.True(isChanging);
            Assert.True(hasChanged);
            Assert.True(oob.HasChanged);
            Assert.True(oob.SetProperty == oob.setProperty);
        }

        private class ObservableObjectBaseMock : ObservableObjectBase {
            private string changed;
            public string Changed {
                get { return this.changed; }
                set {
                    this.changed = value;
                    this.RaisePropertyChanged();
                }
            }

            private string changing;
            public string Changing {
                get { return this.changing; }
                set {
                    this.RaisePropertyChanging();
                    this.changing = value;
                    this.RaisePropertyChanged();
                }
            }

            public bool HasChanged { get; set; }

            public string setProperty;
            public string SetProperty {
                get { return this.setProperty; }
                set {
                    this.HasChanged = this.SetProperty(ref this.setProperty, value);
                }
            }
        }
    }
}
