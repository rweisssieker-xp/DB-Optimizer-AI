# 📊 Chart Integration Examples - LiveCharts2

**Schritt-für-Schritt Guide für Chart Integration**

---

## 🚀 Quick Start

### **1. Installation**
```powershell
dotnet add DBOptimizer.WpfApp package LiveChartsCore.SkiaSharpView.WPF
```

### **2. Add Namespace in XAML**
```xml
xmlns:lvc="clr-namespace:LiveChartsCore.SkiaSharpView.WPF;assembly=LiveChartsCore.SkiaSharpView.WPF"
```

### **3. Add Using Statements in ViewModel**
```csharp
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
```

---

## 📈 Example 1: Health Score Trend Line Chart

### **ViewModel Code**:
```csharp
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

public partial class ExecutiveDashboardViewModel : ObservableObject
{
    [ObservableProperty]
    private ISeries[] _healthScoreSeries;

    [ObservableProperty]
    private Axis[] _xAxes;

    [ObservableProperty]
    private Axis[] _yAxes;

    private void InitializeHealthScoreChart()
    {
        // Data from HealthScoreTrend collection
        var scores = HealthScoreTrend.Select(t => (double)t.Score).ToArray();
        var labels = HealthScoreTrend.Select(t => t.Period).ToArray();

        HealthScoreSeries = new ISeries[]
        {
            new LineSeries<double>
            {
                Values = scores,
                Name = "Health Score",
                Fill = null,
                GeometrySize = 10,
                Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 3 },
                GeometryStroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 2 },
                GeometryFill = new SolidColorPaint(SKColors.White)
            }
        };

        XAxes = new Axis[]
        {
            new Axis
            {
                Labels = labels,
                LabelsRotation = 0,
                TextSize = 12,
                SeparatorsPaint = new SolidColorPaint(new SKColor(200, 200, 200))
            }
        };

        YAxes = new Axis[]
        {
            new Axis
            {
                MinLimit = 0,
                MaxLimit = 100,
                TextSize = 12,
                Name = "Health Score",
                NameTextSize = 14,
                SeparatorsPaint = new SolidColorPaint(new SKColor(200, 200, 200))
            }
        };
    }
}
```

### **XAML Code**:
```xml
<Border Style="{StaticResource CardStyle}">
    <StackPanel>
        <TextBlock Text="📈 Health Score Trend (6 Months)" 
                  FontSize="20" 
                  FontWeight="Bold" 
                  Margin="0,0,0,15"/>
        
        <lvc:CartesianChart Series="{Binding HealthScoreSeries}"
                           XAxes="{Binding XAxes}"
                           YAxes="{Binding YAxes}"
                           Height="300"
                           LegendPosition="Top"/>
    </StackPanel>
</Border>
```

---

## 🍩 Example 2: ROI Breakdown Pie Chart

### **ViewModel Code**:
```csharp
[ObservableProperty]
private ISeries[] _roiBreakdownSeries;

private void InitializeRoiChart()
{
    RoiBreakdownSeries = new ISeries[]
    {
        new PieSeries<double>
        {
            Values = new double[] { 40 },
            Name = "Query Optimization",
            DataLabelsPaint = new SolidColorPaint(SKColors.White),
            DataLabelsSize = 14,
            DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle
        },
        new PieSeries<double>
        {
            Values = new double[] { 25 },
            Name = "Resource Efficiency"
        },
        new PieSeries<double>
        {
            Values = new double[] { 20 },
            Name = "Downtime Prevention"
        },
        new PieSeries<double>
        {
            Values = new double[] { 15 },
            Name = "Automation"
        }
    };
}
```

### **XAML Code**:
```xml
<Border Style="{StaticResource CardStyle}">
    <StackPanel>
        <TextBlock Text="💰 Savings Breakdown" 
                  FontSize="20" 
                  FontWeight="Bold" 
                  Margin="0,0,0,15"/>
        
        <lvc:PieChart Series="{Binding RoiBreakdownSeries}"
                     Height="300"
                     LegendPosition="Right"/>
    </StackPanel>
</Border>
```

---

## 📊 Example 3: Monthly Changes Bar Chart

### **ViewModel Code**:
```csharp
[ObservableProperty]
private ISeries[] _monthlyChangesSeries;

private void InitializeMonthlyChangesChart()
{
    // Sample data
    var months = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun" };
    var successful = new double[] { 18, 22, 25, 20, 24, 28 };
    var failed = new double[] { 2, 1, 3, 2, 1, 2 };

    MonthlyChangesSeries = new ISeries[]
    {
        new ColumnSeries<double>
        {
            Values = successful,
            Name = "Successful",
            Fill = new SolidColorPaint(SKColor.Parse("#4CAF50")),
            MaxBarWidth = 40
        },
        new ColumnSeries<double>
        {
            Values = failed,
            Name = "Failed",
            Fill = new SolidColorPaint(SKColor.Parse("#F44336")),
            MaxBarWidth = 40
        }
    };

    XAxes = new Axis[]
    {
        new Axis
        {
            Labels = months,
            LabelsRotation = 0,
            TextSize = 12
        }
    };

    YAxes = new Axis[]
    {
        new Axis
        {
            MinLimit = 0,
            TextSize = 12,
            Name = "Changes"
        }
    };
}
```

