using System.Threading.Tasks;

namespace DBOptimizer.Core.Services;

public interface IExecutionPlanService
{
    Task<string?> GetEstimatedPlanXmlAsync(string sqlText, int commandTimeoutSeconds = 15);
}

