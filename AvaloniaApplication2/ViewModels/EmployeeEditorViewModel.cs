using System;
using System.Threading.Tasks;
using AvaloniaApplication2.Models;
using AvaloniaApplication2.Services;
using AvaloniaApplication2.Tools;

namespace AvaloniaApplication2.ViewModels;

public class EmployeeEditorViewModel : BaseVM
{
    private readonly ApiService _api;
    private readonly bool _isEdit;

    public EmployeeDto Employee { get; }

    public RelayCommand SaveCommand { get; }
    public RelayCommand CancelCommand { get; }

    public event Action<bool>? CloseRequested;

    public EmployeeEditorViewModel(ApiService api, EmployeeDto? employee)
    {
        _api = api;
        _isEdit = employee != null;

        Employee = employee != null
            ? new EmployeeDto
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Position = employee.Position,
                HireDate = employee.HireDate,
                IsActive = employee.IsActive
            }
            : new EmployeeDto
            {
                HireDate = DateTimeOffset.Now,
                IsActive = true
            };

        SaveCommand = new RelayCommand(() => _ = SaveAsync());
        CancelCommand = new RelayCommand(() => CloseRequested?.Invoke(false));
    }

    private async Task SaveAsync()
    {
        try
        {
            if (_isEdit)
            {
                var ok = await _api.UpdateEmployeeAsync(Employee.Id, Employee);
                CloseRequested?.Invoke(ok);
            }
            else
            {
                var created = await _api.CreateEmployeeAsync(Employee);
                CloseRequested?.Invoke(created != null);
            }
        }
        catch
        {
            CloseRequested?.Invoke(false);
        }
    }
}