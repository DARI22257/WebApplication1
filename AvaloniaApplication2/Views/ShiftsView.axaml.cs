using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaApplication2.Services;
using AvaloniaApplication2.ViewModels;


namespace AvaloniaApplication2.Views;

public partial class ShiftsView : Window
{
    public ShiftsView(ApiService api)
    {
        InitializeComponent();
        DataContext = new ShiftsViewModel(api);
    }
}