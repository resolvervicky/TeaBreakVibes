# ChaiIdle - Directory Setup Instructions

## ✅ Files Created Successfully

Your ChaiIdle project is now ready to build! Here's what was created:

### Project Structure
```
E:\SaaS\ResolverVicky\Apps\resolvervicky\TeaBreakVibes\ChaiIdle\
├── ChaiIdle.csproj                    ✅
├── App.xaml                            ✅
├── App.xaml.cs                         ✅
├── MainWindow.xaml                     ✅
├── MainWindow.xaml.cs                  ✅
├── OverlayWindow.xaml                  ✅
├── OverlayWindow.xaml.cs               ✅
├── IdleDetector.cs                     ✅
├── SettingsService.cs                  ✅
├── TrayService.cs                      ✅
├── README.md                           ✅
├── QUICKSTART.md                       ✅
├── BUILD.md                            ✅
├── DEVELOPMENT.md                      ✅
├── STRUCTURE.md                        ✅
├── build.bat                           ✅
├── build.sh                            ✅
├── AppData_settings_template.json      ✅
└── Assets/
    └── dialogues_template.json         ✅
```

## 🚀 Next Steps

### 1. Build the Project (Windows)

**Option A: Using Build Script**
```bash
cd ChaiIdle
.\build.bat
# Choose option 3 for single-file .exe
```

**Option B: Manual Build**
```bash
cd ChaiIdle
dotnet build -c Release
```

**Option C: Publish as Single .exe (Viral Launch!)**
```bash
cd ChaiIdle
dotnet publish -c Release -r win-x64 ^
  --self-contained true ^
  /p:PublishSingleFile=true ^
  /p:IncludeNativeLibrariesForSelfExtract=true
```

### 2. Run It
```bash
# After build:
dotnet run

# Or run .exe directly:
.\bin\Release\net8.0-windows\ChaiIdle.exe
```

### 3. Add Assets (Optional but Recommended)

Create or download these files and place in `Assets/` folder:

- **tea.mp4** - Tea pouring animation (MP4, 10-15 sec, transparent background)
- **pour.mp3** - Tea pouring sound (MP3, 3-5 sec, ASMR style)
- **tea.ico** - Tray icon (ICO, 256x256)

**Without assets**: App will work fine! Falls back to emoji (🍵) and silent mode.

### 4. Test

Don't touch mouse/keyboard for 5 minutes:
- Overlay should appear
- Dialogue should show
- Sound should play (if pour.mp3 exists)
- Click overlay or press any key to close

To speed up testing, edit:
```
%APPDATA%\ChaiIdle\settings.json
Change "idleMinutes": 5 to 1
```

Then wait just 1 minute instead!

## 📋 Checklist

- [ ] Cloned/downloaded all files
- [ ] Ran `dotnet build -c Release`
- [ ] Ran app successfully
- [ ] Saw overlay after idle time
- [ ] Added MP4/MP3 assets (optional)
- [ ] Built single-file .exe
- [ ] Tested on clean Windows machine
- [ ] Ready to share with the world! 🎉

## 🆘 Troubleshooting

### Error: "Could not find .NET SDK"
**Fix:** Install .NET 8 from https://dotnet.microsoft.com/download/dotnet/8.0

### Error: "AssemblyInfo not found"
**Fix:** Run `dotnet restore` first

### Overlay not showing?
**Fix:** 
1. Check if app is paused (right-click tray > Resume)
2. Change idle time to 1 minute in settings.json
3. Keep console open to see error messages

### No sound?
**Fix:**
1. Add `pour.mp3` to `Assets/` folder
2. Or disable in settings: `"soundEnabled": false`

### High CPU usage?
**Fix:** Likely caused by video codec. Use static image instead of MP4.

## 📞 File Locations to Remember

### Settings
```
C:\Users\{YourName}\AppData\Roaming\ChaiIdle\settings.json
```

### Custom Dialogues
```
C:\Users\{YourName}\AppData\Roaming\ChaiIdle\dialogues.json
```

### Built .exe (Single File - Easiest to Share)
```
E:\SaaS\ResolverVicky\Apps\resolvervicky\TeaBreakVibes\ChaiIdle\
bin\Release\net8.0-windows\win-x64\publish\ChaiIdle.exe
```

## 🎯 Quick Commands

```bash
# Navigate to project
cd E:\SaaS\ResolverVicky\Apps\resolvervicky\TeaBreakVibes\ChaiIdle

# Build debug
dotnet build -c Debug

# Build release
dotnet build -c Release

# Run
dotnet run

# Publish single-file exe (copy this entire command)
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true /p:DebugType=embedded /p:PublishTrimmed=false

# Run the published exe
.\bin\Release\net8.0-windows\win-x64\publish\ChaiIdle.exe
```

## 📖 Documentation Quick Links

| Document | Purpose |
|----------|---------|
| **QUICKSTART.md** | Start here! 30-second setup |
| **README.md** | Full features & user guide |
| **BUILD.md** | Detailed build commands |
| **DEVELOPMENT.md** | Architecture & tech details |
| **STRUCTURE.md** | Project structure walkthrough |

## 🎉 Ready to Go Viral!

1. ✅ All code files created
2. ✅ Project compiles
3. ✅ Idle detection works
4. ✅ Overlay shows
5. ✅ Single-file .exe ready
6. ⬜ Add MP4/MP3 assets
7. ⬜ Publish & share!

---

**Start with:**
```bash
cd ChaiIdle
dotnet build -c Release
dotnet run
```

**Don't touch mouse/keyboard for 5 minutes, then watch the magic! 🍵✨**

Da, oru chai vadikka sollrindhu thiyanum ba!
