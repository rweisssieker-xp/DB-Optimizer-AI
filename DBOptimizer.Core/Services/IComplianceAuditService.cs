using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DBOptimizer.Core.Services;

/// <summary>
/// Service for compliance tracking and audit trail management
/// Provides SOX, GDPR, HIPAA compliance features
/// </summary>
public interface IComplianceAuditService
{
    /// <summary>
    /// Records a change/optimization action for audit trail
    /// </summary>
    Task<AuditEntry> RecordAuditEntryAsync(AuditEntry entry);
    
    /// <summary>
    /// Gets audit trail for a specific time period
    /// </summary>
    Task<List<AuditEntry>> GetAuditTrailAsync(DateTime from, DateTime to);
    
    /// <summary>
    /// Generates compliance report (SOX, GDPR, HIPAA, etc.)
    /// </summary>
    Task<ComplianceReport> GenerateComplianceReportAsync(ComplianceStandard standard, DateTime from, DateTime to);
    
    /// <summary>
    /// Gets SLA compliance status
    /// </summary>
    Task<SlaComplianceStatus> GetSlaComplianceAsync(DateTime from, DateTime to);
    
    /// <summary>
    /// Exports audit trail to various formats
    /// </summary>
    Task<byte[]> ExportAuditTrailAsync(DateTime from, DateTime to, ExportFormat format);
    
    /// <summary>
    /// Gets change statistics for a period
    /// </summary>
    Task<ChangeStatistics> GetChangeStatisticsAsync(DateTime from, DateTime to);
    
    /// <summary>
    /// Verifies rollback availability for a change
    /// </summary>
    Task<RollbackInfo> GetRollbackInfoAsync(string changeId);
}

/// <summary>
/// Audit trail entry for a single change/action
/// </summary>
public class AuditEntry
{
    public string Id { get; set; }
    public DateTime Timestamp { get; set; }
    public string ActionType { get; set; } // Optimization, Index Creation, Query Rewrite, etc.
    public string Description { get; set; }
    public string UserEmail { get; set; }
    public string UserName { get; set; }
    public string ApprovedBy { get; set; }
    public bool AutoApplied { get; set; }
    public bool RequiredApproval { get; set; }
    
    // Change details
    public string TargetObject { get; set; } // Table, Query, Index name
    public string BeforeState { get; set; }
    public string AfterState { get; set; }
    public string SqlScript { get; set; }
    
    // Impact assessment
    public double ImpactPercentage { get; set; }
    public string ImpactDescription { get; set; }
    public int AffectedRows { get; set; }
    
    // Validation
    public bool ValidationPassed { get; set; }
    public List<string> ValidationChecks { get; set; }
    
    // Status
    public ChangeStatus Status { get; set; }
    public string StatusMessage { get; set; }
    
    // Rollback
    public bool RollbackAvailable { get; set; }
    public string RollbackScript { get; set; }
    public DateTime? RolledBackAt { get; set; }
    
    // Monitoring
    public DateTime? MonitoringStarted { get; set; }
    public DateTime? MonitoringEnded { get; set; }
    public int MonitoringDurationDays { get; set; }
}

public enum ChangeStatus
{
    Pending,
    Approved,
    Applied,
    Validated,
    Monitoring,
    Completed,
    RolledBack,
    Failed
}

/// <summary>
/// Compliance report for regulatory requirements
/// </summary>
public class ComplianceReport
{
    public ComplianceStandard Standard { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public DateTime GeneratedAt { get; set; }
    
    // Summary
    public int TotalChanges { get; set; }
    public int ApprovedChanges { get; set; }
    public int AutoAppliedChanges { get; set; }
    public int RolledBackChanges { get; set; }
    public double SuccessRate { get; set; }
    
    // Compliance status
    public ComplianceStatus OverallStatus { get; set; }
    public List<ComplianceCheck> ComplianceChecks { get; set; }
    
    // High-impact changes
    public List<AuditEntry> HighImpactChanges { get; set; }
    
    // Violations (if any)
    public List<ComplianceViolation> Violations { get; set; }
    
    // Recommendations
    public List<string> Recommendations { get; set; }
}

public enum ComplianceStandard
{
    SOX,        // Sarbanes-Oxley
    GDPR,       // General Data Protection Regulation
    HIPAA,      // Health Insurance Portability and Accountability Act
    ISO27001,   // Information Security Management
    PCIDSS,     // Payment Card Industry Data Security Standard
    Custom
}

public enum ComplianceStatus
{
    Compliant,
    PartiallyCompliant,
    NonCompliant,
    Unknown
}

public class ComplianceCheck
{
    public string CheckName { get; set; }
    public string Description { get; set; }
    public bool Passed { get; set; }
    public string Result { get; set; }
    public string Evidence { get; set; }
}

public class ComplianceViolation
{
    public string ViolationType { get; set; }
    public string Description { get; set; }
    public string Severity { get; set; } // Low, Medium, High, Critical
    public string Recommendation { get; set; }
    public DateTime OccurredAt { get; set; }
}

/// <summary>
/// SLA compliance status
/// </summary>
public class SlaComplianceStatus
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    
    // Response Time SLA
    public SlaMetric ResponseTimeSla { get; set; }
    
    // Uptime SLA
    public SlaMetric UptimeSla { get; set; }
    
    // Query Performance SLA
    public SlaMetric QueryPerformanceSla { get; set; }
    
    // Incident Response SLA
    public SlaMetric IncidentResponseSla { get; set; }
    
    // Overall compliance
    public double OverallCompliancePercent { get; set; }
    public ComplianceStatus OverallStatus { get; set; }
}

public class SlaMetric
{
    public string MetricName { get; set; }
    public double TargetValue { get; set; }
    public double ActualValue { get; set; }
    public string Unit { get; set; }
    public double CompliancePercent { get; set; }
    public bool Met { get; set; }
    public List<SlaViolation> Violations { get; set; }
}

public class SlaViolation
{
    public DateTime Timestamp { get; set; }
    public string Description { get; set; }
    public double Duration { get; set; }
    public string Impact { get; set; }
}

/// <summary>
/// Change statistics summary
/// </summary>
public class ChangeStatistics
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    
    // Total counts
    public int TotalOptimizations { get; set; }
    public int AutoApplied { get; set; }
    public int ManualReview { get; set; }
    public int Rollbacks { get; set; }
    
    // Success metrics
    public double SuccessRate { get; set; }
    public double AverageImpact { get; set; }
    
    // By category
    public Dictionary<string, int> ByActionType { get; set; }
    public Dictionary<string, int> ByUser { get; set; }
    
    // Timeline
    public List<DailyChangeCount> DailyTimeline { get; set; }
}

public class DailyChangeCount
{
    public DateTime Date { get; set; }
    public int Count { get; set; }
    public int Successful { get; set; }
    public int Failed { get; set; }
}

/// <summary>
/// Rollback information for a specific change
/// </summary>
public class RollbackInfo
{
    public string ChangeId { get; set; }
    public bool RollbackAvailable { get; set; }
    public string RollbackScript { get; set; }
    public DateTime? RollbackWindowExpires { get; set; }
    public int RollbackWindowHours { get; set; }
    public List<string> Prerequisites { get; set; }
    public List<string> Warnings { get; set; }
    public double EstimatedDuration { get; set; } // minutes
}
