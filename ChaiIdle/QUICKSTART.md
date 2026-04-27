# 🚀 QUICK START - ChaiIdle / OruTeaDa

## ⚡ 30-Second Setup

### 1. Build the Project
```bash
cd ChaiIdle
dotnet build -c Release
```

### 2. Run It
```bash
dotnet run
# Or:
./bin/Release/net8.0-windows/ChaiIdle.exe
```

### 3. Test Idle Detection
- App starts and hides to system tray
- Don't touch mouse/keyboard for 5 minutes
- 🍵 Overlay should appear!
- Click overlay or press any key → closes

## 🎯 First Test Run

1. **Run the app**
   ```bash
   cd ChaiIdle
   dotnet run
   ```

2. **Look for system tray** (bottom-right corner)
   - Should see tea icon or generic icon
   - Double-click to see about dialog

3. **Right-click tray icon**
   - ⚙️ Settings
   - ⏸️ Pause
   - ℹ️ About
   - ❌ Exit

4. **To test overlay quickly** (change idle time to 1 min):
   ```
   Edit: %APPDATA%\ChaiIdle\settings.json
   Change: "idleMinutes": 1
   Restart app
   Wait 1 minute without moving mouse/keyboard
   🍵 Overlay appears!
   ```

## 🎬 For Animation & Sound

Place these files in the `Assets/` folder:

**tea.mp4** (animation)
- Format: MP4 (H.264 codec)
- Size: 1920x1080 or smaller
- Duration: 10-15 seconds
- Background: transparent or solid
- Where: `ChaiIdle/Assets/tea.mp4`

**pour.mp3** (ASMR sound)
- Format: MP3 (192kbps)
- Duration: 3-5 seconds
- Style: Realistic tea pouring/ASMR
- Where: `ChaiIdle/Assets/pour.mp3`

**tea.ico** (tray icon)
- Format: ICO (or PNG converted to ICO)
- Size: 256x256 pixels
- Where: `ChaiIdle/Assets/tea.ico` (optional)

## 📦 Build Single .exe (Viral Launch)

```bash
dotnet publish -c Release -r win-x64 ^
  --self-contained true ^
  /p:PublishSingleFile=true ^
  /p:IncludeNativeLibrariesForSelfExtract=true
```

Output: `bin\Release\net8.0-windows\win-x64\publish\ChaiIdle.exe`

**This .exe runs anywhere on Windows 10/11 without installation!**

## ⚙️ Configuration Files

These are created automatically in:
```
C:\Users\YourName\AppData\Roaming\ChaiIdle\
```

### settings.json
```json
{
  "idleMinutes": 5,
  "enabledLanguages": ["Tamil", "English", "Hinglish"],
  "soundEnabled": true,
  "isPaused": false
}
```

### dialogues.json
Add custom memes here!

```json
{
  "Tamil": [
    "Your custom dialogue here da!",
    "Enna irundhum chai better da!"
  ]
}
```

## 🔍 Debugging

### Check if idle detector is working:
```bash
# Run and keep console open
dotnet run
# Should see: [ChaiIdle] Idle detected! after idle time
```

### Check settings file:
```
C:\Users\YourName\AppData\Roaming\ChaiIdle\settings.json
```

### Check if overlay appears:
1. Set `idleMinutes: 1` in settings.json
2. Run app
3. Don't touch mouse/keyboard for 1 minute
4. Look for overlay (might be off-screen if screen is small)

## 🎮 Manual Overlay Test

Want to test overlay immediately without waiting 5 min?

Edit [MainWindow.xaml.cs](MainWindow.xaml.cs) temporarily:

```csharp
// In ShowOverlay() method, add this:
if (_overlayWindow == null)
{
    _overlayWindow = new OverlayWindow(_settingsService!);
    // ... rest of code
}

// Immediately show for testing:
_overlayWindow.Show();  // <-- Add this line
```

Then rebuild and run. Overlay will show immediately.

## 📱 Testing Multi-Language

Edit `settings.json`:
```json
"enabledLanguages": ["Tamil", "English", "Hinglish", "Telugu"]
```

Each time overlay appears, it picks a random enabled language!

## 🚀 Next Steps

1. ✅ Build and run locally
2. 🎬 Add MP4 animation to `Assets/`
3. 🎵 Add MP3 sound to `Assets/`
4. 🔥 Build single .exe
5. 📤 Share on Twitter/Reddit with #ChaiIdle
6. 🎉 Go viral!

## 📞 Need Help?

- Check `README.md` for features
- Check `DEVELOPMENT.md` for architecture
- Check `BUILD.md` for build options
- Check console output for errors

---

**Da, seri ba! Now go make the most viral chai app ever! 🍵**
