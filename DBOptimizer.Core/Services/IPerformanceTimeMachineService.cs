using DBOptimizer.Core.Models;

namespace DBOptimizer.Core.Services;

/// <summary>
/// Time-travel debugging for performance problems - capture and replay
/// </summary>
public interface IPerformanceTimeMachineService
{
    Task<PerformanceSnapshot> CaptureSnapshotAsync(string description = "");
    Task<List<PerformanceSnapshot>> GetSnapshotHistoryAsync(DateTime from, DateTime to);
    Task<PerformanceSnapshot> LoadSnapshotAsync(DateTime timestamp);
    Task<ReplayAnalysis> AnalyzeProblemAsync(DateTime problemTime);
}

