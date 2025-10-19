using DBOptimizer.Core.Models;

namespace DBOptimizer.Core.Services;

public interface ISqlQueryMonitorService
{
    Task<List<SqlQueryMetric>> GetTopExpensiveQueriesAsync(int topCount = 20);
    Task<List<SqlQueryMetric>> GetQueryStatisticsAsync();
    Task StartMonitoringAsync(CancellationToken cancellationToken = default);
    Task StopMonitoringAsync();
    event EventHandler<SqlQueryMetric>? NewMetricCollected;
}


