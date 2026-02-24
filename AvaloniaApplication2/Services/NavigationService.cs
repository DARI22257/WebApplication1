using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using AvaloniaApplication2.Views;

namespace AvaloniaApplication2.Services;

public static class NavigationService
{
    public static void OpenLogin(ApiService api, AuthService auth)
    {
        if (App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var login = new LoginWindow(api, auth);
            desktop.MainWindow = login;
            login.Show();
        }
    }

    public static void OpenMain(ApiService api, AuthService auth)
    {
        if (App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var main = new MainWindow(api, auth);
            desktop.MainWindow = main;
            main.Show();
        }
    }

    public static void OpenWindow(Window window)
    {
        window.Show();
    }

    public static async Task<bool?> OpenDialog(Window dialog)
    {
        if (App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
            && desktop.MainWindow != null)
        {
            return await dialog.ShowDialog<bool>(desktop.MainWindow);
        }

        return null;
    }
}