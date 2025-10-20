using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DBOptimizer.Core.Services;

/// <summary>
/// Service for generating executive-level performance reports and dashboards.
/// Provides C-level summaries, ROI tracking, and board-ready reports.
/// </summary>
public interface IExecutiveDashboardService
{
    /// <summary>
    /// Generates a comprehensive executive summary report
    /// </summary>
    Task<ExecutiveReport> GenerateExecutiveReportAsync(DateTime? from = null, DateTime? to = null);
    
    /// <summary>
    /// Calculates the overall performance health score (0-100)
    /// </summary>
    Task<HealthScoreReport> GetHealthScoreReportAsync();
    
    /// <summary>
    /// Gets ROI tracking information for implemented optimizations
    /// </summary>
    Task<RoiReport> GetRoiTrackingAsync(DateTime? from = null, DateTime? to = null);
    
    /// <summary>
    /// Generates budget justification report with cost savings
    /// </summary>
    Task<BudgetReport> GenerateBudgetJustificationAsync(DateTime? from = null, DateTime? to = null);
    
    /// <summary>
    /// Gets key performance indicators for executive dashboard
    /// </summary>
    Task<ExecutiveKpis> GetKeyPerformanceIndicatorsAsync();
    
    /// <summary>
    /// Exports executive report to various formats
    /// </summary>
    Task<byte[]> ExportReportAsync(ExecutiveReport report, ExportFormat format);
}

/// <summary>
/// Executive-level report with high-level summary and key metrics
/// </summary>
public class ExecutiveReport
{
    public DateTime GeneratedAt { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    
    // Overall Health
    public int HealthScore { get; set; } // 0-100
    public string HealthGrade { get; set; } // A, B, C, D, F
    public int HealthScoreTrend { get; set; } // Change from previous period
    
    // Key Metrics
    public PerformanceMetrics Performance { get; set; }
    public CostMetrics Costs { get; set; }
    public ReliabilityMetrics Reliability { get; set; }
    public OptimizationMetrics Optimizations { get; set; }
    
    // Top Achievements
    public List<Achievement> TopAchievements { get; set; }
    
    // Investment Summary
    public decimal ToolCostPerMonth { get; set; }
    public decimal SavingsGeneratedPerMonth { get; set; }
    public decimal NetRoi { get; set; }
    public decimal RoiPercentage { get; set; }
    
    // Board-Ready Summary
    public string BoardReadySummary { get; set; }
    
    // Trend Data (last 6 months)
    public List<MonthlyHealthScore> HistoricalTrend { get; set; }
}

public class PerformanceMetrics
{
    public string OverallGrade { get; set; } // A-, B+, etc.
    public double ImprovementPercentage { get; set; }
    public int SlowQueriesFixed { get; set; }
    public double AverageQueryTimeMs { get; set; }
    public double AverageQueryTimeReductionPercent { get; set; }
}

public class CostMetrics
{
    public string OverallGrade { get; set; }
    public decimal MonthlySavings { get; set; }
    public decimal YearToDateSavings { get; set; }
    public decimal ProjectedAnnualSavings { get; set; }
    public double CostEfficiencyImprovement { get; set; }
}

public class ReliabilityMetrics
{
    public string OverallGrade { get; set; }
    public double UptimePercentage { get; set; }
    public double ErrorRate { get; set; }
    public int IncidentsPrevented { get; set; }
    public double MeanTimeBetweenFailures { get; set; }
}

public class OptimizationMetrics
{
    public int TotalOptimizations { get; set; }
    public int AutoApplied { get; set; }
    public int ManualReview { get; set; }
    public int Rollbacks { get; set; }
    public double SuccessRate { get; set; }
    public double UserSatisfactionImprovement { get; set; }
}

public class Achievement
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Icon { get; set; }
    public double ImpactPercentage { get; set; }
}

public class MonthlyHealthScore
{
    public DateTime Month { get; set; }
    public int Score { get; set; }
    public string Grade { get; set; }
}

