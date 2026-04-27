# ChaiIdle Build & Publish Guide

## Quick Build Commands

### Debug Build (For Development)
```bash
cd ChaiIdle
dotnet build -c Debug
dotnet run
```

### Release Build (Optimized)
```bash
cd ChaiIdle
dotnet build -c Release
dotnet bin/Release/net8.0-windows/ChaiIdle.exe
```

### Single-File Executable (VIRAL LAUNCH MODE 🚀)
```bash
cd ChaiIdle
dotnet publish -c Release -r win-x64 \
  --self-contained true \
  /p:PublishSingleFile=true \
  /p:IncludeNativeLibrariesForSelfExtract=true \
  /p:DebugType=embedded \
  /p:PublishTrimmed=false
```

Output: `bin/Release/net8.0-windows/win-x64/publish/ChaiIdle.exe`

File size: ~80-120MB (includes .NET runtime)

## For PowerShell Users (Windows)

```powershell
$projectPath = "E:\SaaS\ResolverVicky\Apps\resolvervicky\TeaBreakVibes\ChaiIdle"
Set-Location $projectPath

# Single-file publish (recommended)
dotnet publish -c Release -r win-x64 `
  --self-contained true `
  /p:PublishSingleFile=true `
  /p:IncludeNativeLibrariesForSelfExtract=true `
  /p:DebugType=embedded

# The .exe is ready!
Write-Host "✅ ChaiIdle.exe ready at: $projectPath\bin\Release\net8.0-windows\win-x64\publish\ChaiIdle.exe"
```

## Distribution

1. **GitHub Releases** - Upload single .exe
2. **Windows Package Manager** - Register with `winget`
3. **Chocolatey** - Create package
4. **Direct Download** - Fast, no installation

## Assets Required

Before publishing, ensure these files exist in `Assets/`:

- `tea.mp4` - Tea pouring animation (10-15 seconds)
- `pour.mp3` - Tea pouring ASMR sound (3-5 seconds)
- `tea.ico` - Tray icon (256x256)

**Asset Specs:**
- **MP4:** H.264 codec, transparent background, 1920x1080, 30fps
- **MP3:** 192kbps, mono, ASMR-style (not background music)
- **ICO:** 256x256 PNG converted to ICO

## Testing Before Launch

```bash
# Test idle detection (set to 1 minute for quick testing)
# Edit settings.json: "idleMinutes": 1

# Run app
ChaiIdle.exe

# Wait 1 minute without touching mouse/keyboard
# Overlay should appear

# Click overlay or press key
# Overlay should close

# Right-click tray icon
# Menu should work

# Right-click > Exit
# App should close cleanly
```

## Size Optimization

Current build: ~80-120MB (with .NET runtime embedded)

To reduce:
1. Enable `PublishTrimmed=true` (may break reflection)
2. Use Framework-dependent deployment (~10MB executable, requires .NET 8 on target machine)

## Auto-Update Strategy (Future)

1. Check GitHub Releases API on startup
2. Download new .exe to temp folder
3. Launch installer script
4. Restart app

---

**Da, ready to go viral ba! 🍵**
