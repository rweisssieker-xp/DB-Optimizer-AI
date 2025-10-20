using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DBOptimizer.Core.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace DBOptimizer.WpfApp.ViewModels;

/// <summary>
/// ViewModel for Compliance & Audit Trail
/// </summary>
public partial class ComplianceAuditViewModel : ObservableObject
{
    private readonly ILogger<ComplianceAuditViewModel> _logger;
    private readonly IComplianceAuditService _complianceService;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private bool _hasData;

    [ObservableProperty]
    private string _errorMessage;

    // Time period
    [ObservableProperty]
    private DateTime _fromDate = DateTime.UtcNow.AddMonths(-3);

    [ObservableProperty]
    private DateTime _toDate = DateTime.UtcNow;

    // Statistics
    [ObservableProperty]
    private int _totalChanges;

    [ObservableProperty]
    private int _autoApplied;

    [ObservableProperty]
    private int _manualReview;

    [ObservableProperty]
    private int _rollbacks;

    [ObservableProperty]
    private double _successRate;

    // Compliance Status
    [ObservableProperty]
    private string _complianceStatus;

    [ObservableProperty]
    private string _complianceStatusColor;

    // SLA Compliance
    [ObservableProperty]
    private double _responseTimeSla;

    [ObservableProperty]
    private bool _responseTimeMet;

    [ObservableProperty]
    private double _uptimeSla;

    [ObservableProperty]
    private bool _uptimeMet;

    [ObservableProperty]
    private double _queryPerformanceSla;

    [ObservableProperty]
    private bool _queryPerformanceMet;

    [ObservableProperty]
    private double _incidentResponseSla;

    [ObservableProperty]
    private bool _incidentResponseMet;

    [ObservableProperty]
    private double _overallSlaCompliance;

    // Audit Trail
    [ObservableProperty]
    private ObservableCollection<AuditEntryItem> _auditTrail;

    [ObservableProperty]
    private ObservableCollection<ComplianceCheckItem> _complianceChecks;

    [ObservableProperty]
    private ObservableCollection<HighImpactChangeItem> _highImpactChanges;

    // Selected compliance standard
    [ObservableProperty]
    private string _selectedStandard = "SOX";

    public ComplianceAuditViewModel(
        ILogger<ComplianceAuditViewModel> logger,
        IComplianceAuditService complianceService)
    {
        _logger = logger;
        _complianceService = complianceService;

        AuditTrail = new ObservableCollection<AuditEntryItem>();
        ComplianceChecks = new ObservableCollection<ComplianceCheckItem>();
        HighImpactChanges = new ObservableCollection<HighImpactChangeItem>();
    }

