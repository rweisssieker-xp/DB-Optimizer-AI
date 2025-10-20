using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DBOptimizer.Core.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace DBOptimizer.WpfApp.ViewModels;

/// <summary>
/// ViewModel for Executive Dashboard
/// Provides C-level performance reports and KPIs
/// </summary>
public partial class ExecutiveDashboardViewModel : ObservableObject
{
    private readonly ILogger<ExecutiveDashboardViewModel> _logger;
    private readonly IExecutiveDashboardService _executiveDashboardService;
    private readonly IPerformanceHealthScoreService _healthScoreService;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private bool _hasData;

    [ObservableProperty]
    private string _errorMessage;

    // Health Score
    [ObservableProperty]
    private int _healthScore;

    [ObservableProperty]
    private string _healthGrade;

    [ObservableProperty]
    private string _healthStatus;

    [ObservableProperty]
    private int _healthScoreTrend;

    [ObservableProperty]
    private string _trendIcon;

    // Key Metrics
    [ObservableProperty]
    private string _queryPerformanceGrade;

    [ObservableProperty]
    private double _queryPerformanceImprovement;

    [ObservableProperty]
    private string _costEfficiencyGrade;

    [ObservableProperty]
    private decimal _monthlySavings;

    [ObservableProperty]
    private string _reliabilityGrade;

    [ObservableProperty]
    private double _uptimePercentage;

    [ObservableProperty]
    private decimal _roiPercentage;

    // Top Achievements
    [ObservableProperty]
    private ObservableCollection<AchievementItem> _achievements;

    // Investment Summary
    [ObservableProperty]
    private decimal _toolCost;

    [ObservableProperty]
    private decimal _savingsGenerated;

    [ObservableProperty]
    private decimal _netRoi;

    // Board Summary
    [ObservableProperty]
    private string _boardReadySummary;

    // Historical Trend
    [ObservableProperty]
    private ObservableCollection<HealthScoreTrendItem> _healthScoreTrend;

    // Quick Stats
    [ObservableProperty]
    private int _optimizationsThisMonth;

    [ObservableProperty]
    private int _incidentsPrevented;

    [ObservableProperty]
    private double _userSatisfaction;

    [ObservableProperty]
    private int _activeIssues;

    public ExecutiveDashboardViewModel(
        ILogger<ExecutiveDashboardViewModel> logger,
        IExecutiveDashboardService executiveDashboardService,
        IPerformanceHealthScoreService healthScoreService)
    {
        _logger = logger;
        _executiveDashboardService = executiveDashboardService;
        _healthScoreService = healthScoreService;

        Achievements = new ObservableCollection<AchievementItem>();
        HealthScoreTrend = new ObservableCollection<HealthScoreTrendItem>();
    }