### **XAML Code**:
```xml
<Border Style="{StaticResource CardStyle}">
    <StackPanel>
        <TextBlock Text="📊 Monthly Changes" 
                  FontSize="20" 
                  FontWeight="Bold" 
                  Margin="0,0,0,15"/>
        
        <lvc:CartesianChart Series="{Binding MonthlyChangesSeries}"
                           XAxes="{Binding XAxes}"
                           YAxes="{Binding YAxes}"
                           Height="300"
                           LegendPosition="Top"/>
    </StackPanel>
</Border>
```

---

## 📈 Example 4: Real-Time Performance Gauge

### **ViewModel Code**:
```csharp
[ObservableProperty]
private ISeries[] _performanceGaugeSeries;

private void InitializePerformanceGauge()
{
    PerformanceGaugeSeries = new ISeries[]
    {
        new PieSeries<double>
        {
            Values = new double[] { HealthScore },
            Fill = new SolidColorPaint(GetColorForScore(HealthScore)),
            MaxOuterRadius = 0.85,
            MaxRadialColumnWidth = 60
        }
    };
}

private SKColor GetColorForScore(int score)
{
    if (score >= 90) return SKColor.Parse("#4CAF50"); // Green
    if (score >= 80) return SKColor.Parse("#2196F3"); // Blue
    if (score >= 70) return SKColor.Parse("#FF9800"); // Orange
    return SKColor.Parse("#F44336"); // Red
}
```

### **XAML Code**:
```xml
<Border Style="{StaticResource CardStyle}">
    <StackPanel>
        <TextBlock Text="⚡ Performance Gauge" 
                  FontSize="20" 
                  FontWeight="Bold" 
                  Margin="0,0,0,15"/>
        
        <lvc:PieChart Series="{Binding PerformanceGaugeSeries}"
                     Height="250"
                     InitialRotation="-90"
                     MaxAngle=180"/>
        
        <TextBlock Text="{Binding HealthScore, StringFormat='{}{0}/100'}"
                  FontSize="32"
                  FontWeight="Bold"
                  HorizontalAlignment="Center"
                  Margin="0,-100,0,0"/>
    </StackPanel>
</Border>
```

---

## 🎯 Complete Integration Example

### **Full ViewModel with Charts**:
```csharp
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.Linq;

namespace DBOptimizer.WpfApp.ViewModels;

public partial class ExecutiveDashboardViewModel : ObservableObject
{
    // Existing properties...
    [ObservableProperty]
    private int _healthScore = 87;

    [ObservableProperty]
    private ObservableCollection<HealthScoreTrendItem> _healthScoreTrend;

    // Chart properties
    [ObservableProperty]
    private ISeries[] _trendChartSeries;

    [ObservableProperty]
    private Axis[] _trendXAxes;

    [ObservableProperty]
    private Axis[] _trendYAxes;

    [ObservableProperty]
    private ISeries[] _savingsBreakdownSeries;

    public ExecutiveDashboardViewModel(/* dependencies */)
    {
        // Initialize collections
        HealthScoreTrend = new ObservableCollection<HealthScoreTrendItem>();
        
        // Initialize after data loads
    }

    [RelayCommand]
    private async Task LoadDashboardAsync()
    {
        // Load data...
        
        // Initialize charts after data is loaded
        InitializeTrendChart();
        InitializeSavingsChart();
    }

    private void InitializeTrendChart()
    {
        if (HealthScoreTrend == null || !HealthScoreTrend.Any())
            return;

        var scores = HealthScoreTrend.Select(t => (double)t.Score).ToArray();
        var labels = HealthScoreTrend.Select(t => t.Period).ToArray();

        TrendChartSeries = new ISeries[]
        {
            new LineSeries<double>
            {
                Values = scores,
                Name = "Health Score",
                Fill = null,
                GeometrySize = 10,
                Stroke = new SolidColorPaint(SKColor.Parse("#2196F3")) { StrokeThickness = 3 },
                GeometryStroke = new SolidColorPaint(SKColor.Parse("#2196F3")) { StrokeThickness = 2 },
                GeometryFill = new SolidColorPaint(SKColors.White)
            }
        };

        TrendXAxes = new Axis[]
        {
            new Axis
            {
                Labels = labels,
                LabelsRotation = 0,
                TextSize = 12,
                SeparatorsPaint = new SolidColorPaint(new SKColor(220, 220, 220))
            }
        };

        TrendYAxes = new Axis[]
        {
            new Axis
            {
                MinLimit = 70,
                MaxLimit = 100,
                TextSize = 12,
                Name = "Score",
                NameTextSize = 14,
                SeparatorsPaint = new SolidColorPaint(new SKColor(220, 220, 220))
            }
        };
    }

    private void InitializeSavingsChart()
    {
        SavingsBreakdownSeries = new ISeries[]
        {
            new PieSeries<double>
            {
                Values = new double[] { 40 },
                Name = "Query Optimization (€4,980)",
                Fill = new SolidColorPaint(SKColor.Parse("#2196F3")),
                DataLabelsPaint = new SolidColorPaint(SKColors.White),
                DataLabelsSize = 12
            },
            new PieSeries<double>
            {
                Values = new double[] { 25 },
                Name = "Resource Efficiency (€3,113)",
                Fill = new SolidColorPaint(SKColor.Parse("#4CAF50"))
            },
            new PieSeries<double>
            {
                Values = new double[] { 20 },
                Name = "Downtime Prevention (€2,490)",
                Fill = new SolidColorPaint(SKColor.Parse("#FF9800"))
            },
            new PieSeries<double>
            {
                Values = new double[] { 15 },
                Name = "Automation (€1,868)",
                Fill = new SolidColorPaint(SKColor.Parse("#9C27B0"))
            }
        };
    }
}
```

