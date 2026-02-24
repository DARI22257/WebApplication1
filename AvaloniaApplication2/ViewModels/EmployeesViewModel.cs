using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Controls.ApplicationLifetimes;
using AvaloniaApplication2.Models;
using AvaloniaApplication2.Services;
using AvaloniaApplication2.Tools;
using AvaloniaApplication2.Views;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaApplication2.ViewModels;

public class EmployeesViewModel : BaseVM
{
    private readonly ApiService _api;

    public ObservableCollection<EmployeeDto> Employees { get; } = new();

    private EmployeeDto? _selectedEmployee;
    public EmployeeDto? SelectedEmployee
    {
        get => _selectedEmployee;
        set
        {
            SetField(ref _selectedEmployee, value);
            EditEmployeeCommand.RaiseCanExecuteChanged();
            DeleteEmployeeCommand.RaiseCanExecuteChanged();
        }
    }

    public RelayCommand AddEmployeeCommand { get; }
    public RelayCommand EditEmployeeCommand { get; }
    public RelayCommand DeleteEmployeeCommand { get; }

    public EmployeesViewModel(ApiService api)
    {
        _api = api;

        AddEmployeeCommand = new RelayCommand(async () => await AddEmployee());
        EditEmployeeCommand = new RelayCommand(async () => await EditEmployee(), () => SelectedEmployee != null);
        DeleteEmployeeCommand = new RelayCommand(async () => await DeleteEmployee(), () => SelectedEmployee != null);

        _ = LoadEmployeesAsync();
    }

    public async Task LoadEmployeesAsync()
    {
        try
        {
            Employees.Clear();
            var list = await _api.GetEmployeesAsync();
            foreach (var emp in list)
                Employees.Add(emp);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка загрузки сотрудников: " + ex);
        }
    }

    private async Task AddEmployee()
    {
        var editor = new EmployeeEditorWindow(_api);
        var parent = (App.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
        if (parent != null)
        {
            var result = await editor.ShowDialog<bool>(parent);
            if (result) await LoadEmployeesAsync();
        }
    }

    private async Task EditEmployee()
    {
        if (SelectedEmployee == null) return;

        var editor = new EmployeeEditorWindow(_api, SelectedEmployee);
        var parent = (App.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
        if (parent != null)
        {
            var result = await editor.ShowDialog<bool>(parent);
            if (result) await LoadEmployeesAsync();
        }
    }

    private async Task DeleteEmployee()
    {
        if (SelectedEmployee == null) return;
        await _api.DeleteEmployeeAsync(SelectedEmployee.Id);
        await LoadEmployeesAsync();
    }
}