using Microsoft.Data.SqlClient;

namespace DBOptimizer.Data.SqlServer;

public class ConnectionChangedEventArgs : EventArgs
{
    public bool IsConnected { get; set; }
    public string ServerName { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
}

public interface ISqlConnectionManager
{
    event EventHandler<ConnectionChangedEventArgs>? ConnectionChanged;

    Task<SqlConnection> GetConnectionAsync();
    Task<bool> IsConnectedAsync();
    void SetConnectionString(string connectionString);
    string GetConnectionString();

    bool IsConnected { get; }
    string CurrentServerName { get; }
    string CurrentDatabaseName { get; }
}


