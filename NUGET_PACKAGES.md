# 📦 NuGet Packages Guide - DB Optimizer AI

**Empfohlene Packages für erweiterte Features**

---

## 🎯 Aktuell Installiert (Core)

```xml
<!-- DBOptimizer.Core -->
<PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="8.0.0" />
<PackageReference Include="Microsoft.Extensions.Logging" Version="8.0.0" />

<!-- DBOptimizer.Data -->
<PackageReference Include="System.Data.SqlClient" Version="4.8.6" />

<!-- DBOptimizer.WpfApp -->
<PackageReference Include="Microsoft.Extensions.Hosting" Version="8.0.0" />
<PackageReference Include="CommunityToolkit.Mvvm" Version="8.2.2" />
```

---

## 📊 Charts & Visualisierung

### **Option 1: LiveCharts2** (Empfohlen)
```powershell
# Installation
dotnet add DBOptimizer.WpfApp package LiveChartsCore.SkiaSharpView.WPF
```

**Features**:
- ✅ Modern & performant
- ✅ Multiple chart types (Line, Bar, Pie, etc.)
- ✅ Real-time updates
- ✅ MVVM-friendly
- ✅ Touch & zoom support

**Usage Example**:
```csharp
// ViewModel
public ObservableCollection<ISeries> Series { get; set; } = new()
{
    new LineSeries<double>
    {
        Values = new double[] { 82, 85, 83, 84, 85, 87 },
        Name = "Health Score",
        Fill = null
    }
};

public Axis[] XAxes { get; set; } = new[]
{
    new Axis
    {
        Labels = new[] { "May", "Jun", "Jul", "Aug", "Sep", "Oct" }
    }
};
```

```xml
<!-- XAML -->
<lvc:CartesianChart Series="{Binding Series}" 
                    XAxes="{Binding XAxes}"
                    Height="300"/>
```

**Docs**: https://lvcharts.com/

---

### **Option 2: OxyPlot**
```powershell
# Installation
dotnet add DBOptimizer.WpfApp package OxyPlot.Wpf
```

**Features**:
- ✅ Mature & stable
- ✅ Rich chart types
- ✅ Export to PNG/PDF
- ✅ Scientific plotting

**Usage**:
```xml
<oxy:PlotView Model="{Binding PlotModel}" Height="300"/>
```

---

### **Option 3: ScottPlot** (High Performance)
```powershell
# Installation
dotnet add DBOptimizer.WpfApp package ScottPlot.WPF
```

**Features**:
- ✅ Extremely fast (millions of points)
- ✅ Interactive
- ✅ Scientific visualization

---

## 🌐 PostgreSQL Support

### **Npgsql** (ADO.NET Provider)
```powershell
# Installation
dotnet add DBOptimizer.Data package Npgsql
```

**Features**:
- ✅ Official PostgreSQL driver
- ✅ Full .NET integration
- ✅ Async/await support
- ✅ Binary protocol

**Connection String**:
```csharp
"Host=localhost;Port=5432;Database=mydb;Username=postgres;Password=password"
```

**Usage**:
```csharp
using var connection = new NpgsqlConnection(connectionString);
await connection.OpenAsync();
```

**Docs**: https://www.npgsql.org/

---

## 🗄️ MySQL Support

### **MySqlConnector** (Empfohlen)
```powershell
# Installation
dotnet add DBOptimizer.Data package MySqlConnector
```

**Features**:
- ✅ Async-first
- ✅ High performance
- ✅ Full MySQL 8.0 support

**Connection String**:
```csharp
"Server=localhost;Database=mydb;User=root;Password=password;"
```

---

## 📄 PDF Export

### **QuestPDF** (Modern & Empfohlen)
```powershell
# Installation
dotnet add DBOptimizer.Core package QuestPDF
```

**Features**:
- ✅ Fluent API (super einfach!)
- ✅ Modern layout engine
- ✅ Charts embedding
- ✅ Free for commercial use

