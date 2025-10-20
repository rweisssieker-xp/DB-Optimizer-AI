using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DBOptimizer.Data.Abstractions;

/// <summary>
/// Universal query monitoring interface for all database platforms
/// Abstracts platform-specific query monitoring implementations
/// </summary>
public interface IQueryMonitor
{
    /// <summary>
    /// Gets the top N queries by execution time
    /// </summary>
    Task<List<QueryMetric>> GetTopQueriesAsync(int top = 50);
    
    /// <summary>
    /// Gets detailed information about a specific query
    /// </summary>
    Task<QueryDetails> GetQueryDetailsAsync(string queryId);
    
    /// <summary>
    /// Gets the execution plan for a query
    /// </summary>
    Task<ExecutionPlan> GetExecutionPlanAsync(string queryId);
    
    /// <summary>
    /// Gets query statistics for a time range
    /// </summary>
    Task<List<QueryStatistic>> GetQueryStatisticsAsync(DateTime from, DateTime to);
    
    /// <summary>
    /// Gets currently running queries
    /// </summary>
    Task<List<RunningQuery>> GetRunningQueriesAsync();
}

/// <summary>
/// Query metric with performance data
/// </summary>
public class QueryMetric
{
    public string QueryId { get; set; }
    public string QueryText { get; set; }
    public int ExecutionCount { get; set; }
    public double TotalTimeMs { get; set; }
    public double AverageTimeMs { get; set; }
    public double MinTimeMs { get; set; }
    public double MaxTimeMs { get; set; }
    public long RowsReturned { get; set; }
    public DateTime LastExecutedAt { get; set; }
    public string DatabaseName { get; set; }
}

/// <summary>
/// Detailed query information
/// </summary>
public class QueryDetails
{
    public string QueryId { get; set; }
    public string QueryText { get; set; }
    public string NormalizedQuery { get; set; }
    public Dictionary<string, object> Statistics { get; set; }
    public List<string> TablesAccessed { get; set; }
    public List<string> IndexesUsed { get; set; }
    public double EstimatedCost { get; set; }
    public ExecutionPlan ExecutionPlan { get; set; }
}

/// <summary>
/// Query execution plan
/// </summary>
public class ExecutionPlan
{
    public string PlatformType { get; set; } // SQLServer, PostgreSQL, MySQL, Oracle
    public string PlanText { get; set; }
    public string PlanXml { get; set; }
    public string PlanJson { get; set; }
    public List<ExecutionPlanNode> Nodes { get; set; }
    public double EstimatedCost { get; set; }
    public double ActualCost { get; set; }
}

/// <summary>
/// Execution plan node
/// </summary>
public class ExecutionPlanNode
{
    public string OperationType { get; set; }
    public string Description { get; set; }
    public double Cost { get; set; }
    public double CostPercentage { get; set; }
    public long RowsEstimated { get; set; }
    public long RowsActual { get; set; }
    public List<ExecutionPlanNode> Children { get; set; }
}

/// <summary>
/// Query statistic data point
/// </summary>
public class QueryStatistic
{
    public DateTime Timestamp { get; set; }
    public string QueryId { get; set; }
    public double ExecutionTimeMs { get; set; }
    public long RowsReturned { get; set; }
    public double CpuTimeMs { get; set; }
    public long LogicalReads { get; set; }
    public long PhysicalReads { get; set; }
}

/// <summary>
/// Currently running query
/// </summary>
public class RunningQuery
{
    public int SessionId { get; set; }
    public string QueryText { get; set; }
    public DateTime StartTime { get; set; }
    public TimeSpan Duration { get; set; }
    public string Status { get; set; }
    public string UserName { get; set; }
    public string DatabaseName { get; set; }
    public double CpuTimeMs { get; set; }
    public long MemoryUsageKB { get; set; }
}
