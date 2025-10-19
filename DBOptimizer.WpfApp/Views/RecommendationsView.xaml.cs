using System.Windows.Controls;
using DBOptimizer.WpfApp.ViewModels;

namespace DBOptimizer.WpfApp.Views;

public partial class RecommendationsView : UserControl
{
    public RecommendationsViewModel ViewModel { get; }

    public RecommendationsView()
    {
        ViewModel = App.GetService<RecommendationsViewModel>();
        DataContext = ViewModel;
        InitializeComponent();
    }
}



