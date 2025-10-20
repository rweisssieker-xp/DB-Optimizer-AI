using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DBOptimizer.Core.Services;

/// <summary>
/// Service for calculating and tracking overall performance health score (0-100)
/// Provides holistic system health assessment with detailed breakdowns
/// </summary>
public interface IPerformanceHealthScoreService
{
    /// <summary>
    /// Calculates the current performance health score (0-100)
    /// </summary>
    Task<HealthScore> CalculateHealthScoreAsync();
    
    /// <summary>
    /// Gets detailed health score breakdown by component
    /// </summary>
    Task<HealthScoreBreakdown> GetHealthScoreBreakdownAsync();
    
    /// <summary>
    /// Get historical health score trends
    /// </summary>
    Task<List<PerformanceHealthScoreHistory>> GetHealthScoreHistoryAsync(int days = 30);
    
    /// <summary>
    /// Gets health score compared to industry benchmarks
    /// </summary>
    Task<HealthScoreBenchmark> GetIndustryBenchmarkAsync(string industry, string companySize);
    
    /// <summary>
    /// Predicts future health score based on current trends
    /// </summary>
    Task<HealthScoreForecast> PredictHealthScoreAsync(int daysAhead = 30);
}

/// <summary>
/// Overall health score with grade and status
/// </summary>
public class HealthScore
{
    public DateTime CalculatedAt { get; set; }
    public int Score { get; set; } // 0-100
    public string Grade { get; set; } // A (90-100), B (80-89), C (70-79), D (60-69), F (<60)
    public string Status { get; set; } // Excellent, Good, Fair, Needs Improvement, Critical
    public HealthTrend Trend { get; set; }
    
    // Quick Summary
    public string Summary { get; set; }
    public List<string> TopStrengths { get; set; }
    public List<string> TopWeaknesses { get; set; }
}

public class HealthTrend
{
    public int ChangeFromLastWeek { get; set; }
    public int ChangeFromLastMonth { get; set; }
    public string Direction { get; set; } // Improving, Stable, Declining
    public string TrendIcon { get; set; } // ⬆️, ➡️, ⬇️
}

/// <summary>
/// Detailed breakdown of health score by component
/// </summary>
public class HealthScoreBreakdown
{
    public int OverallScore { get; set; }
    public string OverallGrade { get; set; }
    
    // Component Scores (weighted)
    public ComponentScore QueryPerformance { get; set; } // 30% weight
    public ComponentScore SystemReliability { get; set; } // 25% weight
    public ComponentScore ResourceEfficiency { get; set; } // 20% weight
    public ComponentScore OptimizationQuality { get; set; } // 15% weight
    public ComponentScore CostEfficiency { get; set; } // 10% weight
    
    // Detailed Metrics
    public List<HealthMetric> DetailedMetrics { get; set; }
    
    // Recommendations
    public List<HealthRecommendation> ImprovementRecommendations { get; set; }
}

public class ComponentScore
{
    public string ComponentName { get; set; }
    public int Score { get; set; } // 0-100
    public string Grade { get; set; } // A-, B+, etc.
    public double Weight { get; set; } // Percentage contribution to overall score
    public int ContributionPoints { get; set; } // Actual points contributed
    
    // Visual representation
    public string VisualBar { get; set; } // ●●●●●●●●●○ (10 dots)
    public string Color { get; set; } // Green, Yellow, Red
    
    // Sub-metrics
    public List<SubMetric> SubMetrics { get; set; }
}

public class SubMetric
{
    public string Name { get; set; }
    public double Value { get; set; }
    public string Unit { get; set; }
    public string Status { get; set; } // Good, Warning, Critical
    public double ImpactOnComponent { get; set; } // Percentage impact on parent component
}

public class HealthMetric
{
    public string Category { get; set; }
    public string Name { get; set; }
    public string CurrentValue { get; set; }
    public string Baseline { get; set; }
    public string Target { get; set; }
    public string Status { get; set; } // Good, Warning, Critical
    public int ScoreImpact { get; set; } // Impact on overall score
}

