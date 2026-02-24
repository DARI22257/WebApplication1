using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaloniaApplication2.Services;

namespace AvaloniaApplication2;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // сервисы приложения
            var auth = new AuthService();
            var api = new ApiService(auth);

            // стартуем с окна логина
            NavigationService.OpenLogin(api, auth);
        }

        base.OnFrameworkInitializationCompleted();
    }
}