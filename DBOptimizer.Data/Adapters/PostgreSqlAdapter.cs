using DBOptimizer.Data.Abstractions;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBOptimizer.Data.Adapters;

/// <summary>
/// PostgreSQL database adapter implementing universal monitoring interfaces
/// Uses pg_stat_statements and pg_stat_* views for monitoring
/// </summary>
public class PostgreSqlQueryMonitor : IQueryMonitor
{
    private readonly ILogger<PostgreSqlQueryMonitor> _logger;
    private readonly string _connectionString;

    public PostgreSqlQueryMonitor(ILogger<PostgreSqlQueryMonitor> logger, string connectionString)
    {
        _logger = logger;
        _connectionString = connectionString;
    }

    public async Task<List<QueryMetric>> GetTopQueriesAsync(int top = 50)
    {
        try
        {
            _logger.LogInformation($"Getting top {top} queries from PostgreSQL...");

            var queries = new List<QueryMetric>();

            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var sql = @"
                SELECT 
                    queryid::text,
                    query,
                    calls as execution_count,
                    total_exec_time / 1000 as total_time_ms,
                    mean_exec_time / 1000 as avg_time_ms,
                    min_exec_time / 1000 as min_time_ms,
                    max_exec_time / 1000 as max_time_ms,
                    rows as rows_returned,
                    CURRENT_TIMESTAMP as last_executed
                FROM pg_stat_statements
                WHERE query NOT LIKE '%pg_stat%'
                ORDER BY total_exec_time DESC
                LIMIT @top";

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@top", top);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                queries.Add(new QueryMetric
                {
                    QueryId = reader.GetString(0),
                    QueryText = reader.GetString(1),
                    ExecutionCount = reader.GetInt32(2),
                    TotalTimeMs = reader.GetDouble(3),
                    AverageTimeMs = reader.GetDouble(4),
                    MinTimeMs = reader.GetDouble(5),
                    MaxTimeMs = reader.GetDouble(6),
                    RowsReturned = reader.GetInt64(7),
                    LastExecutedAt = reader.GetDateTime(8),
                    DatabaseName = connection.Database
                });
            }