public class HealthRecommendation
{
    public int Priority { get; set; } // 1-5 (1 = highest)
    public string Title { get; set; }
    public string Description { get; set; }
    public int PotentialScoreImprovement { get; set; }
    public string Category { get; set; }
    public TimeSpan EstimatedTimeToImplement { get; set; }
    public string Difficulty { get; set; } // Easy, Medium, Hard
}

/// <summary>
/// Historical health score data point
/// </summary>
public class PerformanceHealthScoreHistory
{
    public DateTime Date { get; set; }
    public int Score { get; set; }
    public string Grade { get; set; } = string.Empty;
    public string Period { get; set; } = string.Empty; // "Jan 2025", "Week 42", etc.
    
    // Component scores at that time
    public int QueryPerformanceScore { get; set; }
    public int ReliabilityScore { get; set; }
    public int ResourceEfficiencyScore { get; set; }
    public int OptimizationQualityScore { get; set; }
    public int CostEfficiencyScore { get; set; }
}

/// <summary>
/// Benchmark comparison with industry peers
/// </summary>
public class HealthScoreBenchmark
{
    public int YourScore { get; set; }
    public int IndustryAverage { get; set; }
    public int IndustryMedian { get; set; }
    public int TopPerformerScore { get; set; } // 90th percentile
    
    public string YourPercentile { get; set; } // "Top 25%"
    public string Ranking { get; set; } // "Above Average", "Average", "Below Average"
    
    // Comparison by component
    public Dictionary<string, BenchmarkComponent> ComponentComparison { get; set; }
    
    // Industry metadata
    public string Industry { get; set; }
    public string CompanySize { get; set; }
    public int PeerCount { get; set; }
}

public class BenchmarkComponent
{
    public int YourScore { get; set; }
    public int IndustryAverage { get; set; }
    public int Difference { get; set; }
    public string Status { get; set; } // "Leading", "On Par", "Lagging"
}

/// <summary>
/// Future health score prediction
/// </summary>
public class HealthScoreForecast
{
    public DateTime ForecastDate { get; set; }
    public int CurrentScore { get; set; }
    public int PredictedScore { get; set; }
    public int PredictedChange { get; set; }
    public double ConfidenceLevel { get; set; } // 0-100%
    
    public string Trajectory { get; set; } // "Improving", "Stable", "Declining"
    public List<HealthScoreForecastDataPoint> ForecastCurve { get; set; } = new();
    
    // Risk factors
    public List<RiskFactor> RiskFactors { get; set; } = new();
    
    // Recommendations to improve trajectory
    public List<string> ProactiveActions { get; set; } = new();
}

public class HealthScoreForecastDataPoint
{
    public DateTime Date { get; set; }
    public int PredictedScore { get; set; }
    public int LowerBound { get; set; }
    public int UpperBound { get; set; }
}

public class RiskFactor
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int PotentialImpact { get; set; } // Score points
    public double Probability { get; set; } // 0-100%
    public string Severity { get; set; } // Low, Medium, High, Critical
}

/// <summary>
/// Health score calculation algorithm settings
/// </summary>
public class HealthScoreAlgorithm
{
    // Component weights (must sum to 100%)
    public double QueryPerformanceWeight { get; set; } = 0.30; // 30%
    public double SystemReliabilityWeight { get; set; } = 0.25; // 25%
    public double ResourceEfficiencyWeight { get; set; } = 0.20; // 20%
    public double OptimizationQualityWeight { get; set; } = 0.15; // 15%
    public double CostEfficiencyWeight { get; set; } = 0.10; // 10%
    
    // Thresholds for grades
    public Dictionary<string, (int Min, int Max)> GradeThresholds { get; set; } = new()
    {
        { "A", (90, 100) },
        { "B", (80, 89) },
        { "C", (70, 79) },
        { "D", (60, 69) },
        { "F", (0, 59) }
    };
    
    // Status mappings
    public Dictionary<string, string> StatusMappings { get; set; } = new()
    {
        { "A", "Excellent" },
        { "B", "Good" },
        { "C", "Fair" },
        { "D", "Needs Improvement" },
        { "F", "Critical Issues" }
    };
}
