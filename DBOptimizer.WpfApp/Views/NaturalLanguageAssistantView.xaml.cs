using System.Windows.Controls;
using DBOptimizer.WpfApp.ViewModels;

namespace DBOptimizer.WpfApp.Views;

public partial class NaturalLanguageAssistantView : UserControl
{
    public NaturalLanguageAssistantView()
    {
        InitializeComponent();
        DataContext = App.GetService<NaturalLanguageAssistantViewModel>();
    }
}

