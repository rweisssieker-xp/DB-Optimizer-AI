# rename-project.ps1
# Automatisches Umbenennen des Projekts von DBOptimizer zu DBOptimizer

$rootPath = $PSScriptRoot

Write-Host "Project Rename: DBOptimizer → DBOptimizer" -ForegroundColor Cyan
$confirmation = Read-Host "Fortfahren? (ja/nein)"
if ($confirmation -ne "ja") { exit }

# 1. Ordner umbenennen
Write-Host "`n1. Ordner umbenennen..." -ForegroundColor Green
if (Test-Path "$rootPath\DBOptimizer.Core") {
    Rename-Item "$rootPath\DBOptimizer.Core" "DBOptimizer.Core"
}
if (Test-Path "$rootPath\DBOptimizer.Data") {
    Rename-Item "$rootPath\DBOptimizer.Data" "DBOptimizer.Data"
}
if (Test-Path "$rootPath\DBOptimizer.WpfApp") {
    Rename-Item "$rootPath\DBOptimizer.WpfApp" "DBOptimizer.WpfApp"
}
if (Test-Path "$rootPath\DBOptimizer.sln") {
    Rename-Item "$rootPath\DBOptimizer.sln" "DBOptimizer.sln"
}

# 2. C# Dateien
Write-Host "2. C# Dateien aktualisieren..." -ForegroundColor Green
$csFiles = Get-ChildItem -Path $rootPath -Filter "*.cs" -Recurse | Where-Object { $_.FullName -notmatch "\\obj\\|\\bin\\" }
foreach ($file in $csFiles) {
    $content = Get-Content $file.FullName -Raw
    $content = $content -replace "DBOptimizer\.Core", "DBOptimizer.Core"
    $content = $content -replace "DBOptimizer\.Data", "DBOptimizer.Data"
    $content = $content -replace "DBOptimizer\.WpfApp", "DBOptimizer.WpfApp"
    Set-Content $file.FullName -Value $content -NoNewline
}

# 3. XAML Dateien
Write-Host "3. XAML Dateien aktualisieren..." -ForegroundColor Green
$xamlFiles = Get-ChildItem -Path $rootPath -Filter "*.xaml" -Recurse | Where-Object { $_.FullName -notmatch "\\obj\\|\\bin\\" }
foreach ($file in $xamlFiles) {
    $content = Get-Content $file.FullName -Raw
    $content = $content -replace "DBOptimizer\.WpfApp", "DBOptimizer.WpfApp"
    $content = $content -replace "Database Performance Optimizer", "Database Performance Optimizer"
    Set-Content $file.FullName -Value $content -NoNewline
}

# 4. .csproj Dateien
Write-Host "4. Projekt-Dateien aktualisieren..." -ForegroundColor Green
$csprojFiles = Get-ChildItem -Path $rootPath -Filter "*.csproj" -Recurse | Where-Object { $_.FullName -notmatch "\\obj\\|\\bin\\" }
foreach ($file in $csprojFiles) {
    $content = Get-Content $file.FullName -Raw
    $content = $content -replace "DBOptimizer", "DBOptimizer"
    Set-Content $file.FullName -Value $content -NoNewline
}

# 5. Solution
Write-Host "5. Solution-Datei aktualisieren..." -ForegroundColor Green
if (Test-Path "$rootPath\DBOptimizer.sln") {
    $content = Get-Content "$rootPath\DBOptimizer.sln" -Raw
    $content = $content -replace "DBOptimizer", "DBOptimizer"
    Set-Content "$rootPath\DBOptimizer.sln" -Value $content -NoNewline
}

# 6. Scripts
Write-Host "6. PowerShell Scripts aktualisieren..." -ForegroundColor Green
$ps1Files = Get-ChildItem -Path $rootPath -Filter "*.ps1" | Where-Object { $_.Name -ne "rename-project.ps1" }
foreach ($file in $ps1Files) {
    $content = Get-Content $file.FullName -Raw
    $content = $content -replace "DBOptimizer\.WpfApp", "DBOptimizer.WpfApp"
    Set-Content $file.FullName -Value $content -NoNewline
}

Write-Host "`n✅ Fertig! Nächste Schritte:" -ForegroundColor Green
Write-Host "  1. Lösche bin/obj Ordner"
Write-Host "  2. Öffne DBOptimizer.sln in Visual Studio"
Write-Host "  3. Rebuild Solution"

