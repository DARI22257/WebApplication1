using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaApplication2.Services;
using AvaloniaApplication2.ViewModels;

namespace AvaloniaApplication2.Views;


    public partial class LoginWindow : Window
    {
        public LoginWindow(ApiService api, AuthService auth)
        {
            InitializeComponent();
            DataContext = new LoginViewModel(api, auth);
        }
    }
