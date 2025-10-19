using System.Collections.Generic;
using System.Threading.Tasks;
using DBOptimizer.Core.Models;

namespace DBOptimizer.Core.Services;

public interface IAlertService
{
    Task AddRuleAsync(AlertRule rule);
    Task RemoveRuleAsync(string ruleId);
    Task<IReadOnlyList<AlertRule>> GetRulesAsync();
    Task<IReadOnlyList<AlertRule>> EvaluateAsync(string metricName, double value, DateTime timestampUtc);
}