**Example**:
```csharp
Document.Create(container =>
{
    container.Page(page =>
    {
        page.Margin(50);
        page.Header().Text("Executive Report").FontSize(20);
        page.Content().Text($"Health Score: {healthScore}");
    });
}).GeneratePdf("report.pdf");
```

**Docs**: https://www.questpdf.com/

---

### **iTextSharp** (Alternative - GPL License!)
```powershell
# Installation - ACHTUNG: GPL Lizenz!
dotnet add DBOptimizer.Core package itext7
```

**Note**: iTextSharp hat AGPL Lizenz - QuestPDF ist die bessere Wahl für kommerzielle Nutzung!

---

## 📊 Excel Export

### **EPPlus** (Empfohlen)
```powershell
# Installation
dotnet add DBOptimizer.Core package EPPlus
```

**Features**:
- ✅ Full Excel 2007+ support
- ✅ Formulas, charts, styling
- ✅ Large datasets

**Example**:
```csharp
using var package = new ExcelPackage();
var worksheet = package.Workbook.Worksheets.Add("Report");

worksheet.Cells["A1"].Value = "Health Score";
worksheet.Cells["B1"].Value = healthScore;

package.SaveAs(new FileInfo("report.xlsx"));
```

**License**: Polyform Noncommercial (Free for non-commercial)  
**Docs**: https://www.epplussoftware.com/

---

### **ClosedXML** (Alternative - MIT License)
```powershell
# Installation
dotnet add DBOptimizer.Core package ClosedXML
```

**Features**:
- ✅ MIT License (komplett frei!)
- ✅ Einfache API
- ✅ Good for basic Excel needs

---

## 🎬 Animationen

### **MahApps.Metro** (UI Framework)
```powershell
# Installation
dotnet add DBOptimizer.WpfApp package MahApps.Metro
```

**Features**:
- ✅ Modern WPF controls
- ✅ Built-in animations
- ✅ Metro design
- ✅ Dark/Light themes

**Usage**:
```xml
<Window xmlns:mah="http://metro.mahapps.com/winfx/xaml/controls">
    <mah:MetroWindow>
        <!-- Your content -->
    </mah:MetroWindow>
</Window>
```

---

### **MaterialDesignThemes** (Alternative)
```powershell
# Installation
dotnet add DBOptimizer.WpfApp package MaterialDesignThemes
```

**Features**:
- ✅ Material Design
- ✅ Rich component library
- ✅ Smooth animations

---

## 🔒 Security & Compliance

### **Serilog** (Structured Logging)
```powershell
# Installation
dotnet add package Serilog.Extensions.Hosting
dotnet add package Serilog.Sinks.File
```

**Features**:
- ✅ Structured logging
- ✅ Multiple sinks (File, DB, etc.)
- ✅ Audit trail ready

---

## 📦 Installation Commands Summary

```powershell
# === CHARTS ===
dotnet add DBOptimizer.WpfApp package LiveChartsCore.SkiaSharpView.WPF

# === DATABASES ===
dotnet add DBOptimizer.Data package Npgsql
dotnet add DBOptimizer.Data package MySqlConnector
dotnet add DBOptimizer.Data package Oracle.ManagedDataAccess.Core

# === EXPORT ===
dotnet add DBOptimizer.Core package QuestPDF
dotnet add DBOptimizer.Core package EPPlus

# === UI ENHANCEMENTS ===
dotnet add DBOptimizer.WpfApp package MahApps.Metro
dotnet add DBOptimizer.WpfApp package MaterialDesignThemes

# === LOGGING ===
dotnet add package Serilog.Extensions.Hosting
dotnet add package Serilog.Sinks.File
```

---

## 🎯 Empfohlene Installation (Minimal)

Für **sofort einsatzbereite Features**:

```powershell
# 1. Charts (für Trends)
dotnet add DBOptimizer.WpfApp package LiveChartsCore.SkiaSharpView.WPF

# 2. PostgreSQL Support
dotnet add DBOptimizer.Data package Npgsql

# 3. PDF Export
dotnet add DBOptimizer.Core package QuestPDF

# 4. Excel Export
dotnet add DBOptimizer.Core package EPPlus
```