/// <summary>
/// Health score report with detailed breakdown
/// </summary>
public class HealthScoreReport
{
    public int CurrentScore { get; set; } // 0-100
    public string Grade { get; set; } // A, B+, etc.
    public string Status { get; set; } // Excellent, Good, Fair, etc.
    public int TrendDirection { get; set; } // +5, -3, etc.
    public string IndustryRank { get; set; } // Top 25%, etc.
    
    // Score Breakdown (each out of 100)
    public ComponentScore QueryPerformance { get; set; }
    public ComponentScore SystemReliability { get; set; }
    public ComponentScore ResourceEfficiency { get; set; }
    public ComponentScore OptimizationQuality { get; set; }
    public ComponentScore CostEfficiency { get; set; }
    
    // Strengths and Weaknesses
    public List<HealthFactor> Strengths { get; set; }
    public List<HealthFactor> AreasForImprovement { get; set; }
    
    // Historical Trend
    public List<MonthlyHealthScore> HistoricalTrend { get; set; }
    
    // Goal Progress
    public int TargetScore { get; set; }
    public int GapToTarget { get; set; }
    public int EstimatedMonthsToTarget { get; set; }
}

public class HealthFactor
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Impact { get; set; } // High, Medium, Low
    public string Icon { get; set; }
}

/// <summary>
/// ROI tracking report
/// </summary>
public class RoiReport
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    
    // Investment
    public decimal ToolCost { get; set; }
    public decimal ImplementationCost { get; set; }
    public decimal TrainingCost { get; set; }
    public decimal TotalInvestment { get; set; }
    
    // Returns
    public decimal CostSavings { get; set; }
    public decimal ProductivityGains { get; set; }
    public decimal DowntimeReduction { get; set; }
    public decimal TotalReturns { get; set; }
    
    // ROI Metrics
    public decimal NetReturn { get; set; }
    public decimal RoiPercentage { get; set; }
    public int PaybackPeriodMonths { get; set; }
    
    // Breakdown by Category
    public List<RoiCategory> RoiByCategory { get; set; }
    
    // Trend
    public List<MonthlyRoi> MonthlyRoiTrend { get; set; }
}

public class RoiCategory
{
    public string Category { get; set; }
    public decimal Savings { get; set; }
    public double Percentage { get; set; }
}

public class MonthlyRoi
{
    public DateTime Month { get; set; }
    public decimal Savings { get; set; }
    public decimal RoiPercentage { get; set; }
}

/// <summary>
/// Budget justification report
/// </summary>
public class BudgetReport
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    
    // Current Period
    public decimal CurrentToolCost { get; set; }
    public decimal CurrentSavings { get; set; }
    public decimal CurrentNetBenefit { get; set; }
    
    // Projections
    public decimal ProjectedNextQuarterSavings { get; set; }
    public decimal ProjectedAnnualSavings { get; set; }
    
    // Justification Points
    public List<BudgetJustificationPoint> KeyPoints { get; set; }
    
    // Comparison
    public ComparisonData WithoutToolCost { get; set; }
    public ComparisonData WithToolCost { get; set; }
    public ComparisonData NetDifference { get; set; }
}

public class BudgetJustificationPoint
{
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal FinancialImpact { get; set; }
}

public class ComparisonData
{
    public decimal MonthlyCost { get; set; }
    public decimal AnnualCost { get; set; }
    public string Description { get; set; }
}

/// <summary>
/// Executive KPIs for dashboard
/// </summary>
public class ExecutiveKpis
{
    // Real-time Metrics
    public int CurrentHealthScore { get; set; }
    public double CurrentQueryPerformance { get; set; }
    public double CurrentUptime { get; set; }
    public decimal MonthlySavings { get; set; }
    
    // Trends (vs. previous period)
    public int HealthScoreTrend { get; set; }
    public double PerformanceTrend { get; set; }
    public double UptimeTrend { get; set; }
    public decimal SavingsTrend { get; set; }
    
    // Quick Stats
    public int OptimizationsThisMonth { get; set; }
    public int IncidentsPreventedThisMonth { get; set; }
    public double UserSatisfactionScore { get; set; }
    public int ActiveIssues { get; set; }
}

/// <summary>
/// Export format for reports
/// </summary>
public enum ExportFormat
{
    PDF,
    Excel,
    PowerPoint,
    Word,
    JSON,
    CSV
}
