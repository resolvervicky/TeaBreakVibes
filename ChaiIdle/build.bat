@echo off
REM Da, build script ba! Single-file exe for viral launch 🍵

setlocal enabledelayedexpansion

echo ====================================
echo ChaiIdle - Build Script
echo Da, oru tea vadikka sollrindhu thiyanum ba!
echo ====================================
echo.

cd /d %~dp0

REM Check if dotnet is installed
dotnet --version >nul 2>&1
if errorlevel 1 (
    echo ❌ .NET SDK not found!
    echo Install from: https://dotnet.microsoft.com/download/dotnet/8.0
    pause
    exit /b 1
)

echo ✅ .NET SDK found
echo.

REM Display menu
echo Choose build type:
echo 1 - Debug (fast, for testing)
echo 2 - Release (optimized)
echo 3 - Single-File EXE (for viral launch!)
echo.

set /p choice="Enter choice [1-3]: "

if "%choice%"=="1" goto debug
if "%choice%"=="2" goto release
if "%choice%"=="3" goto singlefile
goto invalid

:debug
echo.
echo Building Debug version...
dotnet build -c Debug
if errorlevel 1 goto error
echo.
echo ✅ Debug build complete!
echo Run: dotnet run
pause
exit /b 0

:release
echo.
echo Building Release version...
dotnet build -c Release
if errorlevel 1 goto error
echo.
echo ✅ Release build complete!
echo Run: bin\Release\net8.0-windows\ChaiIdle.exe
pause
exit /b 0

:singlefile
echo.
echo 🚀 Publishing single-file executable...
echo This may take 2-3 minutes...
echo.

dotnet publish -c Release -r win-x64 ^
  --self-contained true ^
  /p:PublishSingleFile=true ^
  /p:IncludeNativeLibrariesForSelfExtract=true ^
  /p:DebugType=embedded ^
  /p:PublishTrimmed=false

if errorlevel 1 goto error

echo.
echo ✅ Single-file EXE created!
echo Location: bin\Release\net8.0-windows\win-x64\publish\ChaiIdle.exe
echo.
echo Next steps:
echo 1. Copy ChaiIdle.exe anywhere
echo 2. Double-click to run (no installation needed!)
echo 3. Right-click tray icon to configure
echo.
pause
exit /b 0

:invalid
echo ❌ Invalid choice!
pause
exit /b 1

:error
echo ❌ Build failed!
pause
exit /b 1