**Total**: 4 Packages (~15 MB)

---

## 🚀 Erweiterte Installation (Full Featured)

Für **alle Features**:

```powershell
# Charts
dotnet add DBOptimizer.WpfApp package LiveChartsCore.SkiaSharpView.WPF

# Multi-Database
dotnet add DBOptimizer.Data package Npgsql
dotnet add DBOptimizer.Data package MySqlConnector

# Export
dotnet add DBOptimizer.Core package QuestPDF
dotnet add DBOptimizer.Core package EPPlus

# UI
dotnet add DBOptimizer.WpfApp package MahApps.Metro

# Logging
dotnet add package Serilog.Extensions.Hosting
dotnet add package Serilog.Sinks.File
```

**Total**: 8 Packages (~40 MB)

---

## 📋 Compatibility Matrix

| Package | .NET Version | License | Size |
|---------|-------------|---------|------|
| **LiveCharts2** | .NET 6+ | MIT | ~5 MB |
| **Npgsql** | .NET 6+ | PostgreSQL | ~2 MB |
| **MySqlConnector** | .NET 6+ | MIT | ~1 MB |
| **QuestPDF** | .NET 6+ | MIT | ~3 MB |
| **EPPlus** | .NET 6+ | Polyform | ~4 MB |
| **MahApps.Metro** | .NET 6+ | MIT | ~2 MB |

---

## 🔧 Nach Installation

### **1. Register Services** (App.xaml.cs):
```csharp
// PostgreSQL Adapter
services.AddSingleton<IQueryMonitor>(sp =>
{
    var logger = sp.GetRequiredService<ILogger<PostgreSqlQueryMonitor>>();
    var config = sp.GetRequiredService<IConfigurationService>();
    var connString = config.GetPostgreSqlConnectionString();
    return new PostgreSqlQueryMonitor(logger, connString);
});
```

### **2. Add Using Statements**:
```csharp
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Npgsql;
using QuestPDF.Fluent;
using OfficeOpenXml;
```

### **3. Add XAML Namespaces**:
```xml
xmlns:lvc="clr-namespace:LiveChartsCore.SkiaSharpView.WPF;assembly=LiveChartsCore.SkiaSharpView.WPF"
xmlns:mah="http://metro.mahapps.com/winfx/xaml/controls"
```

---

## ⚖️ License Summary

| Package | License | Commercial Use |
|---------|---------|----------------|
| **LiveCharts2** | MIT | ✅ Free |
| **Npgsql** | PostgreSQL | ✅ Free |
| **MySqlConnector** | MIT | ✅ Free |
| **QuestPDF** | MIT | ✅ Free |
| **EPPlus** | Polyform | ⚠️ Non-commercial only (paid for commercial) |
| **ClosedXML** | MIT | ✅ Free |
| **MahApps.Metro** | MIT | ✅ Free |
| **iText7** | AGPL | ❌ Must be open-source OR paid |

**Empfehlung**: Verwende MIT-lizenzierte Packages für maximale Flexibilität!

---

## 🎉 Quick Start After Installation

```powershell
# 1. Install minimal packages
dotnet add DBOptimizer.WpfApp package LiveChartsCore.SkiaSharpView.WPF
dotnet add DBOptimizer.Data package Npgsql

# 2. Build
dotnet build

# 3. Run
dotnet run --project DBOptimizer.WpfApp

# 4. Test new features
# - PostgreSQL connection
# - Chart displays
```

---

## 📚 Weitere Ressourcen

- **LiveCharts Docs**: https://lvcharts.com/
- **Npgsql Docs**: https://www.npgsql.org/doc/
- **QuestPDF Docs**: https://www.questpdf.com/
- **EPPlus Docs**: https://epplussoftware.com/docs/

---

**Status**: ✅ Package Guide Complete  
**Empfehlung**: Start mit LiveCharts + Npgsql  
**Version**: 2.0  
**Last Updated**: October 20, 2025
