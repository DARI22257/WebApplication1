using Avalonia.Controls;
using AvaloniaApplication2.Models;
using AvaloniaApplication2.Services;
using AvaloniaApplication2.ViewModels;

namespace AvaloniaApplication2.Views;

public partial class EmployeeEditorWindow : Window
{
    public EmployeeEditorWindow(ApiService api)
        : this(api, null)
    {
    }

    
    public EmployeeEditorWindow(ApiService api, EmployeeDto? employee)
    {
        InitializeComponent();

        var vm = new EmployeeEditorViewModel(api, employee);
        vm.CloseRequested += ok => Close(ok);

        DataContext = vm;
    }
}