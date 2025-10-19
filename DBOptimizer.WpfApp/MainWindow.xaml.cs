using System.Windows;
using DBOptimizer.WpfApp.ViewModels;

namespace DBOptimizer.WpfApp;

public partial class MainWindow : Window
{
    public MainViewModel ViewModel { get; }

    public MainWindow()
    {
        ViewModel = App.GetService<MainViewModel>();
        DataContext = ViewModel;
        InitializeComponent();
    }
}

