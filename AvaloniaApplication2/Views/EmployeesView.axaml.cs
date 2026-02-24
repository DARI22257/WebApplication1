using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using AvaloniaApplication2.Services;
using AvaloniaApplication2.ViewModels;

namespace AvaloniaApplication2.Views;

public partial class EmployeesView : Window
{
    public EmployeesView(ApiService api)
    {
        InitializeComponent();
        DataContext = new EmployeesViewModel(api);
    }
    

    private void Delete(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void Create(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void Add(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }
}