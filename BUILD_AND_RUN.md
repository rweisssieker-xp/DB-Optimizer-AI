# 🚀 Build & Run Guide - DB Optimizer AI

**Quick Start Guide für lokales Testing**

---

## ✅ Voraussetzungen

### **Software Requirements**:
- ✅ .NET 8 SDK (oder höher)
- ✅ Visual Studio 2022 (oder VS Code + C# Extension)
- ✅ Windows 10/11 (für WPF)
- ✅ Git (optional)

### **Check Installation**:
```powershell
# .NET Version prüfen
dotnet --version
# Should show: 8.0.x or higher

# SDK List
dotnet --list-sdks
```

---

## 📦 Build Instructions

### **Option 1: Visual Studio** (Empfohlen)

1. **Solution öffnen**:
   ```
   File → Open → Solution...
   Navigate to: C:\tmp\DB-Optimizer-AI\DBOptimizer.sln
   ```

2. **Restore NuGet Packages**:
   ```
   Right-click Solution → Restore NuGet Packages
   ```

3. **Build Solution**:
   ```
   Build → Build Solution (Ctrl+Shift+B)
   ```

4. **Run Application**:
   ```
   Debug → Start Debugging (F5)
   Or: Debug → Start Without Debugging (Ctrl+F5)
   ```

---

### **Option 2: Command Line**

```powershell
# 1. Navigate to project directory
cd C:\tmp\DB-Optimizer-AI

# 2. Restore dependencies
dotnet restore

# 3. Build solution
dotnet build

# 4. Run application
dotnet run --project DBOptimizer.WpfApp
```

---

### **Option 3: PowerShell Script** (Schnellstart)

```powershell
# Use existing build script
.\build-and-run.ps1
```

---

## 🧪 Testing Checklist

### **1. Application Start**
- [ ] Application starts without errors
- [ ] MainWindow opens
- [ ] All tabs visible

### **2. Executive Dashboard Tab**
```
Steps:
1. Click "📊 Executive Dashboard" tab
2. Wait for data to load (loading spinner)
3. Verify health score displays (should be ~87)
4. Check metric cards (4 cards)
5. Verify achievements list
6. Check investment summary
7. Test "🔄 Refresh" button
8. Test "📥 Export PDF" button
```

**Expected**:
- Health Score: 87 (B+)
- Query Performance: A-
- Monthly Savings: €12,450
- ROI: 847%

### **3. Compliance & Audit Tab**
```
Steps:
1. Click "📝 Compliance & Audit" tab
2. Wait for data to load
3. Verify statistics cards (5 cards)
4. Check compliance status
5. Test standard selector buttons (SOX, GDPR, etc.)
6. Verify audit trail list
7. Test "📥 Export CSV" button
```

**Expected**:
- Total Changes: 127
- Success Rate: 98.4%
- Compliance Status: Compliant
- SLA Compliance: 98.7%

### **4. Other Tabs**
- [ ] 🏠 Dashboard - Loads without error
- [ ] ⚙️ Settings - Configuration UI works
- [ ] 📈 SQL Performance - Query monitoring works
- [ ] 💾 Database Health - Health metrics display
- [ ] Other innovative features load

---

## 🐛 Troubleshooting

### **Problem: Build Errors**

**Error: "Package not found"**
```powershell
# Solution: Restore NuGet packages
dotnet restore
dotnet clean
dotnet build
```

**Error: ".NET SDK not found"**
```powershell
# Download and install .NET 8 SDK
# https://dotnet.microsoft.com/download/dotnet/8.0
```

**Error: "Project reference error"**
```powershell
# Rebuild all projects
dotnet clean
dotnet build --no-incremental
```

---

### **Problem: Runtime Errors**

**Error: "Service not registered"**
```
Check: App.xaml.cs
Verify: All services are registered in DI container
```

**Error: "ViewModel not found"**
```
Check: ViewModels are registered as Transient
Check: DataContext is set in View
```

**Error: "Database connection failed"**
```
Check: Settings → Connection String
Verify: SQL Server is running
Test: Connection in Settings tab
```

---

### **Problem: UI Issues**

**Empty Dashboard**:
```
1. Check if IsLoading = false
2. Check if HasData = true
3. Check if ErrorMessage is empty
4. Verify ViewModel.InitializeAsync() is called
```

**No Data Displayed**:
```
1. Check bindings in XAML
2. Verify ViewModel properties are populated
3. Check Converter registration in App.xaml
4. Inspect Visual Studio Output window for binding errors
```

---

## 📊 Performance Check

### **Expected Performance**:
- Application Startup: < 5 seconds
- Dashboard Load: < 2 seconds
- Compliance Load: < 2 seconds
- Export Operation: < 1 second

### **Memory Usage**:
- Idle: ~100-150 MB
- Active (all tabs): ~200-300 MB
- Peak (with charts): ~400 MB

---

## 🔧 Development Mode

### **Enable Debug Logging**:
```csharp
// In App.xaml.cs
.ConfigureLogging((context, logging) =>
{
    logging.SetMinimumLevel(LogLevel.Debug);
    logging.AddDebug();
    logging.AddConsole();
})
```

### **Hot Reload** (für UI Development):
```powershell
# Run with hot reload enabled
dotnet watch run --project DBOptimizer.WpfApp
```

### **Inspect XAML**:
```
Debug → Windows → Live Visual Tree
Debug → Windows → Live Property Explorer
```

---

## 📦 Publish for Production

### **Create Release Build**:
```powershell
# Build release
dotnet publish DBOptimizer.WpfApp `
    -c Release `
    -o ./publish `
    --self-contained false

# Build self-contained (no .NET runtime required)
dotnet publish DBOptimizer.WpfApp `
    -c Release `
    -o ./publish-standalone `
    --self-contained true `
    -r win-x64
```

### **Output**:
```
./publish/
├── DBOptimizer.WpfApp.exe
├── DBOptimizer.Core.dll
├── DBOptimizer.Data.dll
├── *.dll (dependencies)
└── ...
```

### **Run Published App**:
```powershell
cd publish
.\DBOptimizer.WpfApp.exe
```

---

## 🎯 Quick Command Reference

| Task | Command |
|------|---------|
| **Build** | `dotnet build` |
| **Run** | `dotnet run --project DBOptimizer.WpfApp` |
| **Clean** | `dotnet clean` |
| **Restore** | `dotnet restore` |
| **Test** | `dotnet test` |
| **Publish** | `dotnet publish -c Release` |
| **Watch** | `dotnet watch run` |

---

## 🔍 Debugging Tips

### **XAML Binding Errors**:
```
1. Open Output window (Ctrl+Alt+O)
2. Filter by "Binding" errors
3. Look for "System.Windows.Data Error"
```

### **Service Resolution Errors**:
```csharp
// Test service resolution in App.xaml.cs OnStartup
var dashboardService = _host.Services.GetRequiredService<IExecutiveDashboardService>();
Console.WriteLine($"Service resolved: {dashboardService != null}");
```

### **ViewModel Data**:
```csharp
// Add debug output in ViewModel
_logger.LogInformation($"Health Score: {HealthScore}");
_logger.LogInformation($"Achievements Count: {Achievements.Count}");
```

---

## 📚 Nützliche Links

- [.NET 8 Documentation](https://docs.microsoft.com/dotnet/)
- [WPF Documentation](https://docs.microsoft.com/dotnet/desktop/wpf/)
- [MVVM Toolkit](https://learn.microsoft.com/windows/communitytoolkit/mvvm/introduction)
- [Dependency Injection](https://docs.microsoft.com/dotnet/core/extensions/dependency-injection)

---

## ✅ Success Indicators

**Everything works wenn**:
- ✅ Application starts in < 5 seconds
- ✅ All 3 new tabs load without errors
- ✅ Executive Dashboard displays Health Score
- ✅ Compliance Dashboard shows statistics
- ✅ Export buttons save files successfully
- ✅ No errors in Output window
- ✅ UI is responsive and smooth

---

## 🎉 Ready to Deploy!

**Nach erfolgreichem Test**:
1. ✅ Create installer (optional)
2. ✅ Package with dependencies
3. ✅ Deploy to target environment
4. ✅ Train users
5. ✅ Monitor production

---

**Status**: ✅ Build & Run Guide Complete  
**Version**: 2.0  
**Last Updated**: October 20, 2025
