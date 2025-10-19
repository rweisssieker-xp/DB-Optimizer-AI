# delete-aos-files.ps1
# Löscht alle AOS-bezogenen Dateien, die nicht mehr benötigt werden

$rootPath = $PSScriptRoot

Write-Host "Lösche AOS-Dateien..." -ForegroundColor Yellow

$filesToDelete = @(
    "DBOptimizer.Core\Services\AosMonitorService.cs",
    "DBOptimizer.Core\Services\IAosMonitorService.cs",
    "DBOptimizer.Core\Models\AosMetric.cs",
    "DBOptimizer.WpfApp\Views\AosMonitoringView.xaml",
    "DBOptimizer.WpfApp\Views\AosMonitoringView.xaml.cs",
    "DBOptimizer.WpfApp\ViewModels\AosMonitoringViewModel.cs"
)

$deletedCount = 0
foreach ($file in $filesToDelete) {
    $fullPath = Join-Path $rootPath $file
    if (Test-Path $fullPath) {
        Remove-Item $fullPath -Force
        Write-Host "  ✓ Gelöscht: $file" -ForegroundColor Gray
        $deletedCount++
    } else {
        Write-Host "  ⊘ Nicht gefunden: $file" -ForegroundColor DarkGray
    }
}

Write-Host "`n✅ $deletedCount Dateien gelöscht" -ForegroundColor Green

