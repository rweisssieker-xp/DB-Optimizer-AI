try {
    Write-Host "Starting application..." -ForegroundColor Yellow
    $process = Start-Process -FilePath "DBOptimizer.WpfApp\bin\Debug\net8.0-windows\DBOptimizer.WpfApp.exe" -PassThru -Wait
    Write-Host "Process exit code: $($process.ExitCode)" -ForegroundColor Cyan
} catch {
    Write-Host "Error starting application: $_" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
}

