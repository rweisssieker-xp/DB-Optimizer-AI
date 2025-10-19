using System.Windows.Controls;
using DBOptimizer.WpfApp.ViewModels;

namespace DBOptimizer.WpfApp.Views;

public partial class DatabaseHealthView : UserControl
{
    public DatabaseHealthViewModel ViewModel { get; }

    public DatabaseHealthView()
    {
        ViewModel = App.GetService<DatabaseHealthViewModel>();
        DataContext = ViewModel;
        InitializeComponent();
    }
}