            _logger.LogInformation($"Retrieved {queries.Count} queries from PostgreSQL");
            return queries;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting top queries from PostgreSQL");
            throw;
        }
    }

    public async Task<QueryDetails> GetQueryDetailsAsync(string queryId)
    {
        try
        {
            _logger.LogInformation($"Getting query details for {queryId}...");

            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var sql = @"
                SELECT 
                    queryid::text,
                    query,
                    calls,
                    total_exec_time / 1000 as total_time_ms,
                    mean_exec_time / 1000 as avg_time_ms,
                    stddev_exec_time / 1000 as stddev_time_ms,
                    rows,
                    shared_blks_hit,
                    shared_blks_read,
                    shared_blks_written
                FROM pg_stat_statements
                WHERE queryid::text = @queryId
                LIMIT 1";

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@queryId", queryId);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var details = new QueryDetails
                {
                    QueryId = reader.GetString(0),
                    QueryText = reader.GetString(1),
                    NormalizedQuery = NormalizeQuery(reader.GetString(1)),
                    Statistics = new Dictionary<string, object>
                    {
                        { "Calls", reader.GetInt64(2) },
                        { "TotalTimeMs", reader.GetDouble(3) },
                        { "AvgTimeMs", reader.GetDouble(4) },
                        { "StdDevTimeMs", reader.GetDouble(5) },
                        { "Rows", reader.GetInt64(6) },
                        { "SharedBlksHit", reader.GetInt64(7) },
                        { "SharedBlksRead", reader.GetInt64(8) },
                        { "SharedBlksWritten", reader.GetInt64(9) }
                    },
                    TablesAccessed = ExtractTablesFromQuery(reader.GetString(1)),
                    IndexesUsed = new List<string>(),
                    EstimatedCost = reader.GetDouble(4) * reader.GetInt64(2)
                };

                return details;
            }

            throw new Exception($"Query not found: {queryId}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting query details for {queryId}");
            throw;
        }
    }

    public async Task<ExecutionPlan> GetExecutionPlanAsync(string queryId)
    {
        try
        {
            _logger.LogInformation($"Getting execution plan for {queryId}...");

            // Get the query text first
            var details = await GetQueryDetailsAsync(queryId);

            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            // Get EXPLAIN ANALYZE output
            var explainSql = $"EXPLAIN (FORMAT JSON, ANALYZE, BUFFERS) {details.QueryText}";

            using var command = new NpgsqlCommand(explainSql, connection);
            using var reader = await command.ExecuteReaderAsync();

            string planJson = "";
            if (await reader.ReadAsync())
            {
                planJson = reader.GetString(0);
            }

            return new ExecutionPlan
            {
                PlatformType = "PostgreSQL",
                PlanText = details.QueryText,
                PlanXml = null,
                PlanJson = planJson,
                Nodes = new List<ExecutionPlanNode>(),
                EstimatedCost = details.EstimatedCost,
                ActualCost = details.EstimatedCost
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting execution plan for {queryId}");
            throw;
        }
    }

    public async Task<List<QueryStatistic>> GetQueryStatisticsAsync(DateTime from, DateTime to)
    {
        try
        {
            _logger.LogInformation($"Getting query statistics from {from:yyyy-MM-dd} to {to:yyyy-MM-dd}...");

            // PostgreSQL pg_stat_statements doesn't have timestamp info
            // This would require custom logging or use pg_stat_activity for current queries
            var statistics = new List<QueryStatistic>();

            // Return empty for now - would need custom implementation
            _logger.LogWarning("Query statistics by date range not available in pg_stat_statements. Consider implementing custom logging.");

            return statistics;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting query statistics");
            throw;
        }
    }

    public async Task<List<RunningQuery>> GetRunningQueriesAsync()
    {
        try
        {
            _logger.LogInformation("Getting running queries from PostgreSQL...");

            var queries = new List<RunningQuery>();

            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var sql = @"
                SELECT 
                    pid,
                    query,
                    query_start,
                    EXTRACT(EPOCH FROM (NOW() - query_start)) as duration_seconds,
                    state,
                    usename,
                    datname
                FROM pg_stat_activity
                WHERE state = 'active'
                AND pid <> pg_backend_pid()
                ORDER BY query_start";

            using var command = new NpgsqlCommand(sql, connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                queries.Add(new RunningQuery
                {
                    SessionId = reader.GetInt32(0),
                    QueryText = reader.GetString(1),
                    StartTime = reader.GetDateTime(2),
                    Duration = TimeSpan.FromSeconds(reader.GetDouble(3)),
                    Status = reader.GetString(4),
                    UserName = reader.GetString(5),
                    DatabaseName = reader.GetString(6),
                    CpuTimeMs = 0, // Not available in pg_stat_activity
                    MemoryUsageKB = 0 // Not available in pg_stat_activity
                });
            }

            _logger.LogInformation($"Retrieved {queries.Count} running queries");
            return queries;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting running queries");
            throw;
        }
    }

    #region Private Helper Methods

    private string NormalizeQuery(string query)
    {
        // Simple normalization - replace literals with placeholders
        return System.Text.RegularExpressions.Regex.Replace(query, @"'[^']*'", "'?'");
    }

    private List<string> ExtractTablesFromQuery(string query)
    {
        var tables = new List<string>();
        var words = query.Split(new[] { ' ', '\n', '\t', ',', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);

        bool nextIsTable = false;
        foreach (var word in words)
        {
            if (word.Equals("FROM", StringComparison.OrdinalIgnoreCase) ||
                word.Equals("JOIN", StringComparison.OrdinalIgnoreCase))
            {
                nextIsTable = true;
                continue;
            }

            if (nextIsTable && !word.Equals("WHERE", StringComparison.OrdinalIgnoreCase))
            {
                tables.Add(word.Trim());
                nextIsTable = false;
            }
        }

        return tables.Distinct().ToList();
    }

    #endregion
}

/// <summary>
/// PostgreSQL database health monitor
/// </summary>
public class PostgreSqlHealthMonitor : IDatabaseHealthMonitor
{
    private readonly ILogger<PostgreSqlHealthMonitor> _logger;
    private readonly string _connectionString;

    public PostgreSqlHealthMonitor(ILogger<PostgreSqlHealthMonitor> logger, string connectionString)
    {
        _logger = logger;
        _connectionString = connectionString;
    }

    public async Task<DatabaseHealth> GetHealthAsync()
    {
        try
        {
            _logger.LogInformation("Getting PostgreSQL health status...");

            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var health = new DatabaseHealth
            {
                DatabaseName = connection.Database,
                PlatformType = "PostgreSQL",
                PlatformVersion = connection.ServerVersion,
                CheckedAt = DateTime.UtcNow
            };

            // Get uptime
            var uptimeSql = "SELECT EXTRACT(EPOCH FROM (NOW() - pg_postmaster_start_time())) / 3600";
            using (var cmd = new NpgsqlCommand(uptimeSql, connection))
            {
                var result = await cmd.ExecuteScalarAsync();
                health.UptimeHours = result != null ? Convert.ToDouble(result) : 0;
            }

            // Get connection stats
            var connSql = @"
                SELECT 
                    COUNT(*) as total_connections,
                    (SELECT setting::int FROM pg_settings WHERE name = 'max_connections') as max_connections
                FROM pg_stat_activity";

            using (var cmd = new NpgsqlCommand(connSql, connection))
            {
                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    health.ActiveConnections = reader.GetInt32(0);
                    health.MaxConnections = reader.GetInt32(1);
                    health.ConnectionUsagePercent = (health.ActiveConnections / (double)health.MaxConnections) * 100;
                }
            }

            // Get database size
            var sizeSql = "SELECT pg_database_size(current_database())";
            using (var cmd = new NpgsqlCommand(sizeSql, connection))
            {
                var result = await cmd.ExecuteScalarAsync();
                var sizeBytes = result != null ? Convert.ToInt64(result) : 0;
                health.DiskUsagePercent = 0; // Would need disk info from system
            }

            // Get query stats
            var querySql = @"
                SELECT 
                    COUNT(*) as total_queries,
                    AVG(mean_exec_time / 1000) as avg_time_ms,
                    COUNT(*) FILTER (WHERE mean_exec_time > 1000) as slow_queries
                FROM pg_stat_statements";

            using (var cmd = new NpgsqlCommand(querySql, connection))
            {
                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    health.TotalQueries = reader.GetInt64(0);
                    health.AverageQueryTimeMs = reader.IsDBNull(1) ? 0 : reader.GetDouble(1);
                    health.SlowQueries = reader.GetInt64(2);
                }
            }

            // Determine overall health status
            health.Status = DetermineHealthStatus(health);
            health.Issues = GenerateHealthIssues(health);

            _logger.LogInformation($"PostgreSQL health check complete: {health.Status}");
            return health;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting PostgreSQL health");
            throw;
        }
    }

    public async Task<DatabaseSize> GetDatabaseSizeAsync()
    {
        try
        {
            _logger.LogInformation("Getting PostgreSQL database size...");

            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var sql = @"
                SELECT 
                    pg_database_size(current_database()) as total_size,
                    pg_database_size(current_database()) as data_size,
                    0 as log_size,
                    SUM(pg_total_relation_size(indexrelid)) as index_size
                FROM pg_stat_user_indexes";

            using var command = new NpgsqlCommand(sql, connection);
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var totalSize = reader.GetInt64(0);
                var indexSize = reader.IsDBNull(3) ? 0 : reader.GetInt64(3);

                return new DatabaseSize
                {
                    DatabaseName = connection.Database,
                    TotalSizeBytes = totalSize,
                    DataSizeBytes = totalSize - indexSize,
                    LogSizeBytes = 0, // PostgreSQL WAL logs not included
                    IndexSizeBytes = indexSize,
                    FreeSpaceBytes = 0, // Would need system query
                    GrowthRateBytesPerDay = 0, // Would need historical data
                    LastMeasured = DateTime.UtcNow,
                    ProjectedSizeIn30Days = totalSize + (totalSize / 100), // Estimate 1% growth
                    ProjectedSizeIn90Days = totalSize + (totalSize / 100 * 3)
                };
            }

            throw new Exception("Failed to get database size");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting database size");
            throw;
        }
    }

    public async Task<ConnectionStatistics> GetConnectionStatsAsync()
    {
        try
        {
            _logger.LogInformation("Getting PostgreSQL connection statistics...");

            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var sql = @"
                SELECT 
                    COUNT(*) as total,
                    COUNT(*) FILTER (WHERE state = 'active') as active,
                    COUNT(*) FILTER (WHERE state = 'idle') as idle,
                    COUNT(*) FILTER (WHERE state = 'idle in transaction') as sleeping,
                    (SELECT setting::int FROM pg_settings WHERE name = 'max_connections') as max_conn
                FROM pg_stat_activity";

            using var command = new NpgsqlCommand(sql, connection);
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new ConnectionStatistics
                {
                    MeasuredAt = DateTime.UtcNow,
                    TotalConnections = reader.GetInt32(0),
                    ActiveConnections = reader.GetInt32(1),
                    IdleConnections = reader.GetInt32(2),
                    SleepingConnections = reader.GetInt32(3),
                    MaxConnections = reader.GetInt32(4),
                    ConnectionsByDatabase = new Dictionary<string, int>(),
                    ConnectionsByUser = new Dictionary<string, int>(),
                    PeakConnections24h = reader.GetInt32(0), // Would need historical tracking
                    PeakConnectionsTime = DateTime.UtcNow
                };
            }

            throw new Exception("Failed to get connection statistics");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting connection statistics");
            throw;
        }
    }

    public async Task<ResourceUtilization> GetResourceUtilizationAsync()
    {
        try
        {
            _logger.LogInformation("Getting PostgreSQL resource utilization...");

            // PostgreSQL doesn't expose OS-level metrics directly
            // Would need to use pg_stat_bgwriter, pg_stat_database, etc.
            return new ResourceUtilization
            {
                MeasuredAt = DateTime.UtcNow,
                CpuUsagePercent = 0, // Requires OS-level monitoring
                CpuUsageDatabasePercent = 0,
                CpuUsageSystemPercent = 0,
                TotalMemoryBytes = 0, // Requires OS-level monitoring
                UsedMemoryBytes = 0,
                FreeMemoryBytes = 0,
                BufferCacheBytes = 0,
                ProcedureCacheBytes = 0,
                DiskReadsPerSecond = 0,
                DiskWritesPerSecond = 0,
                DiskReadLatencyMs = 0,
                DiskWriteLatencyMs = 0,
                NetworkBytesReceivedPerSecond = 0,
                NetworkBytesSentPerSecond = 0,
                TopWaits = new Dictionary<string, WaitStatistic>()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting resource utilization");
            throw;
        }
    }

    public async Task<Dictionary<string, string>> GetConfigurationAsync()
    {
        try
        {
            _logger.LogInformation("Getting PostgreSQL configuration...");

            var config = new Dictionary<string, string>();

            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var sql = "SELECT name, setting FROM pg_settings WHERE category LIKE '%Performance%' OR category LIKE '%Resource%'";

            using var command = new NpgsqlCommand(sql, connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                config[reader.GetString(0)] = reader.GetString(1);
            }

            _logger.LogInformation($"Retrieved {config.Count} configuration settings");
            return config;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting configuration");
            throw;
        }
    }

    #region Private Helper Methods

    private HealthStatus DetermineHealthStatus(DatabaseHealth health)
    {
        if (health.ConnectionUsagePercent > 90 || health.SlowQueries > 100)
            return HealthStatus.Critical;

        if (health.ConnectionUsagePercent > 70 || health.SlowQueries > 50)
            return HealthStatus.Warning;

        return HealthStatus.Healthy;
    }

    private List<HealthIssue> GenerateHealthIssues(DatabaseHealth health)
    {
        var issues = new List<HealthIssue>();

        if (health.ConnectionUsagePercent > 80)
        {
            issues.Add(new HealthIssue
            {
                Category = "Connections",
                Description = $"Connection usage is high: {health.ConnectionUsagePercent:F1}%",
                Severity = health.ConnectionUsagePercent > 90 ? "Critical" : "Warning",
                Recommendation = "Consider increasing max_connections or investigating connection leaks"
            });
        }

        if (health.SlowQueries > 50)
        {
            issues.Add(new HealthIssue
            {
                Category = "Performance",
                Description = $"High number of slow queries: {health.SlowQueries}",
                Severity = health.SlowQueries > 100 ? "Critical" : "Warning",
                Recommendation = "Review slow queries and consider optimization or indexing"
            });
        }

        return issues;
    }

    #endregion
}
