# ChaiIdle - Project Structure

```
ChaiIdle/
├── ChaiIdle.csproj                    # Project configuration
│
├── XAML Files (UI)
│   ├── App.xaml                       # WPF Application definition
│   ├── App.xaml.cs
│   ├── MainWindow.xaml                # Main (hidden) window
│   ├── MainWindow.xaml.cs             # App logic, idle detection setup
│   ├── OverlayWindow.xaml             # Tea break overlay
│   └── OverlayWindow.xaml.cs          # Overlay animation & interaction
│
├── Services
│   ├── IdleDetector.cs                # P/Invoke GetLastInputInfo (⭐ core idle detection)
│   ├── SettingsService.cs             # JSON config management
│   └── TrayService.cs                 # System tray icon & menu
│
├── Documentation
│   ├── README.md                      # User guide & features
│   ├── QUICKSTART.md                  # 30-second setup guide (👈 START HERE)
│   ├── BUILD.md                       # Build commands & publishing
│   ├── DEVELOPMENT.md                 # Architecture & tech notes
│   └── STRUCTURE.md                   # This file
│
├── Build Scripts
│   ├── build.bat                      # Windows build menu
│   └── build.sh                       # Linux/Mac build script
│
└── Assets/
    ├── dialogues_template.json        # Default Tamil/English/Hinglish dialogues
    ├── tea.mp4                        # (Add this) Tea animation
    ├── pour.mp3                       # (Add this) ASMR sound
    └── tea.ico                        # (Add this) Tray icon

AppData Storage (created at runtime):
C:\Users\{YourName}\AppData\Roaming\ChaiIdle\
├── settings.json                      # User settings (idle time, languages)
└── dialogues.json                     # Custom memes added by user
```

## File Descriptions

### 🔧 XAML Files

| File | Purpose |
|------|---------|
| `App.xaml[.cs]` | Entry point, application-level settings |
| `MainWindow.xaml[.cs]` | Hidden window, orchestrates idle detection + overlay |
| `OverlayWindow.xaml[.cs]` | **The tea overlay!** Transparent, animation, dialogue, sound |

### 🛠️ Core Services

| File | Purpose | Key Method |
|------|---------|-----------|
| `IdleDetector.cs` | Detects when user is idle using Windows API | `GetLastInputInfo()` P/Invoke |
| `SettingsService.cs` | Loads/saves JSON config, random dialogue picker | `GetRandomDialogue()` |
| `TrayService.cs` | System tray icon, right-click menu | `Initialize()` |

### 📚 Documentation

- **QUICKSTART.md** ← Start here!
- **README.md** ← User guide
- **BUILD.md** ← Publish commands
- **DEVELOPMENT.md** ← Architecture details

### 🎬 Assets

Add these to `Assets/` folder:

| File | Format | Purpose |
|------|--------|---------|
| `tea.mp4` | H.264 MP4 | Tea pouring animation |
| `pour.mp3` | 192kbps MP3 | ASMR pouring sound |
| `tea.ico` | ICO format | Tray icon (optional) |

If missing, app falls back to emoji (🍵) and silent mode.

## Build Output Structure

After building:

```
ChaiIdle/
├── bin/
│   ├── Debug/
│   │   └── net8.0-windows/
│   │       └── ChaiIdle.exe           # Quick debug build
│   └── Release/
│       ├── net8.0-windows/
│       │   └── ChaiIdle.exe           # Release build
│       └── net8.0-windows/win-x64/
│           └── publish/
│               └── ChaiIdle.exe       # ⭐ SINGLE-FILE (viral launch!)
│
└── obj/                               # Build cache (ignore)
```

## Architecture Flow

```
App Start
  ↓
MainWindow (hidden)
  ├→ IdleDetector.Start()
  │  └→ Checks every 10 sec: user idle?
  │     ├→ If idle: ShowOverlay()
  │     │  ├→ OverlayWindow (transparent, top-most)
  │     │  ├→ Load tea.mp4 (or emoji fallback)
  │     │  ├→ Show random dialogue
  │     │  ├→ Play pour.mp3
  │     │  └→ Auto-close after 10 sec
  │     └→ If active: HideOverlay()
  │
  ├→ TrayService.Initialize()
  │  └→ System tray icon
  │     ├→ Left-click: About dialog
  │     └→ Right-click menu:
  │        ├→ ⚙️ Settings
  │        ├→ ⏸️ Pause/Resume
  │        └→ ❌ Exit
  │
  └→ SettingsService.Load()
     ├→ Read %APPDATA%\ChaiIdle\settings.json
     └→ Load random dialogues by language
```

## Dependencies (Minimal)

- **WPF** (built-in with .NET)
- **System.Windows.Forms** (for NotifyIcon)
- **Newtonsoft.Json** (NuGet package)
- **user32.dll** (Windows API for idle detection)

**No heavy frameworks = Lightweight & Fast! ⚡**

## Configuration Hierarchy

Settings are loaded in this order (first found wins):

1. `%APPDATA%\ChaiIdle\settings.json` (user config)
2. Hardcoded defaults in `SettingsService.cs`

Dialogues:
1. `%APPDATA%\ChaiIdle\dialogues.json` (user custom)
2. Hardcoded defaults in `SettingsService.cs`

## Deployment Targets

### Option 1: Single .exe (Recommended)
- Self-contained: includes .NET runtime
- No installation needed
- Portable: works on any Windows 10/11
- Size: ~80-120MB

### Option 2: Framework-dependent
- Small: ~2MB executable
- Requires: .NET 8 pre-installed on target
- Size: ~80-120MB total (with runtime)

### Option 3: Installer (Future)
- Create MSI/NSIS installer
- System-wide installation
- Windows Add/Remove Programs

---

**Ready to build? Start with [QUICKSTART.md](QUICKSTART.md)! 🍵**
