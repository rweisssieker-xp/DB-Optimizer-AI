using System.Windows.Controls;
using DBOptimizer.WpfApp.ViewModels;

namespace DBOptimizer.WpfApp.Views;

public partial class AiInsightsDashboardView : UserControl
{
    public AiInsightsDashboardView()
    {
        InitializeComponent();
        DataContext = App.GetService<AiInsightsDashboardViewModel>();
    }
}

