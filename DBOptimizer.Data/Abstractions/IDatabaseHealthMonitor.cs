using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DBOptimizer.Data.Abstractions;

/// <summary>
/// Universal database health monitoring interface for all platforms
/// </summary>
public interface IDatabaseHealthMonitor
{
    /// <summary>
    /// Gets the overall database health score
    /// </summary>
    Task<DatabaseHealth> GetHealthAsync();
    
    /// <summary>
    /// Gets database size and growth metrics
    /// </summary>
    Task<DatabaseSize> GetDatabaseSizeAsync();
    
    /// <summary>
    /// Gets connection statistics
    /// </summary>
    Task<ConnectionStatistics> GetConnectionStatsAsync();
    
    /// <summary>
    /// Gets resource utilization metrics
    /// </summary>
    Task<ResourceUtilization> GetResourceUtilizationAsync();
    
    /// <summary>
    /// Gets database-specific configuration settings
    /// </summary>
    Task<Dictionary<string, string>> GetConfigurationAsync();
}

/// <summary>
/// Overall database health status
/// </summary>
public class DatabaseHealth
{
    public string DatabaseName { get; set; }
    public string PlatformType { get; set; } // SQLServer, PostgreSQL, MySQL, Oracle
    public string PlatformVersion { get; set; }
    public DateTime CheckedAt { get; set; }
    
    // Health indicators
    public double UptimeHours { get; set; }
    public double CpuUsagePercent { get; set; }
    public double MemoryUsagePercent { get; set; }
    public double DiskUsagePercent { get; set; }
    
    // Connection health
    public int ActiveConnections { get; set; }
    public int MaxConnections { get; set; }
    public double ConnectionUsagePercent { get; set; }
    
    // Performance indicators
    public double AverageQueryTimeMs { get; set; }
    public long TotalQueries { get; set; }
    public long SlowQueries { get; set; }
    
    // Overall status
    public HealthStatus Status { get; set; }
    public List<HealthIssue> Issues { get; set; }
}

public enum HealthStatus
{
    Healthy,
    Warning,
    Critical,
    Unknown
}

public class HealthIssue
{
    public string Category { get; set; }
    public string Description { get; set; }
    public string Severity { get; set; } // Info, Warning, Error, Critical
    public string Recommendation { get; set; }
}

/// <summary>
/// Database size and growth information
/// </summary>
public class DatabaseSize
{
    public string DatabaseName { get; set; }
    public long TotalSizeBytes { get; set; }
    public long DataSizeBytes { get; set; }
    public long LogSizeBytes { get; set; }
    public long IndexSizeBytes { get; set; }
    public long FreeSpaceBytes { get; set; }
    
    // Growth metrics
    public double GrowthRateBytesPerDay { get; set; }
    public DateTime LastMeasured { get; set; }
    public DateTime? PreviousMeasurement { get; set; }
    
    // Projections
    public long ProjectedSizeIn30Days { get; set; }
    public long ProjectedSizeIn90Days { get; set; }
    
    // Human-readable sizes
    public string TotalSizeFormatted => FormatBytes(TotalSizeBytes);
    public string DataSizeFormatted => FormatBytes(DataSizeBytes);
    public string GrowthRateFormatted => $"{FormatBytes((long)GrowthRateBytesPerDay)}/day";
    
    private static string FormatBytes(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB", "TB" };
        double len = bytes;
        int order = 0;
        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len /= 1024;
        }
        return $"{len:0.##} {sizes[order]}";
    }
}

/// <summary>
/// Connection statistics
/// </summary>
public class ConnectionStatistics
{
    public DateTime MeasuredAt { get; set; }
    public int TotalConnections { get; set; }
    public int ActiveConnections { get; set; }
    public int IdleConnections { get; set; }
    public int SleepingConnections { get; set; }
    
    // Connection limits
    public int MaxConnections { get; set; }
    public double UsagePercent => MaxConnections > 0 ? (TotalConnections / (double)MaxConnections) * 100 : 0;
    
    // Connection details by database
    public Dictionary<string, int> ConnectionsByDatabase { get; set; }
    
    // Connection details by user
    public Dictionary<string, int> ConnectionsByUser { get; set; }
    
    // Historical peaks
    public int PeakConnections24h { get; set; }
    public DateTime PeakConnectionsTime { get; set; }
}

/// <summary>
/// Resource utilization metrics
/// </summary>
public class ResourceUtilization
{
    public DateTime MeasuredAt { get; set; }
    
    // CPU
    public double CpuUsagePercent { get; set; }
    public double CpuUsageDatabasePercent { get; set; } // Database-specific CPU
    public double CpuUsageSystemPercent { get; set; } // System/OS CPU
    
    // Memory
    public long TotalMemoryBytes { get; set; }
    public long UsedMemoryBytes { get; set; }
    public long FreeMemoryBytes { get; set; }
    public double MemoryUsagePercent => TotalMemoryBytes > 0 ? (UsedMemoryBytes / (double)TotalMemoryBytes) * 100 : 0;
    
    // Database-specific memory
    public long BufferCacheBytes { get; set; }
    public long ProcedureCacheBytes { get; set; }
    
    // Disk I/O
    public long DiskReadsPerSecond { get; set; }
    public long DiskWritesPerSecond { get; set; }
    public double DiskReadLatencyMs { get; set; }
    public double DiskWriteLatencyMs { get; set; }
    
    // Network
    public long NetworkBytesReceivedPerSecond { get; set; }
    public long NetworkBytesSentPerSecond { get; set; }
    
    // Wait statistics (platform-specific)
    public Dictionary<string, WaitStatistic> TopWaits { get; set; }
}

public class WaitStatistic
{
    public string WaitType { get; set; }
    public long WaitCount { get; set; }
    public double WaitTimeMs { get; set; }
    public double AverageWaitTimeMs { get; set; }
    public double PercentageOfTotal { get; set; }
}
