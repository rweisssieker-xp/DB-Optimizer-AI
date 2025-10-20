using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DBOptimizer.WpfApp.ViewModels;

namespace DBOptimizer.WpfApp.Views;

/// <summary>
/// Interaction logic for ExecutiveDashboardView.xaml
/// </summary>
public partial class ExecutiveDashboardView : UserControl
{
    public ExecutiveDashboardView()
    {
        InitializeComponent();
    }

    private async void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is ExecutiveDashboardViewModel viewModel)
        {
            await viewModel.InitializeAsync();
        }
    }

    private async void HealthScore_Click(object sender, MouseButtonEventArgs e)
    {
        if (DataContext is ExecutiveDashboardViewModel viewModel)
        {
            await viewModel.ViewHealthScoreDetailsCommand.ExecuteAsync(null);
        }
    }
}