---

## 📋 Complete XAML with Charts

```xml
<UserControl x:Class="DBOptimizer.WpfApp.Views.ExecutiveDashboardView"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:lvc="clr-namespace:LiveChartsCore.SkiaSharpView.WPF;assembly=LiveChartsCore.SkiaSharpView.WPF">
    
    <ScrollViewer VerticalScrollBarVisibility="Auto">
        <StackPanel Margin="30">
            
            <!-- Existing content... -->
            
            <!-- NEW: Trend Chart Section -->
            <Border Style="{StaticResource CardStyle}">
                <StackPanel>
                    <TextBlock Text="📈 6-Month Health Score Trend" 
                              FontSize="20" 
                              FontWeight="Bold" 
                              Margin="0,0,0,15"/>
                    
                    <lvc:CartesianChart Series="{Binding TrendChartSeries}"
                                       XAxes="{Binding TrendXAxes}"
                                       YAxes="{Binding TrendYAxes}"
                                       Height="300"
                                       LegendPosition="Hidden"
                                       TooltipPosition="Top"/>
                </StackPanel>
            </Border>
            
            <!-- NEW: Savings Breakdown Chart -->
            <Grid Margin="0,0,0,20">
                <Grid.ColumnDefinitions>
                    <ColumnDefinition Width="*"/>
                    <ColumnDefinition Width="*"/>
                </Grid.ColumnDefinitions>
                
                <Border Grid.Column="0" Style="{StaticResource CardStyle}" Margin="0,0,10,0">
                    <StackPanel>
                        <TextBlock Text="💰 Savings Breakdown" 
                                  FontSize="20" 
                                  FontWeight="Bold" 
                                  Margin="0,0,0,15"/>
                        
                        <lvc:PieChart Series="{Binding SavingsBreakdownSeries}"
                                     Height="300"
                                     LegendPosition="Bottom"
                                     InitialRotation="-90"/>
                    </StackPanel>
                </Border>
                
                <!-- Other chart or content -->
            </Grid>
            
        </StackPanel>
    </ScrollViewer>
</UserControl>
```

---

## 🎨 Styling Tips

### **Custom Colors**:
```csharp
// Company brand colors
var primaryColor = SKColor.Parse("#667EEA");
var secondaryColor = SKColor.Parse("#764BA2");
var successColor = SKColor.Parse("#4CAF50");
var warningColor = SKColor.Parse("#FF9800");

new LineSeries<double>
{
    Values = data,
    Stroke = new SolidColorPaint(primaryColor) { StrokeThickness = 3 }
}
```

### **Gradients**:
```csharp
new LineSeries<double>
{
    Values = data,
    Fill = new LinearGradientPaint(
        new SKColor(102, 126, 234, 100), // Start color with alpha
        new SKColor(118, 75, 162, 50)    // End color with alpha
    )
}
```

---

## 🔄 Real-Time Updates

```csharp
// Update chart data dynamically
private void UpdateChartData()
{
    var newScores = HealthScoreTrend.Select(t => (double)t.Score).ToArray();
    
    // Update existing series
    if (TrendChartSeries != null && TrendChartSeries.Length > 0)
    {
        ((LineSeries<double>)TrendChartSeries[0]).Values = newScores;
    }
}
```

---

## 🚀 Quick Installation & Test

```powershell
# 1. Install package
dotnet add DBOptimizer.WpfApp package LiveChartsCore.SkiaSharpView.WPF

# 2. Add to ExecutiveDashboardViewModel.cs
# - Add using statements
# - Add chart properties
# - Call InitializeTrendChart() in LoadDashboardAsync()

# 3. Add to ExecutiveDashboardView.xaml
# - Add xmlns:lvc namespace
# - Add <lvc:CartesianChart> control

# 4. Build & Run
dotnet build
dotnet run --project DBOptimizer.WpfApp

# 5. Navigate to Executive Dashboard tab
# - Charts should render automatically!
```

---

## 📚 Resources

- **LiveCharts Docs**: https://lvcharts.com/
- **Samples**: https://github.com/beto-rodriguez/LiveCharts2/tree/master/samples
- **API Reference**: https://lvcharts.com/api/

---

**Status**: ✅ Chart Integration Guide Complete  
**Difficulty**: ⭐⭐ (Easy with examples)  
**Time**: ~30 minutes to integrate  
**Version**: 2.0  
