using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DBOptimizer.Core.Services;

public interface ITrendingService
{
    Task RecordAsync(string metricName, double value, DateTime timestampUtc);
    Task<IReadOnlyList<(DateTime TimestampUtc, double Value)>> QueryAsync(string metricName, DateTime fromUtc, DateTime toUtc);
}

