using System;
using System.Threading.Tasks;
using AvaloniaApplication2.Models;
using AvaloniaApplication2.Services;
using AvaloniaApplication2.Tools;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RelayCommand = CommunityToolkit.Mvvm.Input.RelayCommand;


namespace AvaloniaApplication2.ViewModels;

public class LoginViewModel : BaseVM
{
    private readonly ApiService api;
    private readonly AuthService auth;

    private string _username = "";
    public string Username
    {
        get => _username;
        set => SetField(ref _username, value);
    }

    private string _password = "";
    public string Password
    {
        get => _password;
        set => SetField(ref _password, value);
    }

    private bool _rememberMe = false;
    public bool RememberMe
    {
        get => _rememberMe;
        set => SetField(ref _rememberMe, value);
    }

    private string _errorMessage = "";
    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetField(ref _errorMessage, value);
    }

    public RelayCommand LoginCommand { get; }

    public LoginViewModel(ApiService api, AuthService auth)
    {
        this.api = api;
        this.auth = auth;

        LoginCommand = new RelayCommand(async () => await LoginAsync());
    }

    private async Task LoginAsync()
    {
        ErrorMessage = "";
        try
        {
            var response = await api.LoginAsync(new LoginRequest
            {
                Username = this.Username,
                Password = this.Password
            });

            await auth.SaveTokenAsync(response.Token, RememberMe);

            NavigationService.OpenMain(api, auth);
        }
        catch (ApiException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            ErrorMessage = "Неверный логин или пароль";
        }
        catch (Exception e)
        {
            ErrorMessage = e.Message;
        }
    }

}