    [RelayCommand]
    private async Task LoadDataAsync()
    {
        if (IsLoading) return;

        try
        {
            IsLoading = true;
            HasData = false;
            ErrorMessage = string.Empty;

            _logger.LogInformation("Loading compliance & audit data...");

            // Load statistics
            var statistics = await _complianceService.GetChangeStatisticsAsync(FromDate, ToDate);
            TotalChanges = statistics.TotalOptimizations;
            AutoApplied = statistics.AutoApplied;
            ManualReview = statistics.ManualReview;
            Rollbacks = statistics.Rollbacks;
            SuccessRate = statistics.SuccessRate;

            // Load compliance report
            var standard = Enum.Parse<ComplianceStandard>(SelectedStandard);
            var report = await _complianceService.GenerateComplianceReportAsync(standard, FromDate, ToDate);

            ComplianceStatus = report.OverallStatus.ToString();
            ComplianceStatusColor = report.OverallStatus switch
            {
                Core.Services.ComplianceStatus.Compliant => "#4CAF50",
                Core.Services.ComplianceStatus.PartiallyCompliant => "#FF9800",
                Core.Services.ComplianceStatus.NonCompliant => "#F44336",
                _ => "#9E9E9E"
            };

            // Load compliance checks
            ComplianceChecks.Clear();
            foreach (var check in report.ComplianceChecks)
            {
                ComplianceChecks.Add(new ComplianceCheckItem
                {
                    Name = check.CheckName,
                    Description = check.Description,
                    Passed = check.Passed,
                    Result = check.Result,
                    Evidence = check.Evidence,
                    StatusIcon = check.Passed ? "✅" : "❌",
                    StatusColor = check.Passed ? "#4CAF50" : "#F44336"
                });
            }

            // Load high-impact changes
            HighImpactChanges.Clear();
            foreach (var change in report.HighImpactChanges.Take(5))
            {
                HighImpactChanges.Add(new HighImpactChangeItem
                {
                    Timestamp = change.Timestamp.ToString("yyyy-MM-dd HH:mm"),
                    ActionType = change.ActionType,
                    TargetObject = change.TargetObject,
                    ImpactPercentage = change.ImpactPercentage,
                    UserEmail = change.UserEmail,
                    ApprovedBy = change.ApprovedBy ?? "Auto-Applied",
                    Status = change.Status.ToString()
                });
            }

            // Load SLA compliance
            var slaStatus = await _complianceService.GetSlaComplianceAsync(FromDate, ToDate);
            ResponseTimeSla = slaStatus.ResponseTimeSla.CompliancePercent;
            ResponseTimeMet = slaStatus.ResponseTimeSla.Met;
            UptimeSla = slaStatus.UptimeSla.CompliancePercent;
            UptimeMet = slaStatus.UptimeSla.Met;
            QueryPerformanceSla = slaStatus.QueryPerformanceSla.CompliancePercent;
            QueryPerformanceMet = slaStatus.QueryPerformanceSla.Met;
            IncidentResponseSla = slaStatus.IncidentResponseSla.CompliancePercent;
            IncidentResponseMet = slaStatus.IncidentResponseSla.Met;
            OverallSlaCompliance = slaStatus.OverallCompliancePercent;

            // Load audit trail (top 20)
            var auditTrail = await _complianceService.GetAuditTrailAsync(FromDate, ToDate);
            AuditTrail.Clear();
            foreach (var entry in auditTrail.Take(20))
            {
                AuditTrail.Add(new AuditEntryItem
                {
                    Timestamp = entry.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"),
                    ActionType = entry.ActionType,
                    Description = entry.Description,
                    UserEmail = entry.UserEmail,
                    ApprovedBy = entry.ApprovedBy ?? (entry.AutoApplied ? "Auto-Applied" : "N/A"),
                    Impact = $"{entry.ImpactPercentage:F1}%",
                    Status = entry.Status.ToString(),
                    StatusColor = entry.Status switch
                    {
                        ChangeStatus.Completed => "#4CAF50",
                        ChangeStatus.RolledBack => "#FF9800",
                        ChangeStatus.Failed => "#F44336",
                        _ => "#2196F3"
                    }
                });
            }

            HasData = true;
            _logger.LogInformation("Compliance & audit data loaded successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading compliance & audit data");
            ErrorMessage = $"Failed to load data: {ex.Message}";
            HasData = false;
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadDataAsync();
    }

    [RelayCommand]
    private async Task ExportAuditTrailAsync(string format)
    {
        try
        {
            _logger.LogInformation($"Exporting audit trail to {format}...");

            var exportFormat = Enum.Parse<ExportFormat>(format, true);
            var data = await _complianceService.ExportAuditTrailAsync(FromDate, ToDate, exportFormat);

            // Save file
            var fileName = $"AuditTrail_{DateTime.Now:yyyyMMdd}.{format.ToLower()}";
            var filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), fileName);
            await System.IO.File.WriteAllBytesAsync(filePath, data);

            _logger.LogInformation($"Audit trail exported to {filePath}");

            System.Windows.MessageBox.Show(
                $"Audit trail exported successfully to:\n{filePath}",
                "Export Successful",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error exporting audit trail to {format}");
            System.Windows.MessageBox.Show(
                $"Failed to export audit trail: {ex.Message}",
                "Export Error",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private async Task ChangeStandardAsync(string standard)
    {
        SelectedStandard = standard;
        await LoadDataAsync();
    }

    public async Task InitializeAsync()
    {
        await LoadDataAsync();
    }
}

public class AuditEntryItem
{
    public string Timestamp { get; set; }
    public string ActionType { get; set; }
    public string Description { get; set; }
    public string UserEmail { get; set; }
    public string ApprovedBy { get; set; }
    public string Impact { get; set; }
    public string Status { get; set; }
    public string StatusColor { get; set; }
}

public class ComplianceCheckItem
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool Passed { get; set; }
    public string Result { get; set; }
    public string Evidence { get; set; }
    public string StatusIcon { get; set; }
    public string StatusColor { get; set; }
}

public class HighImpactChangeItem
{
    public string Timestamp { get; set; }
    public string ActionType { get; set; }
    public string TargetObject { get; set; }
    public double ImpactPercentage { get; set; }
    public string UserEmail { get; set; }
    public string ApprovedBy { get; set; }
    public string Status { get; set; }
}
