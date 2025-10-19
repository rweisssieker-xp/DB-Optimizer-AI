using System.Windows.Controls;
using DBOptimizer.WpfApp.ViewModels;

namespace DBOptimizer.WpfApp.Views;

public partial class AiHealthDashboardView : UserControl
{
    public AiHealthDashboardView()
    {
        InitializeComponent();
        DataContext = App.GetService<AiHealthDashboardViewModel>();
    }
}

