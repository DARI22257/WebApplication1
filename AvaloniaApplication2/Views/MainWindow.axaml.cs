using Avalonia.Controls;
using AvaloniaApplication2.Services;
using AvaloniaApplication2.ViewModels;

namespace AvaloniaApplication2.Views;

    public partial class MainWindow : Window
    {
        public MainWindow(ApiService api, AuthService auth)
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(api, auth);
        }
    }
