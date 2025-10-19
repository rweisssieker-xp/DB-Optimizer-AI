using System.Threading.Tasks;

namespace DBOptimizer.Data.SqlServer;

public interface ISqlExecutionPlanService
{
    Task<string?> GetEstimatedPlanXmlAsync(string sqlText, int commandTimeoutSeconds = 15);
}

