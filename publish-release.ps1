# Database Performance Optimizer - Release Publishing Script
param(
    [string]$OutputPath = "./publish",
    [string]$Runtime = "win-x64"
)

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Publishing Database Performance Optimizer" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "Configuration:" -ForegroundColor Yellow
Write-Host "  Output Path: $OutputPath" -ForegroundColor Gray
Write-Host "  Runtime: $Runtime" -ForegroundColor Gray
Write-Host "  Mode: Self-Contained Single File" -ForegroundColor Gray
Write-Host ""

# Clean
Write-Host "Cleaning previous builds..." -ForegroundColor Yellow
dotnet clean DBOptimizer.WpfApp/DBOptimizer.WpfApp.csproj -v quiet

# Publish
Write-Host "Publishing application..." -ForegroundColor Yellow
dotnet publish DBOptimizer.WpfApp/DBOptimizer.WpfApp.csproj `
  --configuration Release `
  --runtime $Runtime `
  --self-contained true `
  --output $OutputPath `
  /p:PublishSingleFile=true `
  /p:IncludeNativeLibrariesForSelfExtract=true `
  /p:EnableCompressionInSingleFile=true `
  /p:PublishReadyToRun=true `
  /p:ReadyToRunUseCrossgen2=true `
  /p:PublishTrimmed=true `
  /p:TrimMode=partial `
  /p:DebugType=None `
  /p:DebugSymbols=false

if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "✅ Publish successful!" -ForegroundColor Green
    Write-Host ""
    Write-Host "Published files:" -ForegroundColor Cyan
    Get-ChildItem $OutputPath -Filter "*.exe" | ForEach-Object {
        $sizeInMB = [math]::Round($_.Length / 1MB, 2)
        Write-Host "  📦 $($_.Name) - $sizeInMB MB" -ForegroundColor White
    }
    Write-Host ""
    Write-Host "Ready for distribution!" -ForegroundColor Green
    Write-Host "Location: $((Resolve-Path $OutputPath).Path)" -ForegroundColor Gray
    Write-Host ""
    
    # Create README
    $readmeContent = @"
# Database Performance Optimizer - Release Package

## Installation

No installation required - just run the executable:

``DBOptimizer.WpfApp.exe``

## First-Time Setup

1. Start the application
2. Go to Settings tab
3. Create new connection profile
4. Configure SQL Server connection
5. Click Connect
6. Start monitoring from Dashboard

## System Requirements

- Windows 10 (Build 17763) or later
- Windows 11 (any version)
- Network access to SQL Server
- SQL permissions: db_datareader + VIEW SERVER STATE

## Support

For issues or questions, contact your IT support team.

---
Version: 1.0.0
Build Date: $(Get-Date -Format 'yyyy-MM-dd')
"@
    
    $readmeContent | Out-File -FilePath "$OutputPath\README.txt" -Encoding UTF8
    
    Write-Host "📄 README.txt created" -ForegroundColor Cyan
    Write-Host ""
    
} else {
    Write-Host "❌ Publish failed! Check errors above." -ForegroundColor Red
}

Write-Host "Press any key to exit..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")



