using System.Windows.Controls;
using DBOptimizer.WpfApp.ViewModels;

namespace DBOptimizer.WpfApp.Views;

public partial class SettingsView : UserControl
{
    public SettingsViewModel ViewModel { get; }

    public SettingsView()
    {
        ViewModel = App.GetService<SettingsViewModel>();
        DataContext = ViewModel;
        InitializeComponent();
        Loaded += async (s, e) =>
        {
            try
            {
                await ViewModel.LoadProfilesCommand.ExecuteAsync(null);
            }
            catch
            {
                // Ignore errors on first load
            }
        };

        // Handle password box changes
        PasswordBox.PasswordChanged += (s, e) =>
        {
            ViewModel.PlainPassword = PasswordBox.Password;
        };
    }
}