    [RelayCommand]
    private async Task LoadDashboardAsync()
    {
        if (IsLoading) return;

        try
        {
            IsLoading = true;
            HasData = false;
            ErrorMessage = string.Empty;

            _logger.LogInformation("Loading executive dashboard...");

            // Load executive report
            var report = await _executiveDashboardService.GenerateExecutiveReportAsync();

            // Update Health Score
            HealthScore = report.HealthScore;
            HealthGrade = report.HealthGrade;
            HealthStatus = GetHealthStatus(report.HealthScore);
            HealthScoreTrend = report.HealthScoreTrend;
            TrendIcon = report.HealthScoreTrend > 0 ? "⬆️" : report.HealthScoreTrend < 0 ? "⬇️" : "➡️";

            // Update Key Metrics
            QueryPerformanceGrade = report.Performance.OverallGrade;
            QueryPerformanceImprovement = report.Performance.ImprovementPercentage;
            CostEfficiencyGrade = report.Costs.OverallGrade;
            MonthlySavings = report.Costs.MonthlySavings;
            ReliabilityGrade = report.Reliability.OverallGrade;
            UptimePercentage = report.Reliability.UptimePercentage;
            RoiPercentage = report.RoiPercentage;

            // Update Achievements
            Achievements.Clear();
            foreach (var achievement in report.TopAchievements)
            {
                Achievements.Add(new AchievementItem
                {
                    Title = achievement.Title,
                    Description = achievement.Description,
                    Icon = achievement.Icon,
                    ImpactPercentage = achievement.ImpactPercentage
                });
            }

            // Update Investment Summary
            ToolCost = report.ToolCostPerMonth;
            SavingsGenerated = report.SavingsGeneratedPerMonth;
            NetRoi = report.NetRoi;

            // Update Board Summary
            BoardReadySummary = report.BoardReadySummary;

            // Update Historical Trend
            HealthScoreTrend.Clear();
            foreach (var trend in report.HistoricalTrend)
            {
                HealthScoreTrend.Add(new HealthScoreTrendItem
                {
                    Period = trend.Period,
                    Score = trend.Score,
                    Grade = trend.Grade,
                    VisualBar = GenerateVisualBar(trend.Score)
                });
            }

            // Load KPIs
            var kpis = await _executiveDashboardService.GetKeyPerformanceIndicatorsAsync();
            OptimizationsThisMonth = kpis.OptimizationsThisMonth;
            IncidentsPrevented = kpis.IncidentsPreventedThisMonth;
            UserSatisfaction = kpis.UserSatisfactionScore;
            ActiveIssues = kpis.ActiveIssues;

            HasData = true;
            _logger.LogInformation("Executive dashboard loaded successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading executive dashboard");
            ErrorMessage = $"Failed to load dashboard: {ex.Message}";
            HasData = false;
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadDashboardAsync();
    }

    [RelayCommand]
    private async Task ExportReportAsync(string format)
    {
        try
        {
            _logger.LogInformation($"Exporting executive report to {format}...");

            var report = await _executiveDashboardService.GenerateExecutiveReportAsync();
            var exportFormat = Enum.Parse<ExportFormat>(format, true);
            var data = await _executiveDashboardService.ExportReportAsync(report, exportFormat);

            // Save file
            var fileName = $"ExecutiveReport_{DateTime.Now:yyyyMMdd}.{format.ToLower()}";
            var filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), fileName);
            await System.IO.File.WriteAllBytesAsync(filePath, data);

            _logger.LogInformation($"Report exported to {filePath}");

            // Show success message
            System.Windows.MessageBox.Show(
                $"Report exported successfully to:\n{filePath}",
                "Export Successful",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error exporting report to {format}");
            System.Windows.MessageBox.Show(
                $"Failed to export report: {ex.Message}",
                "Export Error",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private async Task ViewHealthScoreDetailsAsync()
    {
        try
        {
            var healthReport = await _healthScoreService.GetHealthScoreReportAsync();

            // TODO: Navigate to health score detail view or show dialog
            _logger.LogInformation("Health score details requested");

            System.Windows.MessageBox.Show(
                $"Health Score Details:\n\n" +
                $"Current Score: {healthReport.CurrentScore}/100 ({healthReport.Grade})\n" +
                $"Status: {healthReport.Status}\n" +
                $"Trend: {(healthReport.TrendDirection > 0 ? "+" : "")}{healthReport.TrendDirection} points\n" +
                $"Industry Rank: {healthReport.IndustryRank}\n\n" +
                $"Component Breakdown:\n" +
                $"• Query Performance: {healthReport.QueryPerformance.Score}/100 ({healthReport.QueryPerformance.Grade})\n" +
                $"• System Reliability: {healthReport.SystemReliability.Score}/100 ({healthReport.SystemReliability.Grade})\n" +
                $"• Resource Efficiency: {healthReport.ResourceEfficiency.Score}/100 ({healthReport.ResourceEfficiency.Grade})\n" +
                $"• Optimization Quality: {healthReport.OptimizationQuality.Score}/100 ({healthReport.OptimizationQuality.Grade})\n" +
                $"• Cost Efficiency: {healthReport.CostEfficiency.Score}/100 ({healthReport.CostEfficiency.Grade})",
                "Health Score Details",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error viewing health score details");
        }
    }

    private string GetHealthStatus(int score)
    {
        return score switch
        {
            >= 90 => "Excellent",
            >= 80 => "Good",
            >= 70 => "Fair",
            >= 60 => "Needs Improvement",
            _ => "Critical Issues"
        };
    }

    private string GenerateVisualBar(int score)
    {
        int filled = score / 10;
        return new string('●', filled) + new string('○', 10 - filled);
    }

    public async Task InitializeAsync()
    {
        await LoadDashboardAsync();
    }
}

/// <summary>
/// Achievement item for display
/// </summary>
public class AchievementItem
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Icon { get; set; }
    public double ImpactPercentage { get; set; }
}

/// <summary>
/// Health score trend item for chart display
/// </summary>
public class HealthScoreTrendItem
{
    public string Period { get; set; }
    public int Score { get; set; }
    public string Grade { get; set; }
    public string VisualBar { get; set; }
}
