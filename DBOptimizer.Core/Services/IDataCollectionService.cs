namespace DBOptimizer.Core.Services;

public interface IDataCollectionService
{
    Task StartCollectionAsync(int intervalMinutes = 15);
    Task StopCollectionAsync();
    bool IsCollecting { get; }
    DateTime? LastCollectionTime { get; }
}

