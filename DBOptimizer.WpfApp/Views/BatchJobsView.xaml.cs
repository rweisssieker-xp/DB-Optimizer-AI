using System.Windows.Controls;
using DBOptimizer.WpfApp.ViewModels;

namespace DBOptimizer.WpfApp.Views;

public partial class BatchJobsView : UserControl
{
    public BatchJobsViewModel ViewModel { get; }

    public BatchJobsView()
    {
        ViewModel = App.GetService<BatchJobsViewModel>();
        DataContext = ViewModel;
        InitializeComponent();
    }
}



