using System;
using System.Threading.Tasks;
using AvaloniaApplication2.Models;
using AvaloniaApplication2.Services;
using AvaloniaApplication2.Tools;

namespace AvaloniaApplication2.ViewModels;

public class LoginViewModel : BaseVM
{
    private readonly ApiService _api;
    private readonly AuthService _auth;

    private string _username = "";
    public string Username { get => _username; set => SetField(ref _username, value); }

    private string _password = "";
    public string Password { get => _password; set => SetField(ref _password, value); }

    private bool _rememberMe;
    public bool RememberMe { get => _rememberMe; set => SetField(ref _rememberMe, value); }

    private string _errorMessage = "";
    public string ErrorMessage { get => _errorMessage; set => SetField(ref _errorMessage, value); }

    public RelayCommand LoginCommand { get; }

    public LoginViewModel(ApiService api, AuthService auth)
    {
        _api = api;
        _auth = auth;

        LoginCommand = new RelayCommand(async () => await LoginAsync());
        _api.OnUnauthorized += () =>
        {
            _ = _auth.ClearTokenAsync();
            NavigationService.OpenLogin(_api, _auth);
        };
    }

    private async Task LoginAsync()
    {
        try
        {
            ErrorMessage = "";

            var resp = await _api.LoginAsync(new LoginRequest
            {
                Username = Username,
                Password = Password
            });

            if (resp == null || string.IsNullOrWhiteSpace(resp.Token))
            {
                ErrorMessage = "Неверный логин или пароль";
                return;
            }

            await _auth.SaveTokenAsync(resp.Token, RememberMe);

            // проверим профиль (если токен битый — вернётся null)
            var profile = await _api.GetProfileAsync();
            if (profile == null)
            {
                ErrorMessage = "Не удалось получить профиль (проверь сервер/токен)";
                await _auth.ClearTokenAsync();
                return;
            }

            NavigationService.OpenMain(_api, _auth);
        }
        catch (Exception ex)
        {
            ErrorMessage = "Ошибка входа: " + ex.Message;
        }
    }
}