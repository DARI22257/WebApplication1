using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AvaloniaApplication2.Models;
using AvaloniaApplication2.Services;
using AvaloniaApplication2.Tools;

namespace AvaloniaApplication2.ViewModels;

public class ShiftEditorViewModel : BaseVM
{
    private readonly ApiService _api;
    private readonly bool _isEdit;

    public ShiftDto Shift { get; }
    public ObservableCollection<EmployeeDto> Employees { get; } = new();

    public RelayCommand SaveCommand { get; }
    public RelayCommand CancelCommand { get; }

    public event Action<bool>? CloseRequested;

    private EmployeeDto? _selectedEmployee;
    public EmployeeDto? SelectedEmployee
    {
        get => _selectedEmployee;
        set
        {
            if (!SetField(ref _selectedEmployee, value)) return;
            if (value != null)
                Shift.EmployeeId = value.Id;
        }
    }
    public ShiftEditorViewModel(ApiService api, ShiftDto? shift = null)
    {


        _api = api;
        _isEdit = shift != null;

        Shift = shift != null
            ? new ShiftDto
            {
                Id = shift.Id,
                EmployeeId = shift.EmployeeId,
                StartDateTime = shift.StartDateTime,
                EndDateTime = shift.EndDateTime,
                Description = shift.Description
            }
            : new ShiftDto
            {
                StartDateTime = DateTimeOffset.Now,
                EndDateTime = DateTimeOffset.Now.AddHours(1),
                Description = ""
            };

        SaveCommand = new RelayCommand(async () => await SaveAsync());
        CancelCommand = new RelayCommand(() => CloseRequested?.Invoke(false));

        _ = LoadEmployeesAsync();
    }

    private async Task LoadEmployeesAsync()
    {
        Employees.Clear();
        var list = await _api.GetEmployeesAsync();
        foreach (var e in list)
            Employees.Add(e);

        if (!_isEdit && Employees.Count > 0 && Shift.EmployeeId == 0)
            Shift.EmployeeId = Employees[0].Id;
    }

    private async Task SaveAsync()
    {
        try
        {
            if (_isEdit)
            {
                var ok = await _api.UpdateShiftAsync(Shift.Id, Shift);
                CloseRequested?.Invoke(ok);
            }
            else
            {
                var created = await _api.CreateShiftAsync(Shift);
                CloseRequested?.Invoke(created != null);
            }
        }
        catch
        {
            CloseRequested?.Invoke(false);
        }
    }
}