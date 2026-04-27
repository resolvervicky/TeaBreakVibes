# ChaiIdle Build Script for PowerShell
# Da, build script ba! Windows PowerShell edition

param(
    [string]$BuildType = "Release",
    [switch]$SingleFile = $false,
    [switch]$Test = $false
)

$projectPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$projectName = "ChaiIdle"

Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "ChaiIdle - PowerShell Build Script" -ForegroundColor Cyan
Write-Host "Da, oru tea vadikka sollrindhu thiyanum ba!" -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""

# Check .NET SDK
Write-Host "📦 Checking .NET SDK..." -ForegroundColor Yellow
$dotnetVersion = dotnet --version 2>$null
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ .NET SDK not found!" -ForegroundColor Red
    Write-Host "   Install from: https://dotnet.microsoft.com/download/dotnet/8.0" -ForegroundColor Red
    exit 1
}
Write-Host "✅ .NET found: $dotnetVersion" -ForegroundColor Green
Write-Host ""

# Restore
Write-Host "📥 Restoring NuGet packages..." -ForegroundColor Yellow
Set-Location $projectPath
dotnet restore 2>&1 | Out-Null
Write-Host "✅ Packages restored" -ForegroundColor Green
Write-Host ""

# Build
if ($BuildType -eq "Debug") {
    Write-Host "🔨 Building Debug version..." -ForegroundColor Yellow
    dotnet build -c Debug
    Write-Host "✅ Debug build complete!" -ForegroundColor Green
    Write-Host "   Run: dotnet run" -ForegroundColor Cyan
}
elseif ($BuildType -eq "Release" -and -not $SingleFile) {
    Write-Host "🔨 Building Release version..." -ForegroundColor Yellow
    dotnet build -c Release
    Write-Host "✅ Release build complete!" -ForegroundColor Green
    Write-Host "   Run: .\bin\Release\net8.0-windows\$projectName.exe" -ForegroundColor Cyan
}
elseif ($SingleFile -or $BuildType -eq "SingleFile") {
    Write-Host "🚀 Publishing single-file executable..." -ForegroundColor Yellow
    Write-Host "   (This may take 2-3 minutes)" -ForegroundColor Yellow
    Write-Host ""
    
    dotnet publish -c Release -r win-x64 `
        --self-contained true `
        /p:PublishSingleFile=true `
        /p:IncludeNativeLibrariesForSelfExtract=true `
        /p:DebugType=embedded `
        /p:PublishTrimmed=false
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host ""
        Write-Host "✅ Single-file EXE created!" -ForegroundColor Green
        $exePath = Join-Path $projectPath "bin\Release\net8.0-windows\win-x64\publish\$projectName.exe"
        Write-Host "   Location: $exePath" -ForegroundColor Cyan
        Write-Host ""
        Write-Host "🎉 Next steps:" -ForegroundColor Green
        Write-Host "   1. Copy $projectName.exe anywhere" -ForegroundColor Cyan
        Write-Host "   2. Double-click to run" -ForegroundColor Cyan
        Write-Host "   3. No installation needed!" -ForegroundColor Cyan
        
        if ($Test) {
            Write-Host ""
            Write-Host "🧪 Running app..." -ForegroundColor Yellow
            & $exePath
        }
    }
    else {
        Write-Host "❌ Build failed!" -ForegroundColor Red
        exit 1
    }
}

Write-Host ""

# Test if requested
if ($Test -and -not ($SingleFile -or $BuildType -eq "SingleFile")) {
    Write-Host "🧪 Running app for testing..." -ForegroundColor Yellow
    Write-Host "   (Leave it running for 5 minutes to test idle detection)" -ForegroundColor Yellow
    Write-Host ""
    
    if ($BuildType -eq "Debug") {
        dotnet run
    }
    else {
        & ".\bin\Release\net8.0-windows\$projectName.exe"
    }
}

Write-Host ""
Write-Host "✨ Da, chai ready ba! ☕" -ForegroundColor Green
