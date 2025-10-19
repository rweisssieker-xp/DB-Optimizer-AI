# Database Performance Optimizer - Build and Run Script
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Database Performance Optimizer" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "Step 1: Cleaning previous builds..." -ForegroundColor Yellow
dotnet clean DBOptimizer.WpfApp/DBOptimizer.WpfApp.csproj -v quiet

Write-Host "Step 2: Building application..." -ForegroundColor Yellow
dotnet build DBOptimizer.WpfApp/DBOptimizer.WpfApp.csproj --configuration Debug -v quiet

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Build successful!" -ForegroundColor Green
    Write-Host ""
    Write-Host "Step 3: Starting application..." -ForegroundColor Yellow
    Start-Process ".\DBOptimizer.WpfApp\bin\Debug\net8.0-windows\DBOptimizer.WpfApp.exe"
    Write-Host "✅ Application started!" -ForegroundColor Green
    Write-Host ""
    Write-Host "Check your screen for the application window." -ForegroundColor Cyan
} else {
    Write-Host "❌ Build failed! Check errors above." -ForegroundColor Red
}

Write-Host ""
Write-Host "Press any key to exit..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")



