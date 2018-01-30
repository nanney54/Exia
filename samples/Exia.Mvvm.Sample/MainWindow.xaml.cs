using System.Windows;

namespace Exia.Mvvm.Sample {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e) {
            var vm = this.DataContext as ViewModelBase;

            this.cbValid.IsChecked = await vm.IsModelValidAsync;
        }
    }
}
