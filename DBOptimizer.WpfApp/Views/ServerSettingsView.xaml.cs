using System.Windows.Controls;
using DBOptimizer.WpfApp.ViewModels;

namespace DBOptimizer.WpfApp.Views;

public partial class ServerSettingsView : UserControl
{
    public ServerSettingsViewModel ViewModel { get; }

    public ServerSettingsView()
    {
        ViewModel = App.GetService<ServerSettingsViewModel>();
        DataContext = ViewModel;
        InitializeComponent();
    }
}

