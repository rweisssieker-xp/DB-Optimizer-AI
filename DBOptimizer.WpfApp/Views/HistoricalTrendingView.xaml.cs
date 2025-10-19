using System.Windows.Controls;
using DBOptimizer.WpfApp.ViewModels;

namespace DBOptimizer.WpfApp.Views;

public partial class HistoricalTrendingView : UserControl
{
    public HistoricalTrendingViewModel ViewModel { get; }

    public HistoricalTrendingView()
    {
        ViewModel = App.GetService<HistoricalTrendingViewModel>();
        DataContext = ViewModel;
        InitializeComponent();

        Loaded += async (s, e) =>
        {
            try
            {
                await ViewModel.LoadDataCommand.ExecuteAsync(null);
            }
            catch
            {
                // Ignore errors on first load
            }
        };
    }
}

