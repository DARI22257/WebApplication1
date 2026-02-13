using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Controls.ApplicationLifetimes;
using AvaloniaApplication2.Models;
using AvaloniaApplication2.Services;
using AvaloniaApplication2.Tools;
using AvaloniaApplication2.Views;

namespace AvaloniaApplication2.ViewModels;
public class ShiftsViewModel : BaseVM
{
    private readonly ApiService api;

    public ObservableCollection<ShiftDto> Shifts { get; } = new();
    public ObservableCollection<EmployeeDto> Employees { get; } = new();

    private ShiftDto? _selectedShift;
    public ShiftDto? SelectedShift
    {
        get => _selectedShift;
        set
        {
            SetField(ref _selectedShift, value);
            EditShiftCommand.RaiseCanExecuteChanged();
            DeleteShiftCommand.RaiseCanExecuteChanged();
        }
    }

    private EmployeeDto? _filterEmployee;
    public EmployeeDto? FilterEmployee
    {
        get => _filterEmployee;
        set
        {
            SetField(ref _filterEmployee, value);
            _ = LoadShiftsAsync();
        }
    }

    public RelayCommand AddShiftCommand { get; }
    public RelayCommand EditShiftCommand { get; }
    public RelayCommand DeleteShiftCommand { get; }

    public ShiftsViewModel(ApiService api)
    {
        this.api = api;

        AddShiftCommand = new RelayCommand(async () => await AddShift());
        EditShiftCommand = new RelayCommand(async () => await EditShift(), () => SelectedShift != null);
        DeleteShiftCommand = new RelayCommand(async () => await DeleteShift(), () => SelectedShift != null);

        _ = LoadEmployeesAsync();
        _ = LoadShiftsAsync();
    }

    public async Task LoadEmployeesAsync()
    {
        Employees.Clear();
        var list = await api.GetEmployeesAsync();
        foreach (var e in list)
            Employees.Add(e.Employee);
    }

    public async Task LoadShiftsAsync()
    {
        try
        {
            Shifts.Clear();
            var list = await api.GetShiftsAsync();

            Console.WriteLine("Shifts count: " + list?.Count);
            


        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка загрузки смен: " + ex);
        }
    }

    private async Task AddShift()
    {
        var editor = new ShiftEditorWindow(api);
        var parent = (App.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
        if (parent != null)
        {
            var result = await editor.ShowDialog<bool>(parent);
            if (result) await LoadShiftsAsync();
        }
    }

    private async Task EditShift()
    {
        if (SelectedShift == null) return;

        var editor = new ShiftEditorWindow(api, SelectedShift);
        var parent = (App.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
        if (parent != null)
        {
            var result = await editor.ShowDialog<bool>(parent);
            if (result) await LoadShiftsAsync();
        }
    }

    private async Task DeleteShift()
    {
        if (SelectedShift == null) return;

        await api.DeleteShiftAsync(SelectedShift.Id);
        await LoadShiftsAsync();
    }
}