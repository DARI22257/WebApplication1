using System.Threading.Tasks;
using AvaloniaApplication2.Services;
using AvaloniaApplication2.Tools;
using AvaloniaApplication2.Views;

namespace AvaloniaApplication2.ViewModels;

public class MainWindowViewModel : BaseVM
{
    private readonly ApiService _api;
    private readonly AuthService _auth;

    public RelayCommand OpenEmployeesCommand { get; }
    public RelayCommand OpenShiftsCommand { get; }
    public RelayCommand LogoutCommand { get; }

    public MainWindowViewModel(ApiService api, AuthService auth)
    {
        _api = api;
        _auth = auth;

        OpenEmployeesCommand = new RelayCommand(() =>
            new EmployeesView(_api).Show());

        OpenShiftsCommand = new RelayCommand(() =>
            new ShiftsView(_api).Show());

        LogoutCommand = new RelayCommand(() =>
            _ = LogoutAsync());
    }

    private async Task LogoutAsync()
    {
        await _auth.ClearTokenAsync();
        NavigationService.OpenLogin(_api, _auth);
    }
}