using System.Windows;
using System.Windows.Controls;
using DBOptimizer.WpfApp.ViewModels;

namespace DBOptimizer.WpfApp.Views;

/// <summary>
/// Interaction logic for ComplianceAuditView.xaml
/// </summary>
public partial class ComplianceAuditView : UserControl
{
    public ComplianceAuditView()
    {
        InitializeComponent();
    }

    private async void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is ComplianceAuditViewModel viewModel)
        {
            await viewModel.InitializeAsync();
        }
    }
}
