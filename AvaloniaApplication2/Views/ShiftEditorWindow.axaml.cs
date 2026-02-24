using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaApplication2.Models;
using AvaloniaApplication2.Services;
using AvaloniaApplication2.ViewModels;

namespace AvaloniaApplication2.Views;

public partial class ShiftEditorWindow : Window
{
    public ShiftEditorWindow(ApiService api, ShiftDto? shift = null)
    {
        InitializeComponent();

        var vm = new ShiftEditorViewModel(api, shift);
        vm.CloseRequested += ok => Close(ok);
        DataContext = vm;
    }
